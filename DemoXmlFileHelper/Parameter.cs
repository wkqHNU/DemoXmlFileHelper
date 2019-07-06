using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoXmlFileHelper
{
    [Serializable]
    public class Parameter<T>
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        

        private T value;

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
