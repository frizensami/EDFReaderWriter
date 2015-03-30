using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFReaderWriter.EDFLibrary.EDFData
{
    class EDFDataRecord
    {
        public int numSignals { get; private set; }
        public EDFDataRecordSignal[] signals;
        

        public EDFDataRecord(int numSignals, EDFDataRecordSignal[] signals)
        {
            if (numSignals == signals.Length)
            {
                this.numSignals = numSignals;
                this.signals = signals;
            }
            else
            {
                throw new ArgumentException("Provided signals do not have the number of signals expected! Expected " + numSignals + " got " + signals.Length);
            }
        }

        
    }
}
