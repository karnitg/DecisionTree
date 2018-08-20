using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public static class BTreePrinter<T>
    {
        public static void PrintToFile(DecisionTreeNode<T> node, string filePath)
        {

            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                  //  printNode(node,file);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }   
        }


        class NodeInfo
        {
            public DecisionTreeNode<T> Node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public NodeInfo Parent, No, Yes;
        }

        public static void PrintTree(DecisionTree<T> tree,DecisionTreeNode<T> node, int topMargin = 2, int NoMargin = 2)
        {
            if (node == null)
            {
                return;
            }

            int topNode = Console.CursorTop + topMargin;
            var last = new List<NodeInfo>();
            var next = node;

            //BFS
            for (int level = 0; next != null; level++)
            {
                var item = new NodeInfo { Node = next, Text = String.Format("({0}) {1}",next.NodeCode,next.ConditionFunc.Method.Name ) };

                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + 4;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = NoMargin;
                    last.Add(item);
                }

                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.Node.No)
                    {
                        item.Parent.No = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
                    }
                    else
                    {
                        item.Parent.Yes = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
                    }
                }
                
                //if next.No != null than next => next.No, else next=>next.Yes
                next = next.No ?? next.Yes;

                for (; next == null; item = item.Parent)
                {
                    PrintNodeAndBranch(item, topNode + 4 * level);

                    if (--level < 0) break;
                    if (item == item.Parent.No)
                    {
                        item.Parent.StartPos = item.EndPos;
                        next = item.Parent.Node.Yes;
                    }
                    else
                    {
                        if (item.Parent.No == null)
                            item.Parent.EndPos = item.StartPos;
                        else
                            item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, topNode + 4 * last.Count - 1);
        }

        private static void PrintNodeAndBranch(NodeInfo item, int top)
        {
            SwapColors();
            //print node
            Print(item.Text, top, item.StartPos);

            SwapColors();
            if (item.No != null)
            {
                Print("|", top+1, item.StartPos);
                PrintBranch("N",top + 2, "┌", "┘", item.No.StartPos + item.No.Size / 2, item.StartPos);
                Print("|", top + 3, item.No.StartPos + item.No.Size / 2);

            }
            if (item.Yes != null)
            {
                Print("|", top+1, item.EndPos-1);
                PrintBranch("Y",top + 2, "└", "┐", item.EndPos - 1, item.Yes.StartPos + item.Yes.Size / 2);
                Print("|", top + 3, item.Yes.StartPos + item.Yes.Size / 2);

            }

        }

        private static void PrintBranch(string branchType,int top, string startStr, string endStr, int startPos, int endPos)
        {
            int spaceForPrint = (endPos - startPos- branchType.Length)/2 - 1;

            Print(startStr, top, startPos);
            Print("─", top, startPos + 1, startPos + 1 +spaceForPrint);
            Print(branchType, top, (startPos + 1 + spaceForPrint) + 1, (startPos + 1 + spaceForPrint) + 1 +branchType.Length);
            Print("─", top, (startPos + 1 + spaceForPrint) + 1 + branchType.Length + 1, endPos);
            Print(endStr, top, endPos);

        }

        private static void Print(string s, int top, int left, int right = -1)
        {
            if (Console.BufferWidth<left || Console.BufferWidth<right)
            {
                //print excedes the window borders
                return;
            }

            Console.SetCursorPosition(left, top);

            if (right < 0)
            {
                right = left + s.Length;
            }

            if (Console.BufferWidth < right)
            {
                //print excedes the window borders
                return;
            }
            while (Console.CursorLeft < right)
            {
                Console.Write(s);
            }
        }

        private static void SwapColors()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
        }
    }
}
