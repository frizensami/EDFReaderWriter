using CNTFile;
using CNTFile.Zeo;
using EDFLibrary.EDFData.Types;
using EDFLibrary.EDFHeader;
using EDFReaderWriter.EDFLibrary.EDFData.Generator;
using EDFReaderWriter.Utility_Classes;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// Main function to write all signals data to EDF
        /// </summary>
        public void generateEDFData()
        {


            EDFHeader.EDFHeader header = ObjectHolder.EDFHeaderHolder;
            List<EDFSignal> signals = ObjectHolder.signals;

            //check if annotations are added
            Byte[][] annotationSignals;
            if (signals[signals.Count - 1].samples == null)
            {
                //TODO Add annotations
                int numAnnotationBytes = StripHeaderField.stripHeaderField((signals[signals.Count - 1].numSamples)) * 2; // times 2 as we need number of bytes, not number of 2-byte ints.

                DefaultAnnotationGenerator defGen = new DefaultAnnotationGenerator(StripHeaderField.stripHeaderField(header.numRecords), StripHeaderField.stripHeaderField(header.durationRecord), numAnnotationBytes);

                annotationSignals = defGen.annotationSignals;

            }
            else
            {
                annotationSignals = null;
            }

            EDFDataBlock dataBlock = new EDFDataBlock();
            dataBlock.numRecords = StripHeaderField.stripHeaderField(header.numRecords);
            dataBlock.records = new EDFDataRecord[dataBlock.numRecords]; //declare and define the records field

            for (int i = 0; i < dataBlock.numRecords; i++) //for all the records, proceed
            {
                //create the required data record, initialise fields
                EDFDataRecord record = new EDFDataRecord();
                record.numSignals = StripHeaderField.stripHeaderField(header.ns);
                EDFDataRecordSignal[] newSignals = new EDFDataRecordSignal[record.numSignals]; //will contain all the signals for this record


                for (int j = 0; j < (signals.Count - 1); j++) //for every signal except annotations, which will be handled separately
                {
                    //make a new signal for the record, initialise it, make a new sample array, and copy the required number of samples over to there
                    EDFDataRecordSignal newSignal = new EDFDataRecordSignal();
                    newSignal.numSamples = StripHeaderField.stripHeaderField(signals[j].numSamples);
                    newSignal.samples = new EDFDataRecordSignalSample[newSignal.numSamples];

                    int startCopy = i * newSignal.numSamples; //index to start copying from, then copy newSignal.numsample samples.
                    for (int k = 0; k < newSignal.numSamples; k++) //copy 256 items
                    {
                        newSignal.samples[k] = signals[j].samples[startCopy + k];
                    }


                        // Array.Copy(signals[j].samples, i * newSignal.numSamples, newSamples, 0, newSignal.numSamples); //be careful of overflows here

                        newSignals[j] = newSignal; //copy the new signal into the signals array for this record

                } //TODO what if the numSamples is not a clean multiple of the total number of samples in a signal? There will be uncopied samples 

                //handle annotations -> put into newSignals[signals.count - 1]
                Byte[] annotationSignal = annotationSignals[i];
                //declare a signal sample array that is half the length of the byte array
                EDFDataRecordSignalSample[] newAnnotationSamples = new EDFDataRecordSignalSample[annotationSignal.Length / 2];
                EDFDataRecordSignal newAnnotationSignal = new EDFDataRecordSignal();
                for (int k = 0; k < annotationSignal.Length; k += 2) //advanced 2 bytes by 2 bytes while converting them all into int16s 
                {
                    //convert to int16
                    Int16 convertedInt = BitConverter.ToInt16(annotationSignal, k);

                    //add this to a new signal sample
                    EDFDataRecordSignalSample newAnnotationSample = new EDFDataRecordSignalSample(convertedInt);

                    //assign the signal samples to an index each, but since the length is halved, the highest index is halved too
                    newAnnotationSamples[k / 2] = newAnnotationSample;


                }
                newAnnotationSignal.numSamples = StripHeaderField.stripHeaderField(signals[signals.Count - 1].numSamples); //get size of annotation field and set
                newAnnotationSignal.samples = newAnnotationSamples; //set the samples of the signal to the previously converted and assigned 2byte ints


                //finally, put this as the last signal in the signal array
                newSignals[signals.Count - 1] = newAnnotationSignal;

                //finally, add all signals to record and add that record to the dataBlock
                record.signals = newSignals;
                dataBlock.records[i] = record;
            }

            //get the entire EDF Datablock
            Int16[] outData = dataBlock.getEDFData();
            

            //write to output EDF
            using (BinaryWriter b = new BinaryWriter(File.Open(@"D:\out.edf", FileMode.Append)))
            {
                for (int i = 0; i < outData.Length; i++)
                {
                    
                    //Console.Write(outData[i] + " ");
                    b.Write(outData[i]);
                }
                b.Close();
            }

            
            Console.WriteLine("Done writing to file!");

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
