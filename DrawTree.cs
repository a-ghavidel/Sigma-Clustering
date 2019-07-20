using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PhylogeneticTree
{
    class DrawTree
    {
        private int mode;

        /// <summary>
        /// Draw a tree by the aid of characters.
        /// </summary>
        /// <param name="tree">The tree in Newick format</param>
        /// <param name="m">The mode coud be 0 or 1</param>
        /// <returns>ArrayList: An array of all lines</returns>
        public ArrayList Draw(string tree, int m)
        {
            mode = m;
            //counting the number of commas ----> number of nodes = 2*number of commas - 1
            int numberOfCommas = 0;
            foreach (char ch in tree)
                if (ch == ',')
                    numberOfCommas++;
            int n = 2 * numberOfCommas - 1; //n = number of nodes
            ArrayList lines = new ArrayList(2 * n - 1);
            for (int i = 0; i < 2 * n - 1; i++)
                lines.Add(" ");
            tree = replaceNodesOfTreeByLineNumbers(tree);
            while (tree[0] == '(')
                tree = draw(tree, lines);
            return lines;
        }

        /// <summary>
        /// Draw a tree by the aid of characters.
        /// </summary>
        /// <param name="tree">The tree in Newick format</param>
        /// <returns>ArrayList: An array of all lines</returns>
        public ArrayList Draw(string tree)
        {
            mode = 0;
            //counting the number of commas ----> number of nodes = 2*number of commas - 1
            int numberOfCommas = 0;
            foreach (char ch in tree)
                if (ch == ',')
                    numberOfCommas++;
            int n = 2 * numberOfCommas + 1; //n = number of nodes
            ArrayList lines = new ArrayList(n);
            for (int i = 0; i < n; i++)
                lines.Add(" ");
            tree = replaceNodesOfTreeByLineNumbers(tree);
            while (tree[0] == '(')
                tree = draw(tree, lines);
            return lines;
        }

        private string addZero(int count)
        {
            string str = count.ToString();
            if (str.Length == 1)
                return "000" + str;
            if (str.Length == 2)
                return "00" + str;
            if (str.Length == 3)
                return "0" + str;
            return str;
        }

        private string replaceNodesOfTreeByLineNumbers(string tree)
        {
            int begin, end, count;
            begin = end = count = 0;
            for (int i = 0; i < tree.Length; i++)
            {
                if (tree[i] != '(' && tree[i] != ')' && tree[i] != ',')
                {
                    begin = i;
                    while (tree[i] != '(' && tree[i] != ')' && tree[i] != ',' && i < tree.Length -1)
                        i++;
                    end = i;
                    tree = tree.Substring(0, begin) + addZero(count) + tree.Substring(end);
                    count += 2;
                    while (tree[i] != '(' && tree[i] != ')' && tree[i] != ',' && i < tree.Length -1)
                        i++;
                }
            }//end of for i
            return tree;
        }//end of replaceNodesOfTreeByLineNumbers

        private string draw(string tree, ArrayList lines)
        {
            int begin, end;
            begin = end = 0;
            for (int i = 0; i < tree.Length; i++)
                if (tree[i] == '(')
                    begin = i;
                else
                    if (tree[i] == ')')
                    { end = i; break; }

            string str = leftStarRight(tree.Substring(begin, end - begin + 1));
            string leftChild = str.Substring(0, str.IndexOf('*'));
            string rightChild = str.Substring(str.IndexOf('*') + 1);
            //coonect left to right
            int a = Convert.ToInt32(leftChild);
            int b = Convert.ToInt32(rightChild);
            connect(a, b, lines);
            tree = tree.Replace(tree.Substring(begin, end - begin + 1), addZero((a + b) / 2));
            return tree;

        }//end of draw
        
        private string leftStarRight(string subTree)
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
        }

        private void connect(int a, int b, ArrayList lines)
        {
            string topLineString = lines[a].ToString();
            string downLineString = lines[b].ToString();

            if (topLineString.Length < downLineString.Length && mode == 0)
                while (topLineString.Length != downLineString.Length)
                    topLineString += "─";
            if (topLineString.Length < downLineString.Length && mode == 1)
                while (topLineString.Length != downLineString.Length)
                    topLineString += "-";
            if (topLineString.Length > downLineString.Length && mode == 0)
                while (topLineString.Length != downLineString.Length)
                    downLineString += "─";
            if (topLineString.Length > downLineString.Length && mode == 1)
                while (topLineString.Length != downLineString.Length)
                    downLineString += "-";
            if (mode == 0)
            {
                topLineString += "─┐";
                downLineString += "─┘";
            }
            else
            {
                topLineString += "-|";
                downLineString += "-|";
            }
            
            lines[a] = topLineString;
            lines[b] = downLineString;

            for (int i = a + 1; i < b; i++)
            {
                string str = lines[i].ToString();
                while (str.Length != topLineString.Length)
                    str += " ";
                if (mode == 0)
                {
                    if (i == (a + b) / 2)
                        lines[i] = str.Substring(0, str.Length - 1) + "├─";
                    else
                        lines[i] = str.Substring(0, str.Length - 1) + "│";
                }
                else
                {
                    if (i == (a + b) / 2)
                        lines[i] = str.Substring(0, str.Length - 1) + "|-";
                    else
                        lines[i] = str.Substring(0, str.Length - 1) + "|";
                }


            }//end of for



        }//end of connect

    }//end of class

      
}
