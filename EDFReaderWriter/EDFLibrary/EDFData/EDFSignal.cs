using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFReaderWriter.EDFLibrary
{
    class EDFSignal
    {
        public string label { get; set; }
        public string transducerType { get; set; }
        public string physicalDimension { get; set; }
        public string physicalMinimum { get; set; }
        public string physicalMaximum { get; set; }
        public string digitalMinimum { get; set; }
        public string digitalMaximum { get; set; }
        public string preFiltering { get; set; }
        public string numSamples { get; set; }
    }
}
