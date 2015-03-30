using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFReaderWriter.EDFLibrary
{
    class EDFDataRecordSignalSample
    {
        public Int16 sample { get; private set; }
        public EDFDataRecordSignalSample(Int16 sample)
        {
            this.sample = sample;
        }
    }
}
