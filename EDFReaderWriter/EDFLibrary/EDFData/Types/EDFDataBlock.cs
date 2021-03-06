﻿using System;
using System.Collections.Generic;

namespace EDFLibrary.EDFData.Types
{
    class EDFDataBlock
    {
        public int numRecords { get; set; }
        public EDFDataRecord[] records;

        public EDFDataBlock()
        {
            
        }

        public EDFDataBlock(int numRecords, EDFDataRecord[] records)
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

        public Int16[] getEDFData()
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
