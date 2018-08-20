using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DecisionTreeNode<T>
    {
        public string NodeCode { get; set; }
        public Func<T, bool> ConditionFunc { get; set; }
        public DecisionTreeNode<T> Yes { get; set; }
        public DecisionTreeNode<T> No { get; set; }

        //IsTaken Boolean:
        //tree.AllowCircles = true  -> used for a circle check
        //tree.AllowCircles = false -> used for node mark
        public bool IsTaken { get; set; }

        public void Set(Func<T, bool> i_ConditionFunc, DecisionTreeNode<T> i_Yes, DecisionTreeNode<T> i_No, string i_NodeCode = null)
        {
            if (i_NodeCode!=null)
            {
                NodeCode = i_NodeCode;
            }
            ConditionFunc = i_ConditionFunc;
            Yes = i_Yes;
            No = i_No;

            CheckIsYesNoTaken();
            SetIsTaken();            
        }

        public void CheckIsYesNoTaken()
        {
            if (Yes!=null && Yes.IsTaken)
            {
                throw new Exception(String.Format("The node {0} is taken!", Yes.NodeCode));
            }
            if (No!=null && No.IsTaken)
            {
                throw new Exception(String.Format("The node {0} is taken!",No.NodeCode));
            }
        }

        public void Go(T searchData, bool IsFullPath, DecisionTreeResult decisionTreeResult)
        {
            bool res = ConditionFunc(searchData);
            if (IsFullPath)
            {
                decisionTreeResult.ResultCode += NodeCode + "_";
            }
            //update path
            decisionTreeResult.Path.Add(ConditionFunc.Method.Name,res);

            try
            {
                if (res)
                {
                    Yes.Go(searchData, IsFullPath, decisionTreeResult);
                }
                else
                {
                    No.Go(searchData, IsFullPath, decisionTreeResult);
                }
            }
            catch (Exception)
            {
                //next node is null - update code
                if (!IsFullPath)
                {
                    decisionTreeResult.ResultCode += NodeCode + "_";
                }
                decisionTreeResult.ResultCode += res ? "Y" : "N";
            }
           
        }
        public void SetIsTaken()
        {
            if (Yes != null)
            {
                Yes.IsTaken = true;
            }
            if (No != null)
            {
                No.IsTaken = true;
            }
        }
    }
}
