using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using Bio;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Bio.IO;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using Bio.SimilarityMatrices;
using Bio.IO.FastA;
using System.Windows.Forms.DataVisualization.Charting;


namespace PhylogeneticTree
{
    public partial class Form1 : Form
    {
        List<ISequence> isequence = new List<ISequence>();
        List<ISequence> alignedSeqs = new List<ISequence>();

        public FormSaved frmSaved = new FormSaved();
        public ArrayList table = new ArrayList();
        public List<string> sequenceArray = new List<string>();
        public string result;
        private ArrayList newNodes = new ArrayList();
        public int SEQ_LENGTH = 0;
        public int IDLength = 0;
        public string preferredImageType;
        public double VarianceThreshold = 0.002;
        public Boolean showNotification1, showNotification2, showAdditionalInfo;
        public int numberOfSequences, originalScore, improvedScore;
        const string Filename = "output.fasta";
        ISequenceFormatter formatter = new Bio.IO.FastA.FastAFormatter();
        public int numberOfDNAColumns = 0;

        int open_gap, extend_gap, pp;
        Boolean CoVariance, Euclidean, Pearson, IgnoreGaps, cancelThreads;
        public string Algorithm, lblParsimonyScoreOrigString, txtExtimatedTreeOrigString, lblProcessTimeOrigString,
            lblTimeImprovedString, lblParsimonyScoreImprovedString, txtEstimatedTreeFastString;
        DistanceFunctionTypes distanceFunctionName = new DistanceFunctionTypes();
        ProfileAlignerNames profileAlignerName = new ProfileAlignerNames();
        int loadAttempts = 0;
        private struct indexParsimonyScore
        {
            public int index;
            public int score;

        }

        public Form1()
        {
            InitializeComponent();

        }

        public Form1(string[] arg)
        {
            InitializeComponent();
            //if(arg.Length > 0)
            //    txtDatasetPath.Text = arg[0];
            //load();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages[3].Tag = tabControl1.TabPages[4].Tag = "empty";
            
            //if (DateTime.Now > new DateTime(2016, 4, 29))
            //{
            //    MessageBox.Show("Please Contact Administrator","Technical Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //    this.Close();
            //    Application.Exit();
            //    Application.ExitThread();
            //}

            CheckForIllegalCrossThreadCalls = false;
            toolStripButtonRun.Enabled = false;
            timer1.Enabled = true;
            
            cmxGraphType.SelectedIndex = 0;
            loadSettings();
        }

        /// <summary>
        /// Aligns multiple sequences using a multiple sequence aligner.
        /// This sample uses PAMSAM with a set of default parameters.
        /// </summary>
        /// <param name="sequences">List of sequences to align.</param>
        /// <returns>List of ISequenceAlignment</returns>
        private PAMSAMMultipleSequenceAligner DoMultipleSequenceAlignment(List<ISequence> sequences)
        {

            lblCount.Refresh();
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int kmerLength = 3;
            lblCount.Refresh();
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            lblCount.Refresh();

            lblCount.Refresh();

            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;
            lblCount.Refresh();
            // Call aligner
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                (sequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, open_gap, extend_gap,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);
            lblCount.Refresh();
            return msa;
        }//end of DoMultipleSequenceAlignment        

        private string getSequence(ISequence iseq)
        {
            byte[] bArray = new byte[SEQ_LENGTH];
            string str = "";
            bArray = iseq.ToArray();
            foreach (byte b in bArray)
                str += ((char)b);
            return str;
        }


        private int[] Leaves(string subTree)
        {
            int numberOfLeaves = 1;
            for (int i = 0; i < subTree.Length; i++)
                if (subTree[i] == ',')
                    numberOfLeaves++;
            int[] intArray = new int[numberOfLeaves];
            int startIndex, lastIndex, count;
            startIndex = lastIndex = count = 0;
            for (int i = 0; i < subTree.Length; i++)
            {
                if (subTree[i] >= '0' && subTree[i] <= '9')
                {
                    startIndex = i;
                    while (subTree[i] >= '0' && subTree[i] <= '9')
                        i++;
                    lastIndex = i - 1;
                    intArray[count++] = Convert.ToInt32(subTree.Substring(startIndex, lastIndex - startIndex + 1));
                }

            }//end of for
            return intArray;
        }//end of Leaves

        private string IntToACGT(string subTree, ref int[] leaves, int index)
        {
            for (int i = 0; i < leaves.Count(); i++)
            {
                subTree = subTree.Replace(addZero(leaves[i]), (sequenceArray[leaves[i]][index]).ToString());
                //byte[] bArray = isequence.ElementAt(leaves[i]).ToArray();
                //subTree = subTree.Replace(leaves[i].ToString(), ((char)bArray[index]).ToString());
            }
            return subTree;

        }//end of method

        private int PS(string subTree)
        {
            int score = 0;
            int[] leaves = Leaves(subTree);
            string strTemp = "";
            string temp = "";
            string left, right;

            for (int i = 0; i < SEQ_LENGTH; i++)
            {
                strTemp = IntToACGT(subTree, ref leaves, i);
                //strTemp = "((C,A),(C,(A,G)))";

                while (strTemp[0] == '(')
                {
                    while (strTemp.Contains("N") || strTemp.Contains("(A,A)") || strTemp.Contains("(C,C)") ||
                    strTemp.Contains("(G,G)") || strTemp.Contains("(T,T)"))
                    {
                        int Switch = strTemp.Length;
                        strTemp = strTemp.Replace("(A,A)", "A");
                        strTemp = strTemp.Replace("(C,C)", "C");
                        strTemp = strTemp.Replace("(G,G)", "G");
                        strTemp = strTemp.Replace("(T,T)", "T");
                        strTemp = strTemp.Replace("(A,N)", "A");
                        strTemp = strTemp.Replace("(N,A)", "A");
                        strTemp = strTemp.Replace("(C,N)", "C");
                        strTemp = strTemp.Replace("(N,C)", "C");
                        strTemp = strTemp.Replace("(G,N)", "G");
                        strTemp = strTemp.Replace("(N,G)", "G");
                        strTemp = strTemp.Replace("(T,N)", "T");
                        strTemp = strTemp.Replace("(N,T)", "T");
                        strTemp = strTemp.Replace("(N,N)", "N");
                        //(strTemp.Contains("),N") || strTemp.Contains("N,(")) and leads to un-break loop
                        //for example: strTemp = (((((C,((C,T),N)),C),C),((G,C),C)),C)
                        if (strTemp.Length == 1 || Switch == strTemp.Length)
                            break;
                    }//end of while

                    //scan for number of hamzadhaye barg
                    int numberOfHamzadhayeBarg = 0;
                    for (int j = 0; j < strTemp.Length; j++)
                        if (strTemp[j] == ',' && strTemp[j + 1] != '(' && strTemp[j - 1] != ')')
                            numberOfHamzadhayeBarg++;
                    int[] indexOfCommas = new int[numberOfHamzadhayeBarg];
                    int k = 0;
                    for (int j = 0; j < strTemp.Length; j++)
                        if (strTemp[j] == ',' && strTemp[j + 1] != '(' && strTemp[j - 1] != ')')
                            indexOfCommas[k++] = j;

                    for (int j = k - 1; j >= 0; j--)
                    {
                        int comma = indexOfCommas[j];
                        if (comma >= strTemp.Length)
                            break;
                        while (strTemp[comma] != ')')
                            comma++;
                        int finish = comma;
                        comma = indexOfCommas[j];
                        while (strTemp[comma] != '(' && comma > 0)
                            comma--;
                        int start = comma;
                        comma = indexOfCommas[j];
                        left = strTemp.Substring(start + 1, comma - start - 1);
                        right = strTemp.Substring(comma + 1, finish - comma - 1);

                        if (left.Length > right.Length)
                        {
                            temp = left;
                            left = right;
                            right = temp;
                        }

                        temp = "";

                        if (left.ToLower() == "n" || left == "?")
                            temp = right;
                        else
                            for (int t = 0; t < left.Length; t++)
                                if (right.Contains(left[t]))
                                    temp += left[t];

                        if (temp.Length == 0)
                        {
                            temp = left + right;
                            score++;
                        }

                        strTemp = strTemp.Substring(0, start) + temp + strTemp.Substring(finish + 1);

                        //strTemp = strTemp.Replace(strTemp.Substring(start, finish - start + 1), temp);

                    }//end of for

                }//end of while

            }//end of for i

            return score;
        }//end of PS

        private string addZero(string str)
        {

            if (str.Length == 3)
                str = "0" + str;
            if (str.Length == 2)
                str = "00" + str;
            if (str.Length == 1)
                str = "000" + str;

            return str;
        }

        private string addZero(int a)
        {
            string str = a.ToString();
            if (str.Length == 3)
                str = "0" + str;
            if (str.Length == 2)
                str = "00" + str;
            if (str.Length == 1)
                str = "000" + str;

            return str;
        }

