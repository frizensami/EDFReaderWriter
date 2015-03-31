using System;

namespace EDFLibrary.EDFData.Types
{
    class EDFDataRecord
    {
        public int numSignals { get; set; }
        public EDFDataRecordSignal[] signals;
        
        public EDFDataRecord()
        {

        }

        public EDFDataRecord(int numSignals, EDFDataRecordSignal[] signals)
        {
            if (numSignals == signals.Length)
            {
                this.numSignals = numSignals;
                this.signals = signals;
            }
            else
            {
                throw new ArgumentException("Provided signal array does not have the number of signals expected! Expected " + numSignals + " got " + signals.Length);
            }
        }

        
    }
}
