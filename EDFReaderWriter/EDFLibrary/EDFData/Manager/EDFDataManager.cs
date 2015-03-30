using CNTFile;
using CNTFile.Zeo;
using EDFLibrary.EDFData.Types;
using EDFLibrary.EDFHeader;
using EDFReaderWriter.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFData.Manager
{
    class EDFDataManager
    {
        public enum FileTypes
        {
            zeoEEGCNT
        }

        public EDFDataManager()
        {

        }
        /// <summary>
        /// Based on the filetype specified, this function adds data to the signal object. The signal index gives the index of the global signals list to get the signal metadata from
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="path"></param>
        /// <param name="signalIndex"></param>
        public void addFile(FileTypes fileType, string path, int signalIndex) // TODO signal index is very disgusting, only works for this program. Fix!
        {
            Console.WriteLine("Adding file {0}", path);
            switch (fileType)
            {
                case FileTypes.zeoEEGCNT:
                    addZeoEEGCNT(path, signalIndex);
                    break;
                default:
                    throw new ArgumentException("Adding file failed.");
            }
            
            

        }
        /// <summary>
        /// Hopefully by now all the signals have been populated with data, and we can start writing to the EDF Data Block
        /// </summary>
        public void generateEDFData()
        {


            EDFHeader.EDFHeader header = ObjectHolder.EDFHeaderHolder;
            List<EDFSignal> signals = ObjectHolder.signals;

            //check if annotations are added
            if (signals[signals.Count - 1].samples == null)
            {
                //TODO Add annotations
            }

            EDFDataRecords records = new EDFDataRecords();
            records.numRecords = StripHeaderField.stripHeaderField(header.numRecords);

            foreach (EDFSignal signal in signals)
            {
                for (int i = 0; i < signal.samples.Length; i += StripHeaderField.stripHeaderField(signal.numSamples))
                {
                    Console.WriteLine(i);
                }
            }
        }

        private void addZeoEEGCNT(string path, int signalIndex)
        {
            EDFLibrary.EDFHeader.EDFHeader header = EDFReaderWriter.Utility_Classes.ObjectHolder.EDFHeaderHolder;
            EEGCntFile file = new EEGCntFile();


            //file.ReadCnt(System.IO.Path.GetFullPath(@"test3.cnt"));
            file.ReadCnt(path);
            float[,] eeg_data = file.eeg_data;
            int sampleSize = eeg_data.GetLength(1);
            EDFDataRecordSignalSample[] samples = new EDFDataRecordSignalSample[sampleSize];
            for (int i = 0; i < eeg_data.GetLength(1); i++)
            {
                float rawData = eeg_data[0, i]; //first index is static as we just want 1 channel of zeo data
                Int16 digitalData = ZeoFloatToDigital.zeoFloatToDigital(rawData);
                samples[i] = new EDFDataRecordSignalSample(digitalData);
                //Console.WriteLine("Samples[{0}] = {1}", i, samples[i].sample);
            }
            ObjectHolder.signals[signalIndex].samples = samples;
            
          
        }
    }
}
