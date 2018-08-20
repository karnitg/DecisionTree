using DecisionTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //nodes only
            SingleNodesTest(false);
            //full path
            SingleNodesTest(true);

            //nodes only
            CodeNamesTest(false);
            //full path
            CodeNamesTest(true);

            Console.Read();
 
        }
        private static void SingleNodesTest(bool isFullPath)
        {
            try
            {
                Console.WriteLine(String.Format("SingleNodesTest (isFullPath = {0})", isFullPath));
                Console.WriteLine("---------------");

                DecisionTreeNode<TestSearchData> n1 = new DecisionTreeNode<TestSearchData>();
                DecisionTreeNode<TestSearchData> n2 = new DecisionTreeNode<TestSearchData>();
                DecisionTreeNode<TestSearchData> n3 = new DecisionTreeNode<TestSearchData>();
                DecisionTreeNode<TestSearchData> n4 = new DecisionTreeNode<TestSearchData>();
                DecisionTreeNode<TestSearchData> n5 = new DecisionTreeNode<TestSearchData>();
                DecisionTreeNode<TestSearchData> n6 = new DecisionTreeNode<TestSearchData>();
                //DecisionTreeNode<TestSearchData> n7 = new DecisionTreeNode<TestSearchData>();
                //DecisionTreeNode<TestSearchData> n8 = new DecisionTreeNode<TestSearchData>();

                n1.Set(IsEqual, null, n2, "1");
                n2.Set(IsABigger10, n3, n4, "2");
                n3.Set(IsABigger20, n6, null, "3");
                n4.Set(IsBBigger10, n5, null, "4");
                n5.Set(IsBBigger20, null, null, "5");
                n6.Set(IsBBigger20, null, null, "6");
                //n7.Set(IsBBigger20, n8, null, "7");
                //n8.Set(IsBBigger20, null, null, "8");

                DecisionTree<TestSearchData> tree = new DecisionTree<TestSearchData>(0);
                tree.SetRoot(n1);

                //print tree
                DecisionTree.BTreePrinter<TestSearchData>.PrintTree(tree,n1);

                //print to the command window tree nodes and the paths leading to them 
                tree.PrintAllPathsToCommandWindow(isFullPath);

                //print to the output window tree nodes and the paths leading to them
                tree.PrintAllPathsToOutputWindow(isFullPath);

                //print to the output window a ready switch case to use in the result maneging function
                tree.PrintSwitchCasesToOutputWindow(isFullPath);

                //tests

                //1_Y (10, 10)
                //3_N (15, 10)
                //5_N (5, 15)
                //6_N (25, 10)
                TestSearchData testSearchData1 = new TestSearchData(10, 10);
                TestSearchData testSearchData2 = new TestSearchData(15, 10);
                TestSearchData testSearchData3 = new TestSearchData(5, 15);
                TestSearchData testSearchData4 = new TestSearchData(25, 10);

                DecisionTreeResult res1 = tree.Go(testSearchData1, isFullPath);
                DecisionTreeResult res2 = tree.Go(testSearchData2, isFullPath);
                DecisionTreeResult res3 = tree.Go(testSearchData3, isFullPath);
                DecisionTreeResult res4 = tree.Go(testSearchData4, isFullPath);

                //print test results 
                printRes(testSearchData1, res1.ResultCode);
                printRes(testSearchData2, res2.ResultCode);
                printRes(testSearchData3, res3.ResultCode);
                printRes(testSearchData4, res4.ResultCode);

                Console.WriteLine("----------------------------");

                //an example for a result maneging function
                resultManage(res1.ResultCode, isFullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SingleNodesTest failed. Details in the output window.");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }

        private static void CodeNamesTest(bool isFullPath)
        {
            try
            {
                Console.WriteLine(String.Format("CodeNamesTest (isFullPath = {0})", isFullPath));
                Console.WriteLine("-------------");

                //create tree
                DecisionTree<TestSearchData> tree = new DecisionTree<TestSearchData>(0);

                //set tree logic
                tree.CreateTreeNode("A");
                tree.CreateTreeNode("B");
                tree.CreateTreeNode("C");
                tree.CreateTreeNode("D");
                tree.CreateTreeNode("E");
                tree.CreateTreeNode("F");

                tree.SetTreeNode("A", IsEqual, null, "B");
                tree.SetTreeNode("B", IsABigger10, "C", "D");
                tree.SetTreeNode("C", IsABigger20, "F", null);
                tree.SetTreeNode("D", IsBBigger10, "E", null);
                tree.SetTreeNode("E", IsBBigger20, null,null);
                tree.SetTreeNode("F", IsBBigger20, null, null);

                tree.SetRoot("A");

                //print tree
                DecisionTree.BTreePrinter<TestSearchData>.PrintTree(tree,tree.GetNode("A"));

                //print to the command window tree nodes and the paths leading to them 
                tree.PrintAllPathsToCommandWindow(isFullPath);

                //print to the output window tree nodes and the paths leading to them
                tree.PrintAllPathsToOutputWindow(isFullPath);

                //print to the output window a ready switch case to use in the result maneging function
                tree.PrintSwitchCasesToOutputWindow(isFullPath);

                //tests

                //A_Y (10, 10)
                //C_N (15, 10)
                //E_N (5, 15)
                //F_N (25, 10)
                TestSearchData testSearchData1 = new TestSearchData(10, 10);
                TestSearchData testSearchData2 = new TestSearchData(15, 10);
                TestSearchData testSearchData3 = new TestSearchData(5, 15);
                TestSearchData testSearchData4 = new TestSearchData(25, 10);

                DecisionTreeResult res1 = tree.Go(testSearchData1, isFullPath);
                DecisionTreeResult res2 = tree.Go(testSearchData2, isFullPath);
                DecisionTreeResult res3 = tree.Go(testSearchData3, isFullPath);
                DecisionTreeResult res4 = tree.Go(testSearchData4, isFullPath);

                //print test results 
                printRes(testSearchData1, res1.ResultCode);
                printRes(testSearchData2, res2.ResultCode);
                printRes(testSearchData3, res3.ResultCode);
                printRes(testSearchData4, res4.ResultCode);

                Console.WriteLine("----------------------------");

                //an example for a result maneging function
                resultManageCodeNames(res1.ResultCode,isFullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CodeNamesTest failed. Details in the output window.");
                System.Diagnostics.Debug.WriteLine(ex.ToString());

            }
        }

        private static void printRes(TestSearchData testSearchData, string resultCode)
        {
            Console.WriteLine(String.Format("numbers: {0},{1} ,result node: {2}", testSearchData.a, testSearchData.b, resultCode));
        }

        public static bool IsEqual(TestSearchData data) { return data.a == data.b; } //1
        public static bool IsABigger10(TestSearchData data) { return data.a > 10; } //2
        public static bool IsABigger20(TestSearchData data) { return data.a > 20; } //3
        public static bool IsBBigger10(TestSearchData data) { return data.b > 10; } //4
        public static bool IsBBigger20(TestSearchData data) { return data.b > 20; } //5

        private static void resultManage(string resultCode,bool isFullPath)
        {
            if (isFullPath)
            {
                switch ((resultCode))
                {
                    case "1_Y":
                        //IsEqual(Y)-
                        break;
                    case "1_2_3_6_Y":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(Y)-
                        break;
                    case "1_2_3_6_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(N)-
                        break;
                    case "1_2_3_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(N)-
                        break;
                    case "1_2_4_5_Y":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(Y)-
                        break;
                    case "1_2_4_5_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(N)-
                        break;
                    case "1_2_4_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(N)-
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch ((resultCode))
                {
                    case "1_Y":
                        //IsEqual(Y)-
                        break;
                    case "6_Y":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(Y)-
                        break;
                    case "6_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(N)-
                        break;
                    case "3_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(N)-
                        break;
                    case "5_Y":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(Y)-
                        break;
                    case "5_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(N)-
                        break;
                    case "4_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(N)-
                        break;
                    default:
                        break;
                }
            }

         
        }

        private static void resultManageCodeNames(string resultCode, bool isFullPath)
        {
            if (isFullPath)
            {
                switch ((resultCode))
                {
                    case "A_Y":
                        //IsEqual(Y)-
                        break;
                    case "A_B_C_F_Y":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(Y)-
                        break;
                    case "A_B_C_F_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(N)-
                        break;
                    case "A_B_C_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(N)-
                        break;
                    case "A_B_D_E_Y":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(Y)-
                        break;
                    case "A_B_D_E_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(N)-
                        break;
                    case "A_B_D_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(N)-
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch ((resultCode))
                {
                    case "A_Y":
                        //IsEqual(Y)-
                        break;
                    case "F_Y":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(Y)-
                        break;
                    case "F_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(Y)-IsBBigger20(N)-
                        break;
                    case "C_N":
                        //IsEqual(N)-IsABigger10(Y)-IsABigger20(N)-
                        break;
                    case "E_Y":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(Y)-
                        break;
                    case "E_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(Y)-IsBBigger20(N)-
                        break;
                    case "D_N":
                        //IsEqual(N)-IsABigger10(N)-IsBBigger10(N)-
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
