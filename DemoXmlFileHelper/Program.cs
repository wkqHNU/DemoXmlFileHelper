using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoXmlFileHelper.EnumList;

namespace DemoXmlFileHelper
{
    class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            GetCheckParaList();
        }
        public static ReturnInfo GetCheckParaList()
        {
            ReturnInfo xmlInfo;
            xmlInfo = new ReturnInfo();

            try
            {
                // 1.从xml中读取节点到参数列表ParaList
                string filePath = @"..\..\TP_Agilent5071C_Passive.xml";
                Xml2ParaList.LoadParaData(filePath);
                // 2.从参数列表ParaList中读取参数
                int count = Xml2ParaList.ParaList.Count;
                int thetaStart = Xml2ParaList.GetParaValue<int>(Xml2ParaList.ParaList, "Theta_Start");

                // 3.检查模板参数是否合理
                thetaStart = Xml2ParaList.CheckRange<int>(thetaStart, nameof(thetaStart), 0, 180);
                Log.PrintLog(LogMsgType.Normal, "Each parameter check succeeded");
            }
            catch(Exception ex)
            {
                xmlInfo.isOk = false;
                xmlInfo.explain = ex.Message;
            }
            Console.ReadLine();
            return xmlInfo;
        }
    }
}
