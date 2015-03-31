using System;

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
