using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Bio;

namespace PhylogeneticTree
{
    class cluster
    {
        private int[] a, c, g, t;
        public string consensus;
        public int[] leaves;
        private int numberOfLeaves;
        public string tree;
        public double variance;

        public cluster()
        {
            a = new int[20000];
            c = new int[20000];
            g = new int[20000];
            t = new int[20000];
            consensus = "";
            leaves = new int[150];
            numberOfLeaves = 0;
            variance = 0;
        }

        public cluster(string subTree)
        {
            a = new int[20000];
            c = new int[20000];
            g = new int[20000];
            t = new int[20000];
            tree = subTree;
            consensus = "";
            leaves = new int[150];
            variance = 0;
            fillLeaves();
        }

        private void fillLeaves()
        {
            int start,finish, k;
            start = finish = k = 0;
            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i] >= '0' && tree[i] <= '9')
                {
                    start = i;
                    //while (subTree[i] >= '0' && subTree[i] <= '9' && i < subTree.Length)
                    //    i++;
                    //finish = i - 1;
                    //leaves[k++] = Convert.ToInt32(subTree.Substring(start, finish));
                    int strt = Convert.ToInt32(tree.Substring(start, 4));
                    i += 4;
                    leaves[k++] = strt; // like 0011
                }
            }
            numberOfLeaves = k;
            for (int i = k; i < leaves.Length; i++)
                leaves[k] = -1;
        }//end of fillLeaves


        public void ACGT(int seqLength, ref List<string> sequenceTable)
        {
            Array.Clear(a, 0, 19999);
            Array.Clear(c, 0, 19999);
            Array.Clear(g, 0, 19999);
            Array.Clear(t, 0, 19999);

            for(int i = 0 ; i < seqLength; i++)
            {
                for(int j = 0; j < numberOfLeaves; j++)
                    switch(sequenceTable[leaves[j]][i])
                        {
                        case 'A':
                            a[i]++;
                            break;
                        case 'C':
                            c[i]++;
                            break;
                        case 'G':
                            g[i]++;
                            break;
                        case 'T':
                            t[i]++;
                            break;
                        default:
                            break;
                    }
                int sum = a[i] + c[i] + g[i] + t[i];
                if (sum == 0) //Whene in an arbitrary column we only have gap or aNy
                    a[i] = c[i] = g[i] = t[i] = 0;
                else
                {
                    a[i] = (a[i] * 100) / sum;
                    c[i] = (c[i] * 100) / sum;
                    g[i] = (g[i] * 100) / sum;
                    t[i] = (t[i] * 100) / sum;
                }
            }

          
        }//end of ACGT


        private double dist(char ch1, char ch2)
        {
            if (ch1 == ch2)
            {
                if (ch1 == 'A' || ch1 == 'C' || ch1 == 'G' || ch1 == 'T')
                    return 0.0;
                if (ch1 == 'M' || ch1 == 'R' || ch1 == 'W' || ch1 == 'S' || ch1 == 'Y' || ch1 == 'K')
                    return 0.5;
                if (ch1 == 'N')
                    return 0.75;
                return 2.0 / 3.0;

            }

            if (ch1 > ch2)
            {
                char tmp = ch1;
                ch1 = ch2;
                ch2 = tmp;
            }
            if (ch1 == 'A')
                switch (ch2)
                {
                    case 'B':
                        return 1.0;
                    case 'C':
                        return 1.0;
                    case 'D':
                        return 2.0 / 3;
                    case 'G':
                        return 1.0;
                    case 'H':
                        return 2.0 / 3;
                    case 'K':
                        return 1.0;
                    case 'M':
                        return 0.5;
                    case 'R':
                        return 0.5;
                    case 'S':
                        return 1.0;
                    case 'T':
                        return 1.0;
                    case 'V':
                        return 2.0 / 3;
                    case 'W':
                        return 0.5;
                    case 'Y':
                        return 1.0;
                    default:
                        return 1.0;
                }
            if (ch1 == 'B')
                switch (ch2)
                {
                    case 'C':
                        return 2.0 / 3.0;
                    case 'D':
                        return 7.0 / 9.0;
                    case 'G':
                        return 2.0 / 3.0;
                    case 'H':
                        return 7.0 / 9.0;
                    case 'K':
                        return 2.0 / 3.0;
                    case 'M':
                        return 5.0 / 6.0;
                    case 'R':
                        return 5.0 / 6;
                    case 'S':
                        return 2.0 / 3;
                    case 'T':
                        return 2.0 / 3;
                    case 'V':
                        return 7.0 / 9;
                    case 'W':
                        return 5.0 / 6;
                    case 'Y':
                        return 2.0 / 3;
                    default:
                        return 1.0;
                }
            if (ch1 == 'C')
                switch (ch2)
                {
                    case 'D':
                        return 1.0;
                    case 'G':
                        return 1.0;
                    case 'H':
                        return 2.0 / 3;
                    case 'K':
                        return 1.0;
                    case 'M':
                        return 0.5;
                    case 'R':
                        return 0.5;
                    case 'S':
                        return 0.5;
                    case 'T':
                        return 1.0;
                    case 'V':
                        return 2.0 / 3;
                    case 'W':
                        return 1.0;
                    case 'Y':
                        return 0.5;
                    default:
                        return 1.0;
                }
            if (ch1 == 'D')
                switch (ch2)
                {
                    case 'G':
                        return 2.0 / 3;
                    case 'H':
                        return 7.0 / 9;
                    case 'K':
                        return 2.0 / 3;
                    case 'M':
                        return 5.0 / 6;
                    case 'R':
                        return 2.0 / 3;
                    case 'S':
                        return 5.0 / 6;
                    case 'T':
                        return 2.0 / 3;
                    case 'V':
                        return 7.0 / 9;
                    case 'W':
                        return 2.0 / 3;
                    case 'Y':
                        return 5.0 / 6;
                    default:
                        return 1.0;
                }

            if (ch1 == 'G')
                switch (ch2)
                {
                    case 'H':
                        return 1.0;
                    case 'K':
                        return 0.5;
                    case 'M':
                        return 1.0;
                    case 'R':
                        return 0.5;
                    case 'S':
                        return 0.5;
                    case 'T':
                        return 1.0;
                    case 'V':
                        return 2.0 / 3;
                    case 'W':
                        return 1.0;
                    case 'Y':
                        return 1.0;
                    default:
                        return 1.0;
                }
            if (ch1 == 'H')
                switch (ch2)
                {
                    case 'K':
                        return 5.0 / 6;
                    case 'M':
                        return 2.0 / 3;
                    case 'R':
                        return 2.0 / 3;
                    case 'S':
                        return 5.0 / 6;
                    case 'T':
                        return 2.0 / 3;
                    case 'V':
                        return 7.0 / 9;
                    case 'W':
                        return 2.0 / 3;
                    case 'Y':
                        return 2.0 / 3;
                    default:
                        return 1.0;
                }
            if (ch1 == 'K')
                switch (ch2)
                {
                    case 'M':
                        return 1.0;
                    case 'R':
                        return 0.75;
                    case 'S':
                        return 0.75;
                    case 'T':
                        return 0.5;
                    case 'V':
                        return 5.0 / 6;
                    case 'W':
                        return 0.75;
                    case 'Y':
                        return 0.75;
                    default:
                        return 1.0;
                }
            if (ch1 == 'M')
                switch (ch2)
                {
                    case 'R':
                        return 0.75;
                    case 'S':
                        return 0.75;
                    case 'T':
                        return 1.0;
                    case 'V':
                        return 2.0 / 3;
                    case 'W':
                        return 0.75;
                    case 'Y':
                        return 0.75;
                    default:
                        return 1.0;
                }
            if (ch1 == 'N')
                return 0.75;
            if (ch1 == 'R')
                switch (ch2)
                {
                    case 'S':
                        return 0.75;
                    case 'T':
                        return 1.0;
                    case 'V':
                        return 2.0 / 3;
                    case 'W':
                        return 0.75;
                    case 'Y':
                        return 1.0;
                    default:
                        return 1.0;
                }
            if (ch1 == 'S')
                switch (ch2)
                {
                    case 'T':
                        return 1.0;
                    case 'V':
                        return 2.0 / 3;
                    case 'W':
                        return 1.0;
                    case 'Y':
                        return 0.75;
                    default:
                        return 1.0;
                }
            if (ch1 == 'T')
                switch (ch2)
                {
                    case 'V':
                        return 1.0;
                    case 'W':
                        return 0.5;
                    case 'Y':
                        return 0.5;
                    default:
                        return 1.0;
                }
            if (ch1 == 'V')
                return 5.0 / 6;
            if (ch1 == 'W')
                return 0.75;

            return 1.0;
        }//end of dist

        public void calculateVariance(int seqLength, ref List<string> sequenceTable)
        {
            string str = "";
            double sigma = 0.0;
            double d = 0.0;
            for (int i = 0; i < numberOfLeaves; i++)
            {
                str = sequenceTable[leaves[i]];
                d = 0.0;
                while (consensus.Length < str.Length)
                    consensus += "N";
                    

                for (int j = 0; j < str.Length; j++)
                    d += dist(str[j], consensus[j]);
                d = d / seqLength;
                d = d * d;
                sigma += d;
            }//end of for i
            variance = sigma / numberOfLeaves;
        }//end of calculateVariance


        public int A(int index)
        {
            return a[index];
        }

        public int C(int index)
        {
            return c[index];
        }

        public int G(int index)
        {
            return g[index];
        }

        public int T(int index)
        {
            return t[index];
        }

        public int NumberOfLeaves
        {
            get { return numberOfLeaves; }
            set { numberOfLeaves = value; }
        }
    }
}
