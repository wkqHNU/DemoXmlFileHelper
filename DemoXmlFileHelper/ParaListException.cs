using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoXmlFileHelper
{
    class ParaListException : ApplicationException
    {
        //throw new ParaListException();
        public ParaListException(string message) : base(message)
        { }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
