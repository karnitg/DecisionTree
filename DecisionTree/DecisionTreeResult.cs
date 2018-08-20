using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DecisionTreeResult
    {
        public String ResultCode { get; set; }
        public Dictionary<String,bool> Path { get; set; }
    }
}
