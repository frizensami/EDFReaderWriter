using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFData.Types
{
    public class EDFDataRecordSignalSample
    {
        public Int16 sample { get; set; }
        public EDFDataRecordSignalSample()
        {

        }
        public EDFDataRecordSignalSample(Int16 sample)
        {
            this.sample = sample;
        }
    }
}