        private int originalStepWiseAddition()
        {
            Random random = new Random();
            int[] nodes = new int[isequence.Count()];

            for (int i = 0; i < isequence.Count(); i++)
                nodes[i] = i;

            for (int i = 0; i < nodes.Count(); i++)
            {
                int j = random.Next(nodes.Count());
                int k = nodes[i];
                nodes[i] = nodes[j];
                nodes[j] = k;
            }

            int pbSteps = 2;
            pbWaiting.Value = pbSteps * 100 / isequence.Count();
            pbWaiting.Refresh();

            string subTree = "(" + addZero(nodes[0]) + "," + addZero(nodes[1]) + ")";
            lblCount.Text = "2";
            //int tep = PS("(1,((2,3),5))");
            int minPSindex = -1;
            for (int j = 2; j < nodes.Count(); j++)
            {
                if (cancelThreads)
                    Thread.CurrentThread.Abort();
                int[] scores = new int[j];

                for (int k = 0; k < j; k++)
                {
                    scores[k] = PS(subTree.Replace(addZero(nodes[k]), "(" + addZero(nodes[k]) + "," + addZero(nodes[j]) + ")"));
                }//end of for k
                minPSindex = 0;
                for (int k = 1; k < j; k++)
                    if (scores[k] < scores[minPSindex])
                        minPSindex = k;
                    else
                        if (scores[k] == scores[minPSindex] && random.Next(0, 2) == 1)
                            minPSindex = k;


                subTree = subTree.Replace(addZero(nodes[minPSindex]), "(" + addZero(nodes[minPSindex]) + "," + addZero(nodes[j]) + ")");
                minPSindex = scores[minPSindex];

                lblCount.Text = (j + 1).ToString();
                pbWaiting.Value = ++pbSteps * 100 / isequence.Count();

            }//end of for j

            lblParsimonyScoreOrigString = "Parsimony Score: " + minPSindex.ToString();
            txtExtimatedTreeOrigString = DeleteZero(subTree);

            return minPSindex;
        }//end of method originalStepWiseAddition


        private void runMode()
        {
            toolStripButtonRun.Visible =
                saveToolStripMenuItem.Enabled =
                openToolStripMenuItem.Enabled = toolStripButtonSettings.Enabled =
                settingsToolStripMenuItem1.Enabled = false;
            toolStripButtonStop.Visible = pnlWaiting.Visible = true;
        }//end of runMode

        private void regMode()
        {
            toolStripButtonRun.Visible=
                saveToolStripMenuItem.Enabled =
                openToolStripMenuItem.Enabled = toolStripButtonSettings.Enabled =
                settingsToolStripMenuItem1.Enabled = true;
            toolStripButtonStop.Visible = false;
        }//end of regMode

        private string getConsensus(cluster clust)
        {
            string str;
            string result = "";
            clust.ACGT(SEQ_LENGTH, ref sequenceArray);
            for (int i = 0; i < SEQ_LENGTH; i++)
            {
                str = "";
                foreach (int n in clust.leaves)
                    if (n != -1)
                        str += sequenceArray[n][i];

                Boolean A, C, G, T;
                A = C = G = T = false;
                if (clust.A(i) < pp && clust.C(i) < pp && clust.G(i) < pp && clust.T(i) < pp)
                {
                    if (clust.A(i) == clust.C(i) && clust.G(i) == clust.T(i) && clust.A(i) == clust.T(i))
                        A = C = G = T = true;
                    else
                    {
                        if (clust.A(i) > 0)
                            A = true;
                        if (clust.C(i) > 0)
                            C = true;
                        if (clust.G(i) > 0)
                            G = true;
                        if (clust.T(i) > 0)
                            T = true;
                    }
                }
                else
                {
                    if (str.Contains((char)Alphabets.AmbiguousDNA.A) && clust.A(i) >= pp)
                        A = true;
                    if (str.Contains((char)Alphabets.AmbiguousDNA.C) && clust.C(i) >= pp)
                        C = true;
                    if (str.Contains((char)Alphabets.AmbiguousDNA.G) && clust.G(i) >= pp)
                        G = true;
                    if (str.Contains((char)Alphabets.AmbiguousDNA.T) && clust.T(i) >= pp)
                        T = true;
                }

                if (A)
                {
                    if (C && G)
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.Any;
                        else
                            result += (char)Alphabets.AmbiguousDNA.GCA;

                    if (C && !G)
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.ACT;
                        else
                            result += (char)Alphabets.AmbiguousDNA.AC;

                    if (!C && G)
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.GAT;
                        else
                            result += (char)Alphabets.AmbiguousDNA.GA;

                    if (!C && !G)
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.AT;
                        else
                            result += (char)Alphabets.AmbiguousDNA.A;
                }//end of if contains(A)...

                if (!A && C)
                {
                    if (G)
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.GTC;
                        else
                            result += (char)Alphabets.AmbiguousDNA.GC;
                    else//!G
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.TC;
                        else
                            result += (char)Alphabets.AmbiguousDNA.C;

                }//end of contains(C) && !(A)

                if (!A && !C)
                {
                    if (G)
                        if (T)
                            result += (char)Alphabets.AmbiguousDNA.GT;
                        else
                            result += (char)Alphabets.AmbiguousDNA.G;
                    else
                        result += (char)Alphabets.AmbiguousDNA.T;
                }


            }//end of for i=0...
            return result;
        }//end of getConsensus

