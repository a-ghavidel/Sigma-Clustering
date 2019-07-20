using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using Bio.SimilarityMatrices;
using Bio.IO.FastA;

namespace PhylogeneticTree
{
    class pairwisedDNAAnalyse
    {
        public List<List<pairwisedDNA>> pairwisedDnaMatrix = new List<List<pairwisedDNA>>();
        public List<pairwisedDNAGraph> graphValues = new List<pairwisedDNAGraph>();

        public pairwisedDNAAnalyse(List<ISequence> seqList)
        {
            try
            {
                for (int i = 0; i < seqList.Count; i++)
                {
                    pairwisedDnaMatrix.Add(new List<pairwisedDNA>());
                    for (int j = 0; j < seqList.Count; j++)
                    {
                        if (i < j)
                        {
                            pairwisedDnaMatrix[i].Add(new pairwisedDNA(seqList[i], seqList[j]));
                        }
                        else
                        {
                            pairwisedDnaMatrix[i].Add(new pairwisedDNA());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<pairwisedDNAGraph> createDNAGraph(List<List<pairwisedDNA>> pairwisedDnaMatrix, gooneJensType gt, double intervals = 10)
        {
            try
            {
                List<pairwisedDNAGraph> graphValues = new List<pairwisedDNAGraph>();

                double maxXValue = getMaxValueInMatrix(pairwisedDnaMatrix);
                double minValue = getMinValueInMatrix(pairwisedDnaMatrix);
                double difference = maxXValue - minValue;
                string xLabel = "";
                double yValue = 0;
                double xValue = 0, xValue_left=0, xValue_right=0;

                graphValues.Add(new pairwisedDNAGraph(0, "0", 0,0,0));
                if (intervals != -1)
                {
                    for (double i = minValue; i < intervals; i++)
                    {
                        if (i == 0)
                        {
                            xLabel = " <= " + Math.Round(minValue+(difference / intervals), 6);
                            xValue = Math.Round(minValue + (difference / intervals), 6);
                            xValue_right = Math.Round(minValue + (difference / intervals), 6);
                            yValue = getFrequency(pairwisedDnaMatrix, gt, (maxXValue / intervals), -1);
                        }
                        else if (i == intervals - 1)
                        {
                            xLabel = " > " + Math.Round(minValue + i * (difference / intervals), 6);
                            xValue = Math.Round(minValue + i * (difference / intervals), 6);
                            xValue_left = Math.Round(minValue + i * (difference / intervals), 6);
                            yValue = getFrequency(pairwisedDnaMatrix, gt, -1, i * (maxXValue / intervals));
                        }
                        else
                        {
                            xLabel = " ( " + Math.Round(minValue + i * (difference / intervals), 6) + " - " + Math.Round(minValue + (i + 1) * (difference / intervals), 6) + " ] ";
                            xValue = Math.Round((minValue + i * (difference / intervals) + minValue + (i + 1) * (difference / intervals)) / 2, 6);
                            xValue_left = Math.Round(minValue + i * (difference / intervals), 6);
                            xValue_right = Math.Round(minValue + (i + 1) * (difference / intervals), 6);
                            yValue = getFrequency(pairwisedDnaMatrix, gt, (minValue + i * (difference / intervals)), minValue + (i + 1) * (difference / intervals));
                        }

                        graphValues.Add(new pairwisedDNAGraph(xValue, xLabel, yValue, xValue_left, xValue_right));
                    }
                }
                else
                {
                }

                return graphValues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getMaxValueInMatrix(List<List<pairwisedDNA>> pairwisedDnaMatrix)
        {
            try
            {
                double maxValue = 0;

                for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
                {
                    for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                    {
                        if (pairwisedDnaMatrix[i][j].k2pDistance > maxValue)
                            maxValue = pairwisedDnaMatrix[i][j].k2pDistance;
                    }
                }

                return maxValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getMinValueInMatrix(List<List<pairwisedDNA>> pairwisedDnaMatrix)
        {
            try
            {
                double minValue = 0;

                for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
                {
                    for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                    {
                        if (pairwisedDnaMatrix[i][j].k2pDistance < minValue)
                            minValue = pairwisedDnaMatrix[i][j].k2pDistance;
                    }
                }

                return minValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getMean(List<List<pairwisedDNA>> pairwisedDnaMatrix)
        {
            try
            {
                double sum = 0, count=0;

                for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
                {
                    for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                    {
                        sum += pairwisedDnaMatrix[i][j].k2pDistance;
                        count++;
                    }
                }

                return sum/count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getVariance(List<List<pairwisedDNA>> pairwisedDnaMatrix)
        {
            try
            {
                double sum = 0, count = 0, mean = getMean(pairwisedDnaMatrix);

                for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
                {
                    for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                    {
                        sum += Math.Pow((pairwisedDnaMatrix[i][j].k2pDistance - mean), 2);
                        count++;
                    }
                }

                return sum / count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getStandardDeviation(List<List<pairwisedDNA>> pairwisedDnaMatrix)
        {
            try
            {
                return Math.Pow(getVariance(pairwisedDnaMatrix), 0.5);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double getCount(List<List<pairwisedDNA>> pairwisedDnaMatrix)
        {
            double count = 0;
            for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
            {
                for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                {
                    count++;
                }
            }

            return count;
        }

        public int getFrequency(List<List<pairwisedDNA>> pairwisedDnaMatrix, gooneJensType gt, double min = -1, double max = -1)
        {
            try
            {
                int freq = 0;
                for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
                {
                    for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                    {
                        if (min != -1 && max != -1)
                        {
                            if ((pairwisedDnaMatrix[i][j].k2pDistance > min) && (pairwisedDnaMatrix[i][j].k2pDistance <= max) && pairwisedDnaMatrix[i][j].gType == gt)
                            {
                                freq++;
                            }
                        }
                        else if (min != -1 && max == -1)
                        {
                            if ((pairwisedDnaMatrix[i][j].k2pDistance <= min) && pairwisedDnaMatrix[i][j].gType == gt)
                            {
                                freq++;
                            }
                        }
                        else if (min == -1 && max != -1)
                        {
                            if ((pairwisedDnaMatrix[i][j].k2pDistance > max) && pairwisedDnaMatrix[i][j].gType == gt)
                            {
                                freq++;
                            }
                        }
                    }
                }

                return freq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<pairwisedDNA> getSeqsBetween(double leftValue, double rightValue)
        {
            try
            {
                int rowID = 1;
                List<pairwisedDNA> retList = new List<pairwisedDNA>();
                for (int i = 0; i < pairwisedDnaMatrix.Count; i++)
                {
                    for (int j = 0; j < pairwisedDnaMatrix.Count; j++)
                    {
                        if(pairwisedDnaMatrix[i][j].k2pDistance<rightValue && pairwisedDnaMatrix[i][j].k2pDistance>leftValue)
                        {
                            pairwisedDnaMatrix[i][j].rowID = rowID++;
                            retList.Add(pairwisedDnaMatrix[i][j]);
                        }
                    }
                }

                return retList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class pairwisedDNAGraph
    {
        private double XValue = 0;
        private string XLabel = "";
        public double XValue_left = 0;
        public double XValue_right = 0;
        private double YValue = 0;

        public pairwisedDNAGraph(double _XValue, string _XLabel, double _YValue, double _xValue_left, double _xValue_right)
        {
            XValue = _XValue;
            XLabel = _XLabel;
            YValue = _YValue;
            XValue_left = _xValue_left;
            XValue_right = _xValue_right;
        }

        public double X_VALUE
        {
            get { return XValue; }
            set { XValue = value; }
        }

        public string X_LABEL
        {
            get { return XLabel; }
            set { XLabel = value; }
        }

        public double Y_VALUE
        {
            get { return YValue; }
            set { YValue = value; }
        }
    }

    public class pairwisedDNA
    {
        public ISequence seq1, seq2;
        public double k2pDistance = 0;
        public gooneJensType gType = gooneJensType.unKnown;
        public int rowID = 0;

        public pairwisedDNA()
        {

        }

        public pairwisedDNA(ISequence _seq1, ISequence _seq2)
        {
            seq1 = _seq1;
            seq2 = _seq2;

            k2pDistance = calcK2PDistance(seq1, seq2);
            gType = getGooneType(seq1, seq2);
        }

        public double calcK2PDistance(ISequence seq1, ISequence seq2)
        {
            try
            {
                double dist = 0;

                dnaSequenceUtil dsuc = new dnaSequenceUtil();
                double min = dsuc.findMinSequenceLength(new List<ISequence>() { seq1, seq2 });
                double transitionCount = 0, transversionCount = 0;
                for (int i = 0; i < min; i++)
                {
                    if (((char)seq1[i] == 'A' && (char)seq2[i] == 'G') || ((char)seq1[i] == 'C' && (char)seq2[i] == 'T') || ((char)seq1[i] == 'G' && (char)seq2[i] == 'A') || ((char)seq1[i] == 'T' && (char)seq2[i] == 'C'))
                        transitionCount++;
                    if (((char)seq1[i] == 'A' && (char)seq2[i] == 'C') || ((char)seq1[i] == 'G' && (char)seq2[i] == 'T') || ((char)seq1[i] == 'A' && (char)seq2[i] == 'T') || ((char)seq1[i] == 'G' && (char)seq2[i] == 'C') ||
                        ((char)seq1[i] == 'C' && (char)seq2[i] == 'A') || ((char)seq1[i] == 'T' && (char)seq2[i] == 'G') || ((char)seq1[i] == 'T' && (char)seq2[i] == 'A') || ((char)seq1[i] == 'C' && (char)seq2[i] == 'G'))
                        transversionCount++;
                }
                double P = (transitionCount / min);
                double Q = (transversionCount / min);
                dist = Convert.ToDouble((1F / 2F) * (Math.Log(Convert.ToDouble(1 / (1 - 2 * P - Q)))) + (1F / 4F) * (Math.Log(Convert.ToDouble(1 / (1 - 2 * Q)))));

                return dist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public gooneJensType getGooneType(ISequence seq1, ISequence seq2)
        {
            try
            {
                gooneJensType gt = gooneJensType.unKnown;

                dnaSequenceUtil dsc = new dnaSequenceUtil();
                string[] seq1ID = dsc.getAbbrID(seq1).Split('_');
                string[] seq2ID = dsc.getAbbrID(seq2).Split('_');

                if (seq1ID.Count() == 3 && seq2ID.Count() == 3)
                {
                    if (seq1ID[1] == seq2ID[1])
                    {
                        if (seq1ID[2] == seq2ID[2])
                            gt = gooneJensType.daroon_goone;
                        else
                            gt = gooneJensType.bein_goone;
                    }
                    else
                        gt = gooneJensType.bein_jens;
                }
                return gt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ISequence SEQ_1 { get { return seq1; } set { seq1 = value; } }
        public string SEQ_1_ID { get { return seq1.ID; }}
        public ISequence SEQ_2 { get { return seq2; } set { seq2 = value; } }
        public string SEQ_2_ID { get { return seq2.ID; }}
        public double K2P_DISTANCE { get { return k2pDistance; } set { k2pDistance = value; } }
        public gooneJensType G_TYPE { get { return gType; } set { gType = value; } }
        public int ROW_ID { get { return rowID; } set { rowID = value; } }
    }

    public enum gooneJensType
    {
        daroon_goone,
        bein_goone,
        bein_jens,
        unKnown
    }
}


