using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoXmlFileHelper.EnumList;

namespace DemoXmlFileHelper
{
    class Log
    {
        public static void PrintLog(LogMsgType type, string format, params object[] args)
        {
            string msg = string.Format(format, args);
            Console.WriteLine(msg);
        }
    }
}
