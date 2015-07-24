using CNTFile;
using CNTFile.Zeo;
using EDFLibrary.EDFData.Types;
using EDFLibrary.EDFHeader;
using EDFLibrary.EDFLibrary.EDFData.Generator;
using EDFLibrary.Utility_Classes;
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
        /// NOTE:: RETURNS NEW MODIFIED HEADER OBJECT. This function considers the filetype required to parse, then assigns the work to another function. It requires the header object to modify and the index of the signal to be written to.
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="path"></param>
        /// <param name="signalIndex"></param>
        public EDFHeader.EDFHeader addFile(FileTypes fileType, string path, EDFHeader.EDFHeader iHeader, int signalIndexStart, int signalIndexEnd) 
        {

            Console.WriteLine("Adding file {0}", path);
            switch (fileType)
            {
                case FileTypes.zeoEEGCNT:
                    return addZeoEEGCNT(path, iHeader, signalIndexStart, signalIndexEnd);
                default:
                    return iHeader;
                    throw new ArgumentException("Adding file failed. Specfied filetype argument incorrect");
            }



        }
        /// <summary>
        /// Specific function to add Zeo EEG CNT files
        /// </summary>
        /// <param name="path"></param>
        /// <param name="signalIndex"></param>
        private EDFHeader.EDFHeader addZeoEEGCNT(string path, EDFHeader.EDFHeader iHeader, int signalIndexStart, int signalIndexEnd)
        {
            EDFHeader.EDFHeader header = iHeader;
            EEGCntFile file = new EEGCntFile();


            //file.ReadCnt(System.IO.Path.GetFullPath(@"test3.cnt"));
            file.ReadCnt(path);
            float[,] eeg_data = file.eeg_data;
            //add code to deal with multiple signals here

            int sampleSize = eeg_data.GetLength(1);
            

            for (int j = signalIndexStart, k = 0; j <= signalIndexEnd; j++, k++)
            {
                EDFDataRecordSignalSample[] samples = new EDFDataRecordSignalSample[sampleSize];
                for (int i = 0; i < eeg_data.GetLength(1); i++)
                {
                    //TODO get more than 1 channel of CNT data
                    
                    float rawData = eeg_data[k, i]; //k is the channel number
                    Int16 digitalData = ZeoFloatToDigital.zeoFloatToDigital(rawData);
                    samples[i] = new EDFDataRecordSignalSample(digitalData);
                    //Console.WriteLine("Samples[{0}] = {1}", i, samples[i].sample);
                }
                header.edfSignals[j].samples = samples;
            }
               
            return header;

        }
        /// <summary>
        /// Main function to write all signals data to EDF
        /// </summary>
        public void generateEDFData(EDFHeader.EDFHeader iHeader, string path)
        {


            EDFHeader.EDFHeader header = iHeader;
            List<EDFSignal> signals = header.edfSignals;

            //check if annotations are added
            Byte[][] annotationSignals;
            if (signals[signals.Count - 1].samples == null)
            {
                //TODO Add annotations functionality above the default requirements
                int numAnnotationBytes = ConvertEDFHeaderField.toInt32((signals[signals.Count - 1].numSamples)) * 2; // times 2 as we need number of bytes, not number of 2-byte ints.

                DefaultAnnotationGenerator defGen = new DefaultAnnotationGenerator(ConvertEDFHeaderField.toInt32(header.numRecords), ConvertEDFHeaderField.toInt32(header.durationRecord), numAnnotationBytes);

                annotationSignals = defGen.getAnnotationSignals();

            }
            else
            {
                annotationSignals = null;
            }

            EDFDataBlock dataBlock = new EDFDataBlock();
            dataBlock.numRecords = ConvertEDFHeaderField.toInt32(header.numRecords);
            dataBlock.records = new EDFDataRecord[dataBlock.numRecords]; //declare and define the records field

            for (int i = 0; i < dataBlock.numRecords; i++) //for all the records, proceed
            {
                //create the required data record, initialise fields
                EDFDataRecord record = new EDFDataRecord();
                record.numSignals = ConvertEDFHeaderField.toInt32(header.ns);
                EDFDataRecordSignal[] newSignals = new EDFDataRecordSignal[record.numSignals]; //will contain all the signals for this record


                for (int j = 0; j < (signals.Count - 1); j++) //for every signal except annotations, which will be handled separately
                {
                    //make a new signal for the record, initialise it, make a new sample array, and copy the required number of samples over to there
                    EDFDataRecordSignal newSignal = new EDFDataRecordSignal();
                    newSignal.numSamples = ConvertEDFHeaderField.toInt32(signals[j].numSamples);
                    newSignal.samples = new EDFDataRecordSignalSample[newSignal.numSamples];

                    int startCopy = i * newSignal.numSamples; //index to start copying from, then copy newSignal.numsample samples.
                    for (int k = 0; k < newSignal.numSamples; k++) //copy 256 items
                    {
                        newSignal.samples[k] = signals[j].samples[startCopy + k];
                    }


                        // Array.Copy(signals[j].samples, i * newSignal.numSamples, newSamples, 0, newSignal.numSamples); //be careful of overflows here

                        newSignals[j] = newSignal; //copy the new signal into the signals array for this record

                } 

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
                newAnnotationSignal.numSamples = ConvertEDFHeaderField.toInt32(signals[signals.Count - 1].numSamples); //get size of annotation field and set
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
            using (BinaryWriter b = new BinaryWriter(File.Open(path, FileMode.Append))) //TODO allow file to be specified!
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

       
    }
}