        private void updateConsensus(cluster clust, string newSeq)
        {
            string consensus = "";
            int length = clust.consensus.Length - newSeq.Length;
            if (length < 0)
                for (int i = length; i < 0; i++)
                    clust.consensus += (char)Alphabets.AmbiguousDNA.Any;

            for (int i = 0; i < newSeq.Length; i++)
            {
                string str = clust.consensus[i].ToString() + newSeq[i].ToString();

                switch (str[0])
                {
                    case 'A':
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.A;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.AC;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GA;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.AT;
                        break;
                    case 'C':
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.AC;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.C;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GC;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.TC;
                        break;
                    case 'G':
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.GA;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.GC;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.G;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.GT;
                        break;
                    case 'T':
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.AT;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.TC;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GT;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.T;
                        break;
                    case 'M'://A or C
                        if (str[1] == 'A' || str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.AC;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GCA;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.ACT;
                        break;
                    case 'R'://Purine A or G
                        if (str[1] == 'A' || str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GA;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.GCA;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.GAT;
                        break;
                    case 'W'://Weak A or T
                        if (str[1] == 'A' || str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.AT;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.ACT;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GAT;
                        break;
                    case 'S'://Strong C or G
                        if (str[1] == 'C' || str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GC;
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.GCA;
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.GTC;
                        break;
                    case 'Y'://Pyrimidine C or T
                        if (str[1] == 'C' || str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.TC;
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.ACT;
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.GTC;
                        break;
                    case 'K'://G or T
                        if (str[1] == 'G' || str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.GT;
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.GAT;
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.GTC;
                        break;
                    case 'V'://A or C or G (NOT T)
                        if (str[1] == 'T')
                            consensus += (char)Alphabets.AmbiguousDNA.Any;
                        else
                            consensus += (char)Alphabets.AmbiguousDNA.GCA;
                        break;
                    case 'H'://A or C or T (NOT G)
                        if (str[1] == 'G')
                            consensus += (char)Alphabets.AmbiguousDNA.Any;
                        else
                            consensus += (char)Alphabets.AmbiguousDNA.ACT;
                        break;
                    case 'D'://A or G or T (NOT C)
                        if (str[1] == 'C')
                            consensus += (char)Alphabets.AmbiguousDNA.Any;
                        else
                            consensus += (char)Alphabets.AmbiguousDNA.GAT;
                        break;
                    case 'B'://C or G or T (NOT A)
                        if (str[1] == 'A')
                            consensus += (char)Alphabets.AmbiguousDNA.Any;
                        else
                            consensus += (char)Alphabets.AmbiguousDNA.GTC;
                        break;
                    default:
                        consensus += (char)Alphabets.AmbiguousDNA.Any;
                        break;

                }//end of switch
            }//end of for i=0...
            clust.consensus = consensus;
        }//end of updateConsensus

        private void calculateConsensusForClusters(cluster clust)
        {
            clust.consensus = getConsensus(clust);
        }


        private void calculateConsensusForClusters(ArrayList clusters)
        {
            for (int i = 0; i < clusters.Count; i++)
                ((cluster)clusters[i]).consensus = getConsensus((cluster)clusters[i]);
        }

        private string BreakeCluster(string subTree)
        {
            int paranteseBaz, comma;
            paranteseBaz = comma = 0;
            for (int i = 0; i < subTree.Length; i++)
            {
                if (subTree[i] == '(')
                    paranteseBaz++;
                if (subTree[i] == ')')
                    paranteseBaz--;
                if (subTree[i] == ',' && paranteseBaz == 1)
                { comma = i; break; }

            }//end of for i

            return subTree.Substring(1, comma - 1) + "*" + subTree.Substring(comma + 1, subTree.Length - comma - 2);
        }//end of BreakCluster

        private string DeleteZero(string str)
        {
            string strTemp = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    strTemp = str.Substring(i, 4);
                    if (strTemp[0] == '0')
                        str = str.Replace(strTemp, Convert.ToInt32(strTemp).ToString());

                    while (i < str.Length && str[i] != ',')
                        i++;
                }
            }//end of for
            return str;
        }//end of DeleteZero


        private void ImprovedStepwiseAddition()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //table = new ArrayList();
            //newNodes = new ArrayList();
            pbWaiting.Value = 0; lblCount.Text = "0";
            int pbSteps = 1;
            pnlWaiting.Visible = true;
            label1.Text = "Clustering is currently being done."; label1.Refresh();
            lblCount.Refresh(); pbWaiting.Refresh(); label4.Refresh(); lblTotal.Refresh(); label5.Refresh();
            //
            Random random = new Random();

            int[] nodes = new int[isequence.Count()];

            for (int i = 0; i < isequence.Count(); i++)
                nodes[i] = i;


            pbWaiting.Value = pbSteps * 100 / isequence.Count();
            lblCount.Refresh(); pbWaiting.Refresh();

            string subTree = "(" + addZero(nodes[0]) + "," + addZero(nodes[1]) + ")";
            lblCount.Text = "2"; lblCount.Refresh();
            //int tep = PS("(1,((2,3),5))");
            int minPSindex = -1;
            txtClusters.Text = "Number of clusters: 1";
            for (int j = 2; j < 3; j++)
            {
                if (cancelThreads)
                    Thread.CurrentThread.Abort();

                int[] scores = new int[j];
                for (int k = 0; k < j; k++)
                {
                    scores[k] = PS(subTree.Replace(addZero(nodes[k]), "(" + addZero(nodes[k]) + "," + addZero(nodes[j]) + ")"));
                }//end of for k

                minPSindex = 0;
                for (int k = 1; k < j; k++)
                    if (scores[k] < scores[minPSindex])
                        minPSindex = k;
                    else
                        if (scores[k] == scores[minPSindex] && random.Next(0, 2) == 1)
                            minPSindex = k;


                subTree = subTree.Replace(addZero(nodes[minPSindex]), "(" + addZero(nodes[minPSindex]) + "," + addZero(nodes[j]) + ")");
                minPSindex = scores[minPSindex];

                pbWaiting.Value = ++pbSteps * 100 / nodes.Length;
                lblCount.Text = j.ToString();
                pbWaiting.Refresh();
                lblCount.Refresh();
            }//end of for j

            lblCount.Refresh(); pbWaiting.Refresh();
            cluster clust = new cluster(subTree);
            calculateConsensusForClusters(clust);
            clust.calculateVariance(SEQ_LENGTH, ref sequenceArray);
            lblCount.Refresh();
            ArrayList clusters = new ArrayList(50);
            clusters.Add(clust);
            int index = 3;
            for (int i = 3; i < nodes.Length; i++)
            {
                if (cancelThreads)
                    Thread.CurrentThread.Abort();
                lblCount.Text = i.ToString(); lblCount.Refresh();
                pbWaiting.Value = ++pbSteps * 100 / nodes.Length;
                pbWaiting.Refresh();
                index = i;
                int[] scores = new int[clust.NumberOfLeaves];
                for (int j = 0; j < clust.NumberOfLeaves; j++)
                {

                    scores[j] = PS(subTree.Replace(addZero(clust.leaves[j]), "(" + addZero(clust.leaves[j]) + "," + addZero(nodes[i]) + ")"));

                }//end of for j
                lblCount.Refresh();
                minPSindex = 0;
                for (int k = 1; k < clust.NumberOfLeaves; k++)
                    if (scores[k] < scores[minPSindex])
                        minPSindex = k;
                    else
                        if (scores[k] == scores[minPSindex] && random.Next(0, 2) == 1)
                            minPSindex = k;

                subTree = subTree.Replace(addZero(clust.leaves[minPSindex]), "(" + addZero(clust.leaves[minPSindex]) + "," + addZero(nodes[i]) + ")");
                clust.tree = clust.tree.Replace(addZero(clust.leaves[minPSindex]), "(" + addZero(clust.leaves[minPSindex]) + "," + addZero(nodes[i]) + ")");
                clust.leaves[clust.NumberOfLeaves++] = nodes[i];
                updateConsensus(clust, sequenceArray[nodes[i]]);
                clust.calculateVariance(SEQ_LENGTH, ref sequenceArray);
                if (cancelThreads)
                    Thread.CurrentThread.Abort();
                if (clust.variance > VarianceThreshold)
                {
                    string strtemp = BreakeCluster(clust.tree);

                    cluster clust1 = new cluster(strtemp.Substring(0, strtemp.IndexOf('*')));
                    cluster clust2 = new cluster(strtemp.Substring(strtemp.IndexOf('*') + 1));
                    clusters.Add(clust1);
                    clusters.Add(clust2);
                    calculateConsensusForClusters(clust2);
                    calculateConsensusForClusters(clust1);
                    clust1.calculateVariance(SEQ_LENGTH, ref sequenceArray);
                    clust2.calculateVariance(SEQ_LENGTH, ref sequenceArray);
                    clusters.Remove(clust);
                    txtClusters.Text = "Number of clusters: 2";
                    txtClusters.Refresh();
                    break;
                }//end of if


                minPSindex = scores[minPSindex];

            }//end of for i

            lblCount.Refresh();

            PAMSAMMultipleSequenceAligner pamsamMulti = new PAMSAMMultipleSequenceAligner();
            lblCount.Refresh();
            pamsamMulti.GapExtensionCost = extend_gap;
            pamsamMulti.GapOpenCost = open_gap;
            pamsamMulti.DistanceFunctionName = distanceFunctionName;
            pamsamMulti.ProfileAlignerName = profileAlignerName;
            pamsamMulti.KmerLength = 3;
            float alignmentScore = 0;

            for (int i = index + 1; i < nodes.Length; i++)
            {
                if (cancelThreads)
                    Thread.CurrentThread.Abort();
                float[] clustersScores = new float[clusters.Count];
                lblCount.Text = i.ToString();
                lblCount.Refresh();
                for (int j = 0; j < clusters.Count; j++)
                {
                    Sequence seq = new Sequence(Alphabets.AmbiguousDNA, ((cluster)clusters[j]).consensus);
                    alignmentScore = calculateDistance(seq, new Sequence(Alphabets.AmbiguousDNA, sequenceArray[nodes[i]])); //(ISequence)(sequenceArray[nodes[i]])); //table[(int)newNodes[i]]));
                    clustersScores[j] = alignmentScore;
                }
                lblCount.Refresh();
                pbWaiting.Value = ++pbSteps * 100 / nodes.Length;
                pbWaiting.Refresh();
                int k = 0;//index of first score in clustersScores
                alignmentScore = clustersScores[k];
                for (int j = 1; j < clustersScores.Length; j++)
                    if (clustersScores[j] > alignmentScore)
                    { alignmentScore = clustersScores[j]; k = j; }

                //add to clusters[k]
                minPSindex = -1;
                cluster selectedCluster = (cluster)clusters[k];
                int[] scores = new int[selectedCluster.NumberOfLeaves];
                for (int j = 0; j < selectedCluster.NumberOfLeaves; j++)
                {

                    scores[j] = PS(subTree.Replace(addZero(selectedCluster.leaves[j]), "(" + addZero(selectedCluster.leaves[j]) + "," + addZero(nodes[i]) + ")"));

                }//end of for k
                lblCount.Refresh();
                minPSindex = 0;
                for (k = 1; k < selectedCluster.NumberOfLeaves; k++)
                    if (scores[k] < scores[minPSindex])
                        minPSindex = k;
                    else
                        if (scores[k] == scores[minPSindex] && random.Next(0, 2) == 1)
                            minPSindex = k;

                subTree = subTree.Replace(addZero(selectedCluster.leaves[minPSindex]), "(" + addZero(selectedCluster.leaves[minPSindex]) + "," + addZero(nodes[i]) + ")");
                selectedCluster.tree = selectedCluster.tree.Replace(addZero(selectedCluster.leaves[minPSindex]), "(" + addZero(selectedCluster.leaves[minPSindex]) + "," + addZero(nodes[i]) + ")");
                selectedCluster.leaves[selectedCluster.NumberOfLeaves++] = nodes[i];
                updateConsensus(selectedCluster, sequenceArray[nodes[i]]);
                selectedCluster.calculateVariance(SEQ_LENGTH, ref sequenceArray);

                if (selectedCluster.variance > VarianceThreshold)
                {
                    string strtemp = BreakeCluster(selectedCluster.tree);
                    cluster clust1 = new cluster(strtemp.Substring(0, strtemp.IndexOf('*')));
                    cluster clust2 = new cluster(strtemp.Substring(strtemp.IndexOf('*') + 1));
                    clusters.Add(clust1);
                    clusters.Add(clust2);
                    calculateConsensusForClusters(clust2);
                    calculateConsensusForClusters(clust1);
                    clust1.calculateVariance(SEQ_LENGTH, ref sequenceArray);
                    clust2.calculateVariance(SEQ_LENGTH, ref sequenceArray);
                    clusters.Remove(selectedCluster);
                    txtClusters.Text = "Number of clusters: " + clusters.Count.ToString();
                    txtClusters.Refresh();
                }//end of if


                minPSindex = scores[minPSindex];


            }//end of for i = VarianceThreshold ....


            stopWatch.Stop();
            pnlWaiting.Visible = false;
            lblTimeImprovedString = "Process time: " + (int)stopWatch.Elapsed.TotalSeconds + " seconds";
            pnlWaiting.Visible = false;
            lblParsimonyScoreImprovedString = "Parsimony Score: " + minPSindex.ToString();
            //txtEstimatedTreeFastString = DeleteZero(subTree);
            txtClusters.Text = "Number of clusters: " + clusters.Count.ToString() + "\r\n" +
                lblTimeImprovedString + "\r\n" + "Variance proportions: ";
            for (int i = 0; i < clusters.Count; i++)
            {
                txtClusters.Text += "{Cluster " + (i + 1).ToString() + ": " +
                    ((cluster)clusters[i]).variance.ToString("0.######") + "} ; ";
            }//end of for i
            txtClusters.Refresh();

            DrawTree dt = new DrawTree();
            foreach (object obj in clusters)
            {
                txtClusters.Text += "\r\n\r\n------------------------\r\n";
                int k = 0;
                cluster myCluster = (cluster)obj;
                ArrayList lines = new ArrayList(2 * myCluster.NumberOfLeaves - 1);
                lines = dt.Draw(myCluster.tree);
                for (int i = 0; i < lines.Count; i++)
                {
                    if (i % 2 == 0)
                        lines[i] = isequence.ElementAt(myCluster.leaves[k++]).ID.Substring(0, IDLength) + " " +
                            lines[i];
                    else
                    {
                        string tmpstring = " ";
                        for (int j = 0; j < IDLength; j++)
                            tmpstring += " ";
                        lines[i] = tmpstring + lines[i];
                    }

                    txtClusters.Text += "\r\n" + lines[i];
                }//end of for i

                myCluster = null;
                lines = null;
            }//end of foreach

            //label4.Text = "Number of Clusters: " + clusters.Count.ToString();
            regMode();
            improvedScore = minPSindex;
            result = txtClusters.Text;
        }//end of ImprovedStepwiseAddition

        //draw tree graphically
        private void drawTree(string tree)
        {
            Point initialPosition = new Point(15, 14);
            int count = 0;
            ArrayList pointArray = new ArrayList();

            pointArray.Add(initialPosition);
            for (int i = 0; i < tree.Length; i++)
                if (tree[i] == ',')
                {
                    initialPosition.Y = 28 * pointArray.Count + 14;
                    pointArray.Add(initialPosition);
                }

            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.Graphics formGraphics = txtClusters.CreateGraphics();

            count = 0;
            for (int i = 0; i < tree.Length; i++)
            {
                ArrayList selectedCommas = new ArrayList();
                int number = 0;
                for (int j = 0; j < tree.Length; j++)
                    if (tree[j] == ',')
                    {
                        number++;
                        if (tree[j - 1] != ')' && tree[j + 1] != '(')
                            selectedCommas.Add(number);
                    }

                foreach (object obj in selectedCommas)
                {
                    count = (int)obj;
                    Point pt1 = (Point)pointArray[count - 1];
                    Point pt2 = (Point)pointArray[count];
                    Point pt1_2 = new Point(pt1.X, pt1.Y);
                    pt1_2.X = pt2.X + 2 * Math.Abs(pt2.Y - pt1.Y);
                    formGraphics.DrawLine(myPen, pt1, pt1_2);
                    formGraphics.DrawLine(myPen, pt1_2, pt2);
                    pointArray[count - 1] = pt1_2;
                }//end of foreach
                for (int k = selectedCommas.Count - 1; k >= 0; k--)
                {
                    count = (int)selectedCommas[k];
                    for (int j = 0; j < tree.Length; j++)
                    {
                        if (tree[j] == ',')
                            count--;
                        if (count == 0)
                        {
                            int lastIndex, firstIndex;
                            firstIndex = j;
                            while (tree[j] != ')')
                                j++;
                            lastIndex = j;
                            while (tree[firstIndex] != '(')
                                firstIndex--;
                            tree = tree.Substring(0, firstIndex) + "0" + tree.Substring(lastIndex + 1);

                            break;
                        }//end of if
                    }//end of for

                    count = i = 0;
                }//end of foreach

                for (int j = selectedCommas.Count - 1; j >= 0; j--)
                {
                    int n = (int)selectedCommas[j];
                    pointArray.RemoveAt(n);
                }//end of for

                selectedCommas = null;
            }//end of for i


            myPen.Dispose();
            formGraphics.Dispose();

            //treeXY(tree);
        }//end of drawTree

        private string leftStarRight(string subTree)
        {
            if (subTree[0] != '(')
                return subTree;
            int paranteseBaz, comma;
            paranteseBaz = comma = 0;
            for (int i = 0; i < subTree.Length; i++)
            {
                if (subTree[i] == '(')
                    paranteseBaz++;
                if (subTree[i] == ')')
                    paranteseBaz--;
                if (subTree[i] == ',' && paranteseBaz == 1)
                { comma = i; break; }

            }//end of for i

            return subTree.Substring(1, comma - 1) + "*" + subTree.Substring(comma + 1, subTree.Length - comma - 2);
        }//end of BreakCluster


        private float calculateDistance(ISequence sequence1, ISequence sequence2)
        {
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int kmerLength = 3;

            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            //DistanceFunctionTypes distanceFunctionName = new DistanceFunctionTypes();

            //ProfileAlignerNames profileAlignerName = new ProfileAlignerNames();
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;
            // Call aligner
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                (new[] { sequence1, sequence2 }, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, open_gap, extend_gap,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);


            return msa.AlignmentScore;
        }

        private void originalSW()
        {
            int result = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            pbWaiting.Value = 0;
            pnlWaiting.Visible = true;
            label1.Text = "Making tree with original stepwise addition"; label1.Refresh();
            lblCount.Refresh(); pbWaiting.Refresh();
            lblTotal.Refresh(); label4.Refresh(); label5.Refresh();
            pbWaiting.Value = Convert.ToInt32(100 / isequence.Count());
            pbWaiting.Refresh();
            result = originalStepWiseAddition();

            stopWatch.Stop();
            lblProcessTimeOrigString = "Process Time: " + (int)stopWatch.Elapsed.TotalSeconds + " seconds";
            pnlWaiting.Visible = false;

            originalScore = result;

        }//end of OriginalStepWiseAddition

        private void load(bool _importToCurrent = false)
        {

            try
            {
                List<ISequence> loadedSeqs = new List<ISequence>();

                if (!_importToCurrent)//this line was added by Ghavidel
                    isequence.Clear();

                if (txtDatasetPath.Text.ToLower().Contains(".fasta") || txtDatasetPath.Text.ToLower().Contains(".fas") || txtDatasetPath.Text.ToLower().Contains(".faa"))
                {
                    Bio.IO.FastA.FastAParser fastaParser = new Bio.IO.FastA.FastAParser(txtDatasetPath.Text);
                    loadedSeqs = fastaParser.Parse().ToList();
                    isequence.AddRange(loadedSeqs);
                }
                else
                {
                    Bio.IO.GenBank.GenBankParser genBankParser = new Bio.IO.GenBank.GenBankParser(txtDatasetPath.Text);
                    loadedSeqs = genBankParser.Parse().ToList();
                    isequence.AddRange(loadedSeqs);
                }

                Boolean sameSize = true;

                //Finding Max seq length                
                SEQ_LENGTH = int.MaxValue;
                for (int i = 0; i < isequence.Count; i++)
                    if (isequence.ElementAt(i).Count() < SEQ_LENGTH)
                        SEQ_LENGTH = isequence.ElementAt(i).Count();
                //		
                
                showSeqsInDGV(radGridView1, loadedSeqs.ToList(), _importToCurrent);

                updateFreq();

                if (VarianceThreshold != Convert.ToDouble(Settings1.Default.Threshold_Size))
                    VarianceThreshold = Convert.ToDouble(Settings1.Default.Threshold_Size);

                if (sameSize == false && showNotification2)
                    MessageBox.Show("The dataset was loaded successfully, but it contains sequences of different sizes! " +
                        "In this program, minimum size (" + SEQ_LENGTH.ToString() +
                        ") is considered as sequence length.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (showNotification1 && radGridView1.Rows.Count > numberOfSequences)
                    MessageBox.Show("The dataset was loaded successfully, but it contains " + radGridView1.Rows.Count.ToString() +
                        " sequences! So it takes long time to run! I recommend you to choose another dataset if you haven't got enough time.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                toolStripButtonRun.Enabled = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("System.OutOfMemoryException was thrown! Please exit the application and re-open it again. If the problem persists, you may need to restart your Windows. \r\n"+ex.ToString(), "Fatal Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                //pnlLoad.Visible = false;
            }
        }

        private void updateFreq()
        {
            dnaSequenceUtil dsc = new dnaSequenceUtil();
            decimal AFreq = Math.Round(dsc.getFreq(isequence, 'A') * 100, 2);
            decimal CFreq = Math.Round(dsc.getFreq(isequence, 'C') * 100, 2);
            decimal GFreq = Math.Round(dsc.getFreq(isequence, 'G') * 100, 2);
            decimal TFreq = Math.Round(dsc.getFreq(isequence, 'T') * 100, 2);

            lblFreq.Text = "A: " + AFreq.ToString() + " % - C: " + CFreq.ToString() + " % - G: " + GFreq.ToString() + " % - T: " + TFreq.ToString() + " %";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (txtDatasetPath.Text == "")
            {
                MessageBox.Show("Please select currect FASTA or Gene Bank file.", "Failed to load", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            load();

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 1.0)
                timer1.Enabled = false;
            else
                this.Opacity += 0.5;
        }


        private void loadSettings()
        {
            try
            {
                Algorithm = Settings1.Default.Algorithm;
                if (Algorithm == "Needleman-Wunsch")
                    profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
                else
                    if (Algorithm == "Smith–Waterman")
                        profileAlignerName = ProfileAlignerNames.SmithWatermanProfileAligner;
                    else
                        MessageBox.Show("Please close the application and run it again.", "Fatal Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                open_gap = Convert.ToInt32(Settings1.Default.Open_Gap);
                extend_gap = Convert.ToInt32(Settings1.Default.Extend_Gap);
                if (Settings1.Default.IDLength != "")
                    IDLength = Convert.ToInt32(Settings1.Default.IDLength);
                else
                    IDLength = 20;
                CoVariance = Settings1.Default.CoVariance;
                Euclidean = Settings1.Default.Euclidean;
                Pearson = Settings1.Default.Pearson;
                showAdditionalInfo = Settings1.Default.ShowAdditionalInfo;
                preferredImageType = Settings1.Default.ImageType;
                if (CoVariance)
                    distanceFunctionName = DistanceFunctionTypes.CoVariance;
                else
                    if (Euclidean)
                        distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
                    else
                        distanceFunctionName = DistanceFunctionTypes.PearsonCorrelation;

                numberOfSequences = Convert.ToInt32(Settings1.Default.NumberOfSequences);
                showNotification1 = Settings1.Default.NotificationEnabled1;
                showNotification2 = Settings1.Default.NotificationEnabled2;
                pp = Convert.ToInt32(Settings1.Default.Presence_Percentage);
                VarianceThreshold = Convert.ToDouble(Settings1.Default.Threshold_Size);
                IgnoreGaps = Settings1.Default.Ignore_Gaps;
                preferredImageType = Settings1.Default.ImageType;


                if (Process.GetCurrentProcess().PriorityClass.ToString() != Settings1.Default.Priority)
                    switch (Settings1.Default.Priority)
                    {
                        case ("Normal"):
                            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                            break;
                        case ("AboveNormal"):
                            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
                            break;
                        case ("High"):
                            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                            break;
                        case ("RealTime"):
                            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                            break;
                        default:
                            break;
                    }

            }
            catch
            {
                if (++loadAttempts <= 3)
                    loadSettings();
                else
                {
                    MessageBox.Show("Failed to load settings! The application will be closed.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Dispose();
                }
            }
        }


        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();
            DialogResult dr = settingsForm.ShowDialog();
            if (dr == DialogResult.OK)
                loadSettings();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private string RemoveG(string str)
        {
            int a = 0; string strtemp;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == 'g')
                {
                    int j = i + 1;
                    while (str[j] >= '0' && str[j] <= '9')
                        j++;
                    strtemp = str.Substring(i + 1, j - i - 1);
                    strtemp = addZero(strtemp);

                    str = str.Substring(0, i) + strtemp + str.Substring(j);
                }//end of if
            }//end of for


            return str;
        }

        private void process()
        {
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            //toolStripButtonOpen.Enabled = false;
            cancelThreads = false;
            lblTotal.Text = isequence.Count().ToString(); ;
            lblCount.Text = "0";

            Thread mainThread = new Thread(new ThreadStart(mainThreadStart));
            mainThread.Priority = ThreadPriority.Highest;
            mainThread.Start();
        }//end of process

        private void btnRead_Click_1(object sender, EventArgs e)
        {
            process();
        }

        private void mainThreadStart()
        {
            Thread iThread = new Thread(new ThreadStart(ImprovedStepwiseAddition));
            // Start the thread

            iThread.Priority = ThreadPriority.Highest;
            iThread.Start();
            iThread.Join();

            if (cancelThreads)
            {
                pnlWaiting.Visible = false;
                Thread.CurrentThread.Abort();
            }

            //toolStripButtonOpen.Enabled = true;
            toolStripButtonRun.Visible = true;
            cancelThreads = toolStripButtonStop.Visible = false;

        }//end of mainthreadStart


        private Bitmap CaptureControl(Control ctl)
        {
            Rectangle rect;

            if (ctl is Form)
                rect = new Rectangle(ctl.Location, ctl.Size);
            else
                rect = new Rectangle(ctl.PointToScreen(new Point(0, 0)), ctl.Size);

            Bitmap bitmap = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

        private void panel3_Paint(object sender, PaintEventArgs e, string tree)
        {
            Point initialPosition = new Point(15, 14);
            int count = 0;
            ArrayList pointArray = new ArrayList();

            pointArray.Add(initialPosition);
            for (int i = 0; i < tree.Length; i++)
                if (tree[i] == ',')
                {
                    initialPosition.Y = 28 * pointArray.Count + 14;
                    pointArray.Add(initialPosition);
                }

            System.Drawing.Pen myPen;
            myPen = new System.Drawing.Pen(System.Drawing.Color.Black);
            System.Drawing.Graphics formGraphics = txtClusters.CreateGraphics();

            count = 0;
            for (int i = 0; i < tree.Length; i++)
            {
                ArrayList selectedCommas = new ArrayList();
                int number = 0;
                for (int j = 0; j < tree.Length; j++)
                    if (tree[j] == ',')
                    {
                        number++;
                        if (tree[j - 1] != ')' && tree[j + 1] != '(')
                            selectedCommas.Add(number);
                    }


                foreach (object obj in selectedCommas)
                {
                    count = (int)obj;
                    Point pt1 = (Point)pointArray[count - 1];
                    Point pt2 = (Point)pointArray[count];
                    Point pt1_2 = new Point(pt1.X, pt1.Y);
                    pt1_2.X = pt2.X + 2 * Math.Abs(pt2.Y - pt1.Y);
                    formGraphics.DrawLine(myPen, pt1, pt1_2);
                    formGraphics.DrawLine(myPen, pt1_2, pt2);
                    pointArray[count - 1] = pt1_2;
                }//end of foreach
                for (int k = selectedCommas.Count - 1; k >= 0; k--)
                {
                    count = (int)selectedCommas[k];
                    for (int j = 0; j < tree.Length; j++)
                    {
                        if (tree[j] == ',')
                            count--;
                        if (count == 0)
                        {
                            int lastIndex, firstIndex;
                            firstIndex = j;
                            while (tree[j] != ')')
                                j++;
                            lastIndex = j;
                            while (tree[firstIndex] != '(')
                                firstIndex--;
                            tree = tree.Substring(0, firstIndex) + "0" + tree.Substring(lastIndex + 1);

                            break;
                        }//end of if
                    }//end of for

                    count = i = 0;
                }//end of foreach

                for (int j = selectedCommas.Count - 1; j >= 0; j--)
                {
                    int n = (int)selectedCommas[j];
                    pointArray.RemoveAt(n);
                }//end of for

                selectedCommas = null;

            }//end of for i


            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void saveImage()
        {
            Bitmap bm = CaptureControl(txtClusters);
            try
            {
                bm.Save(@"e:\test.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//end of saveImage

        private void open(bool _importToCurrent = false)
        {
            string fPath = System.IO.Directory.GetCurrentDirectory();
            openFileDialog2.InitialDirectory = fPath + "\\datasets";
            openFileDialog2.RestoreDirectory = false;
            DialogResult dr = openFileDialog2.ShowDialog();
            if (dr == DialogResult.Cancel)
                return;
            if (dr == DialogResult.OK)
                txtDatasetPath.Text = openFileDialog2.FileName;

            if (txtDatasetPath.Text == "")
            {
                MessageBox.Show("Please select currect .fasta file.", "Failed to load", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
          

            Point pt = this.Location;
            pt.X = this.Location.X + this.Size.Width / 2 - 94;
            pt.Y = this.Location.Y + this.Size.Height / 2 - 48;
            MouseClickMessageFilter mousOnOff = new MouseClickMessageFilter();
            mousOnOff.DisableMouseClicks();
            this.Cursor = Cursors.AppStarting;
            this.Enabled = false;
            using (new PleaseWait(pt, "Loading dataset..."))
            {
                load(_importToCurrent);
                sequenceArray.Clear();
                for (int i = 0; i < isequence.Count; i++)
                {
                    dnaSequenceUtil dc = new dnaSequenceUtil();
                    sequenceArray.Add(dc.getACGTDNA(isequence[i]));
                }
            }
            //if select all is checked then this new loaded seqs will also be selected automatically
            for (int i = 0; i < radGridView1.Rows.Count; i++)
                radGridView1.Rows[i].Cells["choosed"].Value = chxCheckAll.Checked;
            radGridView1.Refresh();

            tabControl1.TabPages[3].Tag = tabControl1.TabPages[4].Tag = "empty";
            this.TopMost = true; 
            this.Cursor = Cursors.Default;
            mousOnOff.EnableMouseClicks();
            mousOnOff = null;
            this.Enabled = true;
            this.TopMost = false;

        }//end of open
        

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            helpToolStripMenuItem1_Click_1(sender, e);
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            settingsToolStripMenuItem1_Click(sender, e);
        }

        private void toolStripButtonRun_Click(object sender, EventArgs e)
        {
            runMode();
            process();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            regMode();
            cancelThreads = true;
        }

        private void saveFile()
        {
            string[] lines = txtClusters.Text.Split('\n');
            int iPos = 10;

            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (preferredImageType == "JPEG")
                    sfd.Filter = "PDF (*.pdf)|*.pdf|JPEG (*.jpg)|*.jpg";
                else
                    sfd.Filter = "PDF (*.pdf)|*.pdf|Bitmap (*.bmp)|*.bmp";

                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (sfd.FileName[sfd.FileName.Length - 1] == 'g')//save jpeg
                    {
                        int maxWidth = 0;
                        for (int i = 0; i < lines.Length; i++)
                            if (lines[i].Length > maxWidth)
                                maxWidth = lines[i].Length;
                        Bitmap myBitmap = new Bitmap(maxWidth * 12 + 50, lines.Length * 12 + 50);
                        Graphics g = Graphics.FromImage(myBitmap);
                        foreach (string line in lines)
                        {
                            g.DrawString(line, new Font("Consolas", 10), Brushes.Black, new PointF(5, iPos));
                            iPos += 12;
                        }
                        myBitmap.Save(sfd.FileName);
                    }
                    else//save pdf
                    {
                        PdfDocument pdf = new PdfDocument();
                        PdfPage pdfPage = pdf.AddPage();
                        pdfPage.Height = txtClusters.Lines.Count() * 12 + 50;
                        XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                        XFont font = new XFont("Consolas", 10, XFontStyle.Regular);
                        foreach (string line in lines)
                        {
                            graph.DrawString(line, font, Brushes.Black, new PointF(5, iPos));
                            iPos += 12;
                        }
                        pdf.Save(sfd.FileName);
                        Process.Start(sfd.FileName);
                    }

                    frmSaved.Visible = true;
                    frmSaved.WindowState = FormWindowState.Normal;
                    timer2.Enabled = true;
                }

            }//end of try
            catch (Exception e)
            {
                MessageBox.Show("The file was not saved!\r\nError: " + e.Message, "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                timer2.Enabled = false;
                frmSaved.Visible = false;
            }

        }//end of saveFile

        static void DrawSomethingToBitmap(Image img, string text)
        {
            Graphics g = Graphics.FromImage(img);
            g.DrawString(text, SystemFonts.GetFontByName("Consolas"), Brushes.Gray,
                img.Width / 2, img.Height / 2);

        }

        private void helpToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }

        private void checkUpdateToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CheckUpdate updateForm = new CheckUpdate();
            updateForm.Show();
        }

        private void viewMyWebsiteToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("www.blueweb.ir");
        }

        private void sendMeEmailToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            { System.Diagnostics.Process.Start("mailto:abolfazl.ghavidel@gmail.com"); }
            catch
            {
                MessageBox.Show("No installed e-mail-application was found!", "Error", MessageBoxButtons.OK,
                 MessageBoxIcon.Error);
            }
        }

        private void aboutToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            frmAbout frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = (int)(frmSaved.Opacity * 100);
            if (frmSaved.Opacity < 0.05)
            {
                timer2.Enabled = false;
                frmSaved.Visible = false;
                frmSaved.Opacity = 1.00;
                tabControl1.Focus();
            }
            else
                frmSaved.Opacity -= 0.03;
        }

        bool WriteToFile(string filename, List<ISequence> sequences)
        {
            //ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFileName(filename);
            if (formatter == null) return false;

            Sequence sequence = new Sequence(Alphabets.DNA, "ACGT");
            formatter.Open(filename);
            for (int i = 0; i < sequences.Count(); i++)
            {
                formatter.Write(sequences[i]);
            }

            formatter.Close();

            return true;
        }

        private void radGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "A":
                        e.CellStyle.BackColor = Color.FromArgb(110, 255, 117);
                        break;
                    case "C":
                        e.CellStyle.BackColor = Color.FromArgb(84, 113, 221);
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "G":
                        e.CellStyle.BackColor = Color.Black;
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "T":
                        e.CellStyle.BackColor = Color.FromArgb(255, 106, 106);
                        break;
                }
            }
        }

        private void radGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string dna = "";
            for (int i = 3; i < radGridView1.Rows[e.RowIndex].Cells.Count; i++)
            {
                if (radGridView1.Rows[e.RowIndex].Cells[i].Value != null)
                    dna += radGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
            }
            Sequence sequence = new Sequence(Alphabets.DNA, dna);
            sequence.ID = radGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            List<ISequence> isq = isequence.ToList();
            isq[e.RowIndex] = sequence;
            isequence = isq;
        }

        private void btnAlign_Click(object sender, EventArgs e)
        {
            List<ISequence> selectedSequences = new List<ISequence>();

            radGridView1.EndEdit();

            for (int j = 0; j < radGridView1.Rows.Count; j++)
            {
                if (Convert.ToBoolean(radGridView1.Rows[j].Cells["choosed"].Value))
                {
                    string dna = "";
                    for (int i = 3; i < radGridView1.Rows[j].Cells.Count; i++)
                    {
                        if (radGridView1.Rows[j].Cells[i].Value != null)
                            dna += radGridView1.Rows[j].Cells[i].Value.ToString();
                    }
                    Sequence sequence = new Sequence(Alphabets.AmbiguousDNA, dna);
                    sequence.ID = radGridView1.Rows[j].Cells[1].Value.ToString();

                    selectedSequences.Add(sequence);
                }
            }

            if (selectedSequences.Count == 0)
            {
                MessageBox.Show("Please select some rows before starting alignment process", "Error!");
                return;
            }

            Point pt = this.Location;
            pt.X = this.Location.X + this.Size.Width / 2 - 94;
            pt.Y = this.Location.Y + this.Size.Height / 2 - 48;
            using (new PleaseWait(pt, "MSA is being done..."))
            {
                

                SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                int kmerLength = 3;

                UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
                //DistanceFunctionTypes distanceFunctionName = new DistanceFunctionTypes();

                //ProfileAlignerNames profileAlignerName = new ProfileAlignerNames();
                ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;
                // Call aligner
                PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                    (selectedSequences, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                    profileAlignerName, profileProfileFunctionName, similarityMatrix, open_gap, extend_gap,
                    Environment.ProcessorCount * 2, Environment.ProcessorCount);


                alignedSeqs = (List<ISequence>)msa.AlignedSequences;
                //getConsensus(alignedSeqs, msa);
                SimpleConsensusResolver sc = new SimpleConsensusResolver(Alphabets.DNA, pp);
                byte[] consensusBArray = new byte[alignedSeqs[0].Count];
                for (int i = 0; i < alignedSeqs[0].Count; i++)
                {
                    byte[] bArray = new byte[alignedSeqs.Count];
                    for (int j = 0; j < alignedSeqs.Count; j++)
                    {
                        bArray[j] = alignedSeqs[j][i];
                    }

                    //consensusBArray[i] = msa.ConsensusResolver.GetConsensus(bArray);
                    consensusBArray[i] = sc.GetConsensus(bArray);
                }

                byte[] bb = new byte[alignedSeqs[0].Count];
                for (int i = 0; i < bb.Count(); i++)
                {
                    bb[i] = (byte)'-';
                }
                alignedSeqs.Add(makeSequence(bb, " "));
                alignedSeqs.Add(makeSequence(consensusBArray, "Consensus: "));


                showSeqsInDGV(dgvAlignment, alignedSeqs);
            }//end of using...
            //p.Kill();

            tabControl1.SelectedTab = alignedTabPage;
        }

        private Sequence makeSequence(byte[] consensusBArray, string id)
        {
            string dnaConsensus = "";
            for (int i = 0; i < consensusBArray.Count(); i++)
            {
                dnaConsensus += (char)consensusBArray[i];
            }
            Sequence seq = new Sequence(Alphabets.AmbiguousDNA, dnaConsensus);
            seq.ID = id;

            return seq;
        }

        private void showSeqsInDGV(DataGridView dgv, List<ISequence> iSeqs, bool _importToCurrent = false)
        {
            if (!_importToCurrent)
            {
                dgv.Columns.Clear();
                dgv.Rows.Clear();

                dnaSequenceUtil dsc = new dnaSequenceUtil();
                numberOfDNAColumns = dsc.findMaxSequenceLength(iSeqs);
                

                DataGridViewColumn[] columns = new DataGridViewColumn[numberOfDNAColumns + 3];

                DataGridViewCheckBoxColumn dgvc = new DataGridViewCheckBoxColumn();
                dgvc.HeaderText = "select";
                dgvc.Width = 40;
                dgvc.Name = "choosed";
                columns[0] = dgvc;
                DataGridViewTextBoxColumn dgvtbc1 = new DataGridViewTextBoxColumn();
                dgvtbc1.Name = "rowColumn";
                dgvtbc1.HeaderText = "#";
                dgvtbc1.Width = 50;
                columns[1] = dgvtbc1;
                DataGridViewTextBoxColumn dgvtbc2 = new DataGridViewTextBoxColumn();
                dgvtbc2.Name = "idColumn";
                dgvtbc2.HeaderText = "ID";
                dgvtbc2.Width = 120;
                columns[2] = dgvtbc2;

                for (int i = 0; i < numberOfDNAColumns; i++)
                {
                    using (DataGridViewTextBoxColumn forDGVC = new DataGridViewTextBoxColumn())
                    {
                        forDGVC.Name = "column" + (i + 1);
                        forDGVC.HeaderText = (i + 1).ToString();
                        forDGVC.DefaultCellStyle.Font = new Font("Tahoma", 8.5F, FontStyle.Bold);
                        forDGVC.Width = 35;
                        forDGVC.FillWeight = 1;
                        columns[i + 3] = forDGVC;
                    }
                }

                dgv.Columns.AddRange(columns);
            }
            else
            {
                dnaSequenceUtil dsc = new dnaSequenceUtil();
                int importedNumberOfColumns = dsc.findMaxSequenceLength(iSeqs);
                if (importedNumberOfColumns > numberOfDNAColumns)
                {
                    DataGridViewColumn[] columns = new DataGridViewColumn[importedNumberOfColumns - numberOfDNAColumns];
                    for (int i = 0; i < importedNumberOfColumns - numberOfDNAColumns; i++)
                    {
                        using (DataGridViewTextBoxColumn forDGVC = new DataGridViewTextBoxColumn())
                        {
                            forDGVC.Name = "column" + (numberOfDNAColumns + i + 1);
                            forDGVC.HeaderText = (numberOfDNAColumns + i + 1).ToString();
                            forDGVC.DefaultCellStyle.Font = new Font("Tahoma", 8.5F, FontStyle.Bold);
                            forDGVC.Width = 35;
                            forDGVC.FillWeight = 1;
                            columns[i] = forDGVC;
                        }
                    }

                    dgv.Columns.AddRange(columns);

                    numberOfDNAColumns = importedNumberOfColumns;
                }

            }//end of else

            int augmentedNumber = dgv.Rows.Count;
            dgv.Rows.Add(iSeqs.Count);
            for (int i = 0; i < iSeqs.Count(); i++)
            {
                try
                {
                    byte[] bArray = iSeqs[i].ToArray();
                    for (int j = 0; j < bArray.Length; j++)
                        if ((char)bArray[j] == '?' || (char)bArray[j] == 'n')
                            dgv.Rows[i + augmentedNumber].Cells[j + 3].Value = 'N';
                        else
                        {
                            dgv.Rows[i + augmentedNumber].Cells[j + 3].Value = (char)bArray[j];
                        }

                    dgv.Rows[i + augmentedNumber].Cells[2].Value = iSeqs[i].ID;
                    dgv.Rows[i + augmentedNumber].Cells[1].Value = (i + augmentedNumber + 1).ToString();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void dgvAlignment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                switch (e.Value.ToString())
                {
                    case "A":
                        e.CellStyle.BackColor = Color.FromArgb(110, 255, 117);
                        break;
                    case "C":
                        e.CellStyle.BackColor = Color.FromArgb(84, 113, 221);
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "G":
                        e.CellStyle.BackColor = Color.Black;
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "T":
                        e.CellStyle.BackColor = Color.FromArgb(255, 106, 106);
                        break;
                }
            }
        }

        private byte[] getConsensus(List<Sequence> seqs, PAMSAMMultipleSequenceAligner msa)
        {
            byte[] consensusBArray = new byte[seqs[0].Count];
            for (int i = 0; i < seqs[0].Count; i++)
            {
                byte[] bArray = new byte[seqs.Count];
                for (int j = 0; j < seqs.Count; j++)
                {
                    bArray[j] = seqs[j][i];
                }
                consensusBArray[i] = msa.ConsensusResolver.GetConsensus(bArray);
            }
            return consensusBArray;
        }

        private void chxCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < radGridView1.Rows.Count; i++)
            {
                radGridView1.Rows[i].Cells["choosed"].Value = chxCheckAll.Checked;
            }
        }

        private void saveTreeAsPdfFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Fasta Format|*.fasta";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string saveFilePath = sfd.FileName;
                WriteToFile(saveFilePath, isequence);
            }
        }

