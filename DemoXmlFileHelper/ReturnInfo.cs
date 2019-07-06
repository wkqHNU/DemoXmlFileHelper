using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoXmlFileHelper
{
    public class ReturnInfo
    {
        public bool isOk { set; get; }
        public string explain { set; get; }
        public ReturnInfo()
        {
            this.isOk = true;
        }
        public ReturnInfo(bool isOk, string explain)
        {
            this.isOk = isOk;
            this.explain = explain;
        }
    }
}
