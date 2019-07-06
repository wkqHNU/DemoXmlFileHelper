using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoXmlFileHelper
{
    class FreqBlock
    {
        public FreqBlock() { }
        public FreqBlock(double start, double end, double interval)
        {
            this.start = start;
            this.stop = end;
            this.step = interval;
        }

        private double start;

        public double Start
        {
            get { return start; }
            set
            {
                start = value;
            }
        }

        private double stop;

        public double Stop
        {
            get { return stop; }
            set
            {
                stop = value;
            }
        }

        private double step;

        public double Step
        {
            get { return step; }
            set
            {
                step = value;
            }
        }

        private int points;

        public int Points
        {
            get { return points; }
            set
            {
                points = value;
            }
        }
    }
}