        private void openDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
            
        }

        private void importDatasetToCurrentOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open(true);
            
        }

        private void btnDeleteSelectedSeqs_Click(object sender, EventArgs e)
        {
            radGridView1.EndEdit();
            for (int j = radGridView1.Rows.Count-1; j >=0 ; j--)
            {
                if (Convert.ToBoolean(radGridView1.Rows[j].Cells["choosed"].Value))
                {
                    radGridView1.Rows.RemoveAt(j);
                    isequence.RemoveAt(j);
                }
            }
        }

        private void btnAbbreviate_Click(object sender, EventArgs e)
        {
            if (isequence.Count > 0)
            {
                dnaSequenceUtil dsc = new dnaSequenceUtil();
                for (int i = 0; i < isequence.Count; i++)
                {
                    //isequence[i].ID = dsc.getAbbrID(isequence[i]);
                    if (btnAbbreviate.Tag.ToString() == "abb")
                        radGridView1.Rows[i].Cells["idColumn"].Value = dsc.getAbbrID(isequence[i]);
                    else
                        radGridView1.Rows[i].Cells["idColumn"].Value = isequence[i].ID;
                }
                btnAbbreviate.Text = btnAbbreviate.Tag.ToString() != "abb" ? "Abbreviate IDs" : "Return";
                btnAbbreviate.Tag = btnAbbreviate.Tag.ToString() != "abb" ? "abb" : "ret";
            }
        }

        private void btnGetConstants_Click(object sender, EventArgs e)
        {
            if (isequence.Count > 0)
            {
                dnaSequenceUtil dsc = new dnaSequenceUtil();
                List<int> constans = new List<int>();
                List<int> variables = new List<int>();

                dsc.getConstantsAndVariables(isequence, ref constans, ref variables);

                radGridView1.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.radGridView1_CellFormatting);

                for (int i = 0; i < dsc.findMaxSequenceLength(isequence); i++)
                {
                    if (constans.Contains(i))
                    {
                        radGridView1.Columns[i + 3].DefaultCellStyle.BackColor = Color.FromArgb(178, 236, 255);
                    }
                    else
                        radGridView1.Columns[i + 3].DefaultCellStyle.BackColor = Color.FromArgb(249, 255, 178);
                }

                radGridView1.Refresh();

                lblCAndVFreq.Text = "C Freq: " + Math.Round(((double)constans.Count / (double)(constans.Count + variables.Count))*100, 2) + " % - V Freq: " + Math.Round(((double)variables.Count / (double)(constans.Count + variables.Count))*100, 2) + " %";
            }
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            if (isequence.Count > 0)
            {
                radGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.radGridView1_CellFormatting);
                radGridView1.Refresh();
            }
        }

        System.Windows.Forms.DataVisualization.Charting.SeriesChartType chartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        pairwisedDNAAnalyse pda;
        List<pairwisedDNAGraph> dataForGraph_DaroonGoone = new List<pairwisedDNAGraph>();
        List<pairwisedDNAGraph> dataForGraph_beinGoone = new List<pairwisedDNAGraph>();

        private void btnOpenCollection_ButtonClick(object sender, EventArgs e)
        {
            openDatasetToolStripMenuItem_Click(null, null);
        }

        private void btnSaveCollection_ButtonClick(object sender, EventArgs e)
        {
            saveDatasetToolStripMenuItem_Click(null, null);
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open(true);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = tabControl1.SelectedIndex;
            if (tabControl1.SelectedIndex >= 3 && tabControl1.SelectedTab.Tag.ToString() == "empty")
            { 
                btnCreateDistanceMatrix_Click(tabControl1.SelectedIndex, e);
                tabControl1.SelectedTab = tabControl1.TabPages[a];
            }
        }

        List<pairwisedDNAGraph> dataForGraph_beinJens = new List<pairwisedDNAGraph>();

        private void btnCreateDistanceMatrix_Click(object sender, EventArgs e)
        {
            if (isequence.Count() == 0)
            {
                MessageBox.Show("Please load a dataset through File -> Open or by pressing Ctrl + O","Attention",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (tabControl1.TabPages[4].Tag.ToString() != "empty")
            {
                tabControl1.SelectedTab = tabPage5;
                return;
            }

            Point pt = this.Location;
            pt.X = this.Location.X + this.Size.Width / 2 - 94;
            pt.Y = this.Location.Y + this.Size.Height / 2 - 48;
            using (new PleaseWait(pt))
            {

                pda = new pairwisedDNAAnalyse(isequence);

                dgvDistanceMatrix.Rows.Clear();
                dgvDistanceMatrix.Columns.Clear();

                if (isequence.Count < 600)
                {
                    for (int i = 0; i < pda.pairwisedDnaMatrix.Count; i++)
                    {
                        dgvDistanceMatrix.Columns.Add("column" + i.ToString() + "distance", i.ToString());
                        dgvDistanceMatrix.Rows.Add();
                    }
                    for (int i = 0; i < pda.pairwisedDnaMatrix.Count; i++)
                    {
                        for (int j = 0; j < pda.pairwisedDnaMatrix.Count; j++)
                        {
                            dgvDistanceMatrix.Rows[i].Cells[j].Value = pda.pairwisedDnaMatrix[i][j].k2pDistance.ToString();
                            if (pda.pairwisedDnaMatrix[i][j].gType == gooneJensType.daroon_goone)
                            {
                                dgvDistanceMatrix.Rows[i].Cells[j].Style.BackColor = Color.FromArgb(180, 250, 150);
                            }
                            else if (pda.pairwisedDnaMatrix[i][j].gType == gooneJensType.bein_goone)
                            {
                                dgvDistanceMatrix.Rows[i].Cells[j].Style.BackColor = Color.FromArgb(250, 150, 150);
                            }
                            else if (pda.pairwisedDnaMatrix[i][j].gType == gooneJensType.bein_jens)
                            {
                                dgvDistanceMatrix.Rows[i].Cells[j].Style.BackColor = Color.RoyalBlue;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error", "Number of Sequence Exceed 600, in this case due to data grid view limitation, can not show them in grid view");
                }

                txtMinValue.Text = Math.Round(pda.getMinValueInMatrix(pda.pairwisedDnaMatrix), 4).ToString();
                txtMaxValue.Text = Math.Round(pda.getMaxValueInMatrix(pda.pairwisedDnaMatrix), 4).ToString();
                txtVariance.Text = Math.Round(pda.getVariance(pda.pairwisedDnaMatrix), 4).ToString();
                txtSD.Text = Math.Round(pda.getStandardDeviation(pda.pairwisedDnaMatrix), 4).ToString();

                //draw the graph
                System.Windows.Forms.DataVisualization.Charting.Series sr_daroonGoone = new System.Windows.Forms.DataVisualization.Charting.Series("Intra Species");
                System.Windows.Forms.DataVisualization.Charting.Series sr_beinGoone = new System.Windows.Forms.DataVisualization.Charting.Series("Inter Species");
                System.Windows.Forms.DataVisualization.Charting.Series sr_beinJens = new System.Windows.Forms.DataVisualization.Charting.Series("Inter Genera");
                sr_daroonGoone.ChartType = chartType;
                sr_daroonGoone.Color = Color.Green;
                sr_beinGoone.ChartType = chartType;
                sr_beinGoone.Color = Color.Red;
                sr_beinJens.ChartType = chartType;
                sr_beinJens.Color = Color.Blue;

                int intervals = Convert.ToInt32(txtInterval.Text);
                dataForGraph_DaroonGoone = pda.createDNAGraph(pda.pairwisedDnaMatrix, gooneJensType.daroon_goone, intervals);
                dataForGraph_beinGoone = pda.createDNAGraph(pda.pairwisedDnaMatrix, gooneJensType.bein_goone, intervals);
                dataForGraph_beinJens = pda.createDNAGraph(pda.pairwisedDnaMatrix, gooneJensType.bein_jens, intervals);

                chart1.Series.Clear();
                chart1.Series.Add(sr_daroonGoone);
                chart1.Series.Add(sr_beinGoone);
                chart1.Series.Add(sr_beinJens);
                chart1.Legends[0].Docking = Docking.Top;

                chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;

                for (int i = 0; i < dataForGraph_DaroonGoone.Count; i++)
                {
                    sr_daroonGoone.Points.AddXY(dataForGraph_DaroonGoone[i].X_VALUE, dataForGraph_DaroonGoone[i].Y_VALUE);
                }
                for (int i = 0; i < dataForGraph_beinGoone.Count; i++)
                {
                    sr_beinGoone.Points.AddXY(dataForGraph_beinGoone[i].X_VALUE, dataForGraph_beinGoone[i].Y_VALUE);
                }
                for (int i = 0; i < dataForGraph_beinJens.Count; i++)
                {
                    sr_beinJens.Points.AddXY(dataForGraph_beinJens[i].X_VALUE, dataForGraph_beinJens[i].Y_VALUE);
                }

                chart1.Invalidate();
                //end of draw the graph
                tabControl1.TabPages[3].Tag = tabControl1.TabPages[4].Tag = "full";
                if(sender.ToString() != "3")
                    tabControl1.SelectedTab = tabPage5;
            }
        }//end of method

        private void cmxGraphType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isequence.Count > 0)
            {
                switch (cmxGraphType.SelectedIndex)
                {
                    case 0:
                        chartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        break;
                    case 1:
                        chartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                        break;
                    case 2:
                        chartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                        break;
                }

                btnCreateDistanceMatrix_Click(null, null);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (alignedSeqs.Count > 0)
            {
                dnaSequenceUtil dsc = new dnaSequenceUtil();
                List<int> constans = new List<int>();
                List<int> variables = new List<int>();

                alignedSeqs.RemoveAt(alignedSeqs.Count - 1);
                alignedSeqs.RemoveAt(alignedSeqs.Count - 1);
                dsc.getConstantsAndVariables(alignedSeqs, ref constans, ref variables);

                dgvAlignment.CellFormatting -= new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAlignment_CellFormatting);

                for (int i = 0; i < dsc.findMaxSequenceLength(alignedSeqs); i++)
                {
                    if (constans.Contains(i))
                    {
                        dgvAlignment.Columns[i + 3].DefaultCellStyle.BackColor = Color.FromArgb(178, 236, 255);
                    }
                    else
                        dgvAlignment.Columns[i + 3].DefaultCellStyle.BackColor = Color.FromArgb(249, 255, 178);
                }

                dgvAlignment.Refresh();

                lblFreqInAlign.Text = "C Freq: " + Math.Round(((double)constans.Count / (double)(constans.Count + variables.Count))*100, 2) + " % - V Freq: " + Math.Round(((double)variables.Count / (double)(constans.Count + variables.Count))*100, 2) + " %";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (alignedSeqs.Count > 0)
            {
                dgvAlignment.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAlignment_CellFormatting);
                dgvAlignment.Refresh();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            chart1.Printing.Print(true);
        }

        Point? prevPosition = null;
        double xValChart = 0, yValChart = 0;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value)
                return;
            toolTip1.RemoveAll();
            prevPosition = pos;

            var results = chart1.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
            foreach (var result in results)
            {
                if (result.ChartElementType == ChartElementType.PlottingArea)
                {
                    xValChart = Math.Round(result.ChartArea.AxisX.PixelPositionToValue(pos.X), 6);
                    yValChart = Math.Round(result.ChartArea.AxisY.PixelPositionToValue(pos.Y));

                    toolTip1.Show("Distance=" + xValChart + ", Frequency=" + yValChart, this.chart1, e.Location.X+15, e.Location.Y - 20);
                }
            }
             
        }

        List<pairwisedDNA> selectedSeqsInGraph = new List<pairwisedDNA>();
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            selectedSeqsInGraph.Clear();

            double value = Convert.ToDouble(xValChart);
            double leftValue = 0, rightValue = 0;
            int j = 0;
            while ((dataForGraph_beinGoone[j++].X_VALUE < value)) ;
            leftValue = dataForGraph_beinGoone[j - 2].X_VALUE;
            rightValue = dataForGraph_beinGoone[j-1].X_VALUE;
            double leftDist = value - leftValue;
            double rightDist = rightValue - value;

            if (leftDist < rightDist)
            {
                selectedSeqsInGraph.AddRange(pda.getSeqsBetween(dataForGraph_beinGoone[j - 2].XValue_left, dataForGraph_beinGoone[j - 2].XValue_right));
            }
            else
            {
                selectedSeqsInGraph.AddRange(pda.getSeqsBetween(dataForGraph_beinGoone[j-1].XValue_left, dataForGraph_beinGoone[j-1].XValue_right));
            }

            selectedSeqsInGraph.Sort(delegate(pairwisedDNA p1, pairwisedDNA p2)
            {
                return p1.K2P_DISTANCE.CompareTo(p2.k2pDistance);
            });

            int rowID=1;
            foreach (pairwisedDNA pDNA in selectedSeqsInGraph)
            {
                pDNA.rowID = rowID++;
            }

            pairwisedDNABindingSource.DataSource = selectedSeqsInGraph;
            pairwisedDNABindingSource.ResetBindings(false);

            for (int i = 0; i < dgvSelectedSeqsInGraph.Rows.Count; i++)
            {
                switch (selectedSeqsInGraph[i].G_TYPE)
                {
                    case gooneJensType.bein_goone:
                        dgvSelectedSeqsInGraph.Rows[i].DefaultCellStyle.BackColor = Color.IndianRed;
                        break;
                    case gooneJensType.bein_jens:
                        dgvSelectedSeqsInGraph.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                        break;
                    case gooneJensType.daroon_goone:
                        dgvSelectedSeqsInGraph.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        break;
                }
            }
        }

        private void checkSeqs(List<ISequence> seqs, bool _value)
        {
            for (int i = 0; i < seqs.Count; i++)
            {
                radGridView1.Rows[isequence.FindIndex(x => x.ID == seqs[i].ID)].Cells["choosed"].Value = _value;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            btnCreateDistanceMatrix_Click(null, null);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Alt && !e.Shift && !e.Control)
            {
                if (txtInterval.Focused)
                {
                    btnRefresh_Click(null, null);
                }
            }
        }

        private void btnAbbrIDsInGraph_Click(object sender, EventArgs e)
        {
            if (selectedSeqsInGraph.Count > 0)
            {
                dnaSequenceUtil dsc = new dnaSequenceUtil();
                for (int i = 0; i < selectedSeqsInGraph.Count; i++)
                {
                    //isequence[i].ID = dsc.getAbbrID(isequence[i]);
                    if (btnAbbrIDsInGraph.Tag.ToString() == "abb")
                    {
                        dgvSelectedSeqsInGraph.Rows[i].Cells["seq1"].Value = dsc.getAbbrID(selectedSeqsInGraph[i].seq1);
                        dgvSelectedSeqsInGraph.Rows[i].Cells["seq2"].Value = dsc.getAbbrID(selectedSeqsInGraph[i].seq2);
                    }
                    else
                    {
                        dgvSelectedSeqsInGraph.Rows[i].Cells["seq1"].Value = selectedSeqsInGraph[i].seq1.ID;
                        dgvSelectedSeqsInGraph.Rows[i].Cells["seq2"].Value = selectedSeqsInGraph[i].seq2.ID;
                    }
                }
                btnAbbrIDsInGraph.Text = btnAbbrIDsInGraph.Tag.ToString() != "abb" ? "Abbreviate IDs" : "Return";
                btnAbbrIDsInGraph.Tag = btnAbbrIDsInGraph.Tag.ToString() != "abb" ? "abb" : "ret";
            }
        }

        private void sendCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormComment frmComment = new FormComment();
            frmComment.ShowDialog();
        }//end of method
    }
}
