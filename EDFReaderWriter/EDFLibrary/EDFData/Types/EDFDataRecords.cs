using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFData.Types
{
    class EDFDataRecords
    {
        public int numRecords { get; set; }
        public EDFDataRecord[] records;

        public EDFDataRecords()
        {

        }

        public EDFDataRecords(int numRecords, EDFDataRecord[] records)
        {
            if (numRecords == records.Length)
            {
                this.numRecords = numRecords;
                this.records = records;
            }
            else
            {
                throw new ArgumentException("Expected " + numRecords + " records, got " + records.Length + "!");
            }
        }

        public Int16[] getEDFDate()
        {
            
            List<Int16> outData = new List<Int16>();
            for (int i = 0; i < records.Length; i++)
            {
                for (int j = 0; j < records[i].signals.Length; j++)
                {
                    for (int k = 0; k < records[i].signals[j].samples.Length; k++)
                    {
                        outData.Add(records[i].signals[j].samples[k].sample);
                    }
                }
            }

            return outData.ToArray();
        }

    }
}
