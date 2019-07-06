using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoXmlFileHelper
{
    class EnumList
    {
        public enum LogMsgType
        {
            Normal,
            Error,
            Debug,
            Warning,
        }

        public enum NodeStyle
        {
            TextBox,
            ComboBox,
            FileSelect,
            FolderSelect,
            NoDisplay,
            FrequencyList,
            Instrument,
            BandAndCh,
        }
    }
}
