using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFData.Types
{
    class EDFDataRecordSignal
    {
        public int numSamples { get; set; }
        public EDFDataRecordSignalSample[] samples { get; set; }

        public EDFDataRecordSignal()
        {

        }

        public EDFDataRecordSignal(int numSamples, EDFDataRecordSignalSample[] samples)
        {
            if (numSamples == samples.Length)
            {
                this.numSamples = numSamples;
                this.samples = samples;
            }
            else
            {
                throw new ArgumentException("Provided sample array does not have the number of samples expected! Expected " + numSamples + " got " + samples.Length);
            }



        }
    }
}
