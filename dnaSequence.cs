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
using System.Windows.Forms;

namespace PhylogeneticTree
{
    class dnaSequenceUtil
    {
        public int findMaxSequenceLength(List<ISequence> iseqs)
        {
            try
            {
                int maximumLength = iseqs.Max(x => x.ToArray().Length);
                return maximumLength;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int findMinSequenceLength(List<ISequence> iseqs)
        {
            try
            {
                int maximumLength = iseqs.Min(x => x.ToArray().Length);
                return maximumLength;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getAbbrID(ISequence iseq)
        {
            try
            {
                string id = iseq.ID;
                //fasta
                string[] idSplit = id.Split('|');
                string code = idSplit[3];
                string[] jensGooneSplit = idSplit[4].Remove(0, 1).Split(' ');
                string jens = jensGooneSplit[0];
                string goone = jensGooneSplit[1];
                //end of fasta

                return code + "_" + jens + "_" + goone;
            }
            catch (Exception)
            {
                return iseq.ID;
            }
        }

        public void getConstantsAndVariables(List<ISequence> iseqs, ref List<int> constants, ref List<int> variables)
        {
            try
            {
                constants = new List<int>();
                variables = new List<int>();
                int minSeqLength = findMinSequenceLength(iseqs);
                for (int i = 0; i < minSeqLength; i++)
                {
                    string s = "";
                    for (int j = 0; j < iseqs.Count; j++)
                    {
                        s += (char)iseqs[j][i];
                    }
                    string replacedS = s.Replace(s[0].ToString(), "");
                    if (replacedS.Length == 0)
                    {
                        constants.Add(i);
                    }
                    else
                        variables.Add(i);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal getFreq(List<ISequence> iseqs, char ch)
        {
            try
            {
                decimal freq = 0;
                decimal total = 0;
                for (int i = 0; i < iseqs.Count; i++)
                {
                    for (int j = 0; j < iseqs[i].Count; j++)
                    {
                        if ((char)iseqs[i][j] == ch)
                            freq++;
                        total++;
                    }
                }

                return freq / total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getACGTDNA(ISequence iseq)
        {
            string retS = "";
            byte[] bArray = iseq.ToArray();
            for (int j = 0; j < bArray.Length; j++)
                retS += (char)bArray[j];

            return retS;
        }
    }
}
