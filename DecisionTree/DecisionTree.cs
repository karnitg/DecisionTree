using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DecisionTree<T>
    {
        public DecisionTreeNode<T> Root { get; set; }
        public Dictionary<string,DecisionTreeNode<T>> nodes = null;

        public DecisionTree(int numOfNodes)
        {
            string key = "";
            DecisionTreeNode<T> newNode = null;
            nodes = new Dictionary<string, DecisionTreeNode<T>>();
            for (int i = 0; i < numOfNodes; i++)
            {
                key = String.Format("{0}", i + 1);
                newNode = new DecisionTreeNode<T>();
                newNode.NodeCode = key;
                nodes.Add(key,newNode);
            }
            SetRoot("1");
        }
        public void SetRoot(string i_NodeCode)
        {
            Root = GetNode(i_NodeCode);
        }
        public void SetRoot(DecisionTreeNode<T> i_Root)
        {
            Root = i_Root;
        }

        public void PrintAllPathsToOutputWindow(bool isFullPath)
        {
            System.Diagnostics.Debug.WriteLine("*********");

            Dictionary<string, string> allPaths = new Dictionary<string, string>();
            GetAllPaths(Root, allPaths, "", "", "", isFullPath);

            foreach (var item in allPaths)
            {
                System.Diagnostics.Debug.WriteLine(item.Key);
                System.Diagnostics.Debug.WriteLine(item.Value);
            }

            System.Diagnostics.Debug.WriteLine("*********");

        }

        public void PrintAllPathsToCommandWindow(bool isFullPath)
        {
            Console.WriteLine("*********");

            Dictionary<string, string> allPaths = new Dictionary<string, string>();
            GetAllPaths(Root, allPaths, "", "", "", isFullPath);

            foreach (var item in allPaths)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
            }

            Console.WriteLine("*********");

        }
        /// <summary>
        /// prints the witch case for the tree, including the path for each case
        /// </summary>
        /// <param name="isFullPath">
        /// true for full node path in case key
        /// false for just leaf in case key. in this case the path printed for each leaf, will be the first for the leaf
        /// </param>
        public void PrintSwitchCasesToOutputWindow(bool isFullPath)
        {
            System.Diagnostics.Debug.WriteLine("*********");

            Dictionary<string, string> allPaths = new Dictionary<string, string>();
            GetAllPaths(Root, allPaths, "", "", "", isFullPath);

            System.Diagnostics.Debug.WriteLine("switch ((resultCode))");
            System.Diagnostics.Debug.WriteLine("{");

            foreach (var item in allPaths)
            {
                System.Diagnostics.Debug.WriteLine("case \"" + item.Key + "\":");
                System.Diagnostics.Debug.WriteLine("//"+item.Value);
                System.Diagnostics.Debug.WriteLine("break;");
            }
            System.Diagnostics.Debug.WriteLine("default:");
            System.Diagnostics.Debug.WriteLine("break;");

            System.Diagnostics.Debug.WriteLine("}");

            System.Diagnostics.Debug.WriteLine("*********");

        }
        public void SetTreeNode(string i_NodeToSet, Func<T, bool> i_ConditionFunc, string i_Yes_NodeCode, string i_No_NodeCode)
        {
            DecisionTreeNode<T> node = GetNode(i_NodeToSet);

            //node.NodeCode = i_NodeToSet;
            node.ConditionFunc = i_ConditionFunc;
            node.Yes = GetNode(i_Yes_NodeCode);
            node.No = GetNode(i_No_NodeCode);
            node.CheckIsYesNoTaken();
            node.SetIsTaken();
          
        }
        public void CreateTreeNode(string i_NodeCode)
        {
            DecisionTreeNode<T> node = new DecisionTreeNode<T>();
            node.NodeCode = i_NodeCode;
            nodes.Add(i_NodeCode, node);
        }
        public void CreateTreeNode(string i_NodeCode, Func<T, bool> i_ConditionFunc, string i_Yes_NodeCode,string i_No_NodeCode)
        {
            DecisionTreeNode<T> node = new DecisionTreeNode<T>();

            node.NodeCode = i_NodeCode;
            node.ConditionFunc = i_ConditionFunc;
            node.Yes = GetNode(i_Yes_NodeCode);
            node.No = GetNode(i_No_NodeCode);
            node.CheckIsYesNoTaken();
            node.SetIsTaken();
           
            if (GetNode(i_Yes_NodeCode) == null && i_Yes_NodeCode != null)
            {
                throw new Exception(String.Format("Node {0} not found or not created yet.", i_Yes_NodeCode));
            }
            if (GetNode(i_No_NodeCode) == null && i_No_NodeCode != null)
            {
                throw new Exception(String.Format("Node {0} not found or not created yet.", i_No_NodeCode));
            }

            nodes.Add(i_NodeCode, node);
        }

    

        //public void CreateTreeNode(string i_NodeCode, Func<T, bool> i_ConditionFunc, DecisionTreeNode<T> i_Yes, DecisionTreeNode<T> i_No)
        //{
        //    DecisionTreeNode<T> node = new DecisionTreeNode<T>();

        //    node.NodeCode = i_NodeCode;
        //    node.ConditionFunc = i_ConditionFunc;
        //    node.Yes = i_Yes;
        //    node.No = i_No;

        //    nodes.Add(i_NodeCode, node);
        //}
        //public void SetTreeNode(string i_NodeToSet, Func<T, bool> i_ConditionFunc, DecisionTreeNode<T> i_Yes, DecisionTreeNode<T> i_No)
        //{
        //    DecisionTreeNode<T> node = GetNode(i_NodeToSet);

        //    node.ConditionFunc = i_ConditionFunc;
        //    node.Yes = i_Yes;
        //    node.No = i_No;
        //}


        public DecisionTreeNode<T> GetNode(string i_NodeCode)
        {
            DecisionTreeNode<T> res = null;
            if (i_NodeCode!=null)
            {
                nodes.TryGetValue(i_NodeCode, out res);
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchData"></param>
        /// <param name="IsPathCode">IMPORTANT! use in trees that the same condition function and same answer can have diffrent results in diffrent paths</param>
        /// <returns></returns>
        public DecisionTreeResult Go(T searchData, bool IsFullPath)
        {
            DecisionTreeResult res = new DecisionTreeResult();
            res.Path = new Dictionary<string, bool>();  

            Root.Go(searchData, IsFullPath, res);

            return res;
        }

        public void GetAllPaths(DecisionTreeNode<T> node, Dictionary<string, string> nodelist,string key,string funcPath, string yn, bool isFullPath)
        {
            try
            {
                //update function path with yes or no - accourding to with which we came with
                funcPath += yn==""?"":"(" +yn+")-";
                //create list if null
                if (nodelist == null)
                {
                    nodelist = new Dictionary<string, string>();
                }
                //key - the track key, value - the track function path
                if (node == null)
                { 
                    if (!nodelist.ContainsKey(key + yn))
                    {
                        nodelist.Add(key + yn, funcPath);
                    }
                }
                else
                {
                    if (isFullPath)
                    {
                        key += node.NodeCode + "_";
                    }
                    else
                    {
                        key = node.NodeCode + "_";
                    }

                    funcPath += node.ConditionFunc.Method.Name;
                    
                    GetAllPaths(node.Yes, nodelist, key, funcPath,"Y", isFullPath);
                    GetAllPaths(node.No, nodelist, key, funcPath, "N", isFullPath);
                }
            }
            catch (Exception)
            {
                //failed - check last path
                nodelist.Add("failed", "last key: "+key + " ,path: "+ funcPath);
            }
        }
    }
}
