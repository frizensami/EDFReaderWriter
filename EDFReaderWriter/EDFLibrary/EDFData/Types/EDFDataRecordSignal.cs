using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFReaderWriter.EDFLibrary
{
    class EDFDataRecordSignal
    {
        public int numSamples { get; private set; }
        public EDFDataRecordSignalSample[] samples { get; private set; }
        
        public EDFDataRecordSignal(int numSamples, EDFDataRecordSignalSample[] samples)
        {
            if (numSamples == samples.Length)
            {
                this.numSamples = numSamples;
                this.samples = samples;
            }
            else
            {
                throw new ArgumentException("Provided samples do not have the number of samples expected! Expected " + numSamples + " got " + samples.Length);
            }
               
            
            
        }
    }
}
