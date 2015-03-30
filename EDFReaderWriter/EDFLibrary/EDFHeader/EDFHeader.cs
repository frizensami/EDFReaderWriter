using EDFReaderWriter.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFHeader
{
    public class EDFHeader
    {

        //basic info
        public string edfVersion { get; private set; } //0 by default
        public string localPatientIdent { get; private set; } //patient identification
        public string localRecordingIdent { get; private set; } //equipment used, technician name, etc, check spec
        public string startDate { get; private set; } //start date of record
        public string startTime { get; private set; } //start time of record
        public string reserved { get; private set; } //reserved block, only accept EDF+C or EDF+D
        public string numRecords { get; private set; } //-1 if unknown
        public string durationRecord { get; private set; } //duration of one record in seconds
        public string ns { get; private set; } //number of signals in one data record


        //info with size dependant on ns
        public List<string> labels { get; private set; }
        public List<string> transducerTypes { get; private set; }
        public List<string> physicalDimensions { get; private set; }
        public List<string> physicalMinimums { get; private set; }
        public List<string> physicalMaximums { get; private set; }
        public List<string> digitalMinimums { get; private set; }
        public List<string> digitalMaximums { get; private set; }
        public List<string> prefilterings { get; private set; }
        public List<string> numSamplesPerRecords { get; private set; }

        string reserved2;


        string numbytes; //calculate last -> number of bytes in header

        /// <summary>
        /// Use this construtor to initialise the base object EDFHeader. To complete it, call upon setNSDependantData
        /// </summary>
        /// <param name="iEdfVersion"></param>
        /// <param name="iLocalPatientIdent"></param>
        /// <param name="iLocalRecordingIdent"></param>
        /// <param name="iStartDate"></param>
        /// <param name="iStartTime"></param>
        /// <param name="iReserved"></param>
        /// <param name="iNumRecords"></param>
        /// <param name="iDurationRecord"></param>
        /// <param name="iNs"></param>

        public EDFHeader(string iEdfVersion, string iLocalPatientIdent, string iLocalRecordingIdent, string iStartDate, string iStartTime,
                         string iReserved, string iNumRecords, string iDurationRecord, string iNs)
        {
            setVersion(iEdfVersion);
            setLocalPatientIdent(iLocalPatientIdent);
            setLocalRecordingIdent(iLocalRecordingIdent);
            setStartDate(iStartDate);
            setStartTime(iStartTime);
            setReserved(iReserved);
            setNumRecords(iNumRecords);
            setDurationRecord(iDurationRecord);
            setNs(iNs);


        }

        /// <summary>
        /// This function completes all the data required for the header. Call generateEDFHeader to get the completed header
        /// </summary>
        /// <param name="iLabels"></param>
        /// <param name="iTransducerTypes"></param>
        /// <param name="iPhysicalDimensions"></param>
        /// <param name="iPhysicalMinimums"></param>
        /// <param name="iPhysicalMaximums"></param>
        /// <param name="iDigitalMinimums"></param>
        /// <param name="iDigitalMaximums"></param>
        /// <param name="iPrefilterings"></param>
        /// <param name="iNumSamplesPerRecords"></param>
        public void setNSDependantData(List<string> iLabels, List<string> iTransducerTypes, List<string> iPhysicalDimensions,
                                        List<string> iPhysicalMinimums,
                                        List<string> iPhysicalMaximums, List<string> iDigitalMinimums, List<string> iDigitalMaximums, List<string> iPrefilterings,
                                        List<string> iNumSamplesPerRecords)
        {

            labels = new List<string>();
            transducerTypes = new List<string>();
            physicalDimensions = new List<string>();
            physicalMinimums = new List<string>();
            physicalMaximums = new List<string>();
            digitalMinimums = new List<string>();
            digitalMaximums = new List<string>();
            prefilterings = new List<string>();
            numSamplesPerRecords = new List<string>();

            setLabels(iLabels);
            settransducerTypes(iTransducerTypes);
            setphysicalDimensions(iPhysicalDimensions);
            setphysicalMinimums(iPhysicalMinimums);
            setphysicalMaximums(iPhysicalMaximums);
            setdigitalMinimums(iPhysicalMinimums);
            setdigitalmaximums(iPhysicalMaximums);
            setprefilterings(iPrefilterings);
            setnumSamplesPerRecords(iNumSamplesPerRecords);

            for (int i = 0; i < iLabels.Count; i++)
            {
                //add every signal to the application global list of signals
                EDFSignal signal = new EDFSignal();
                signal.label = iLabels[i];
                signal.transducerType = iTransducerTypes[i];
                signal.physicalDimension = iPhysicalDimensions[i];
                signal.physicalMinimum = iPhysicalMinimums[i];
                signal.physicalMaximum = iPhysicalMaximums[i];
                signal.digitalMinimum = iDigitalMinimums[i];
                signal.digitalMaximum = iDigitalMaximums[i];
                signal.preFiltering = iPrefilterings[i];
                signal.numSamples = iNumSamplesPerRecords[i];
                ObjectHolder.signals.Add(signal);

            }
        }

        /// <summary>
        /// returns the final header
        /// </summary>
        /// <returns></returns>
        public string generateEDFHeader()
        {
            string header = "";
            //initialise the numbytes block - just for the start
            numbytes = BuildHeader.buildHeader((Convert.ToString(256 + Convert.ToInt32(ns) * 256)), 8);


            //build the end reserved block
            reserved2 = BuildHeader.buildHeader("", Convert.ToInt32(ns) * 32);

            //add basics
            header += edfVersion + localPatientIdent + localRecordingIdent + startDate + startTime + numbytes + reserved + numRecords +
                      durationRecord + ns;

            //add advanced stuff
            foreach (string x in labels)
            {
                header += x;
            }
            foreach (string x in transducerTypes)
            {
                header += x;
            }
            foreach (string x in physicalDimensions)
            {
                header += x;
            }
            foreach (string x in physicalMinimums)
            {
                header += x;
            }
            foreach (string x in physicalMaximums)
            {
                header += x;
            }
            foreach (string x in digitalMinimums)
            {
                header += x;
            }
            foreach (string x in digitalMaximums)
            {
                header += x;
            }
            foreach (string x in prefilterings)
            {
                header += x;
            }
            foreach (string x in numSamplesPerRecords)
            {
                header += x;
            }


            header += reserved2;

            return header;
        }

        //physicalMinimums     
        //physicalMaximums     
        //digitalMinimums      
        //digitalMaximums      
        //prefilterings        
        //numSamplesPerRecords 
        //basic setters

        private void setVersion(string iEdfVersion)
        {
            edfVersion = BuildHeader.buildHeader(iEdfVersion, 8);
        }
        private void setLocalPatientIdent(string iLocalPatientIdent)
        {
            localPatientIdent = BuildHeader.buildHeader(iLocalPatientIdent, 80);
        }
        private void setLocalRecordingIdent(string iLocalRecordingIdent)
        {
            localRecordingIdent = BuildHeader.buildHeader(iLocalRecordingIdent, 80);
        }
        private void setStartDate(string iStartDate)
        {
            startDate = BuildHeader.buildHeader(iStartDate, 8);
        }
        private void setStartTime(string iStartTime)
        {
            startTime = BuildHeader.buildHeader(iStartTime, 8);
        }
        private void setReserved(string iReserved)
        {


            if (iReserved == EDFReserved.EDF_CONTINUOUS || iReserved == EDFReserved.EDF_DISCONTINUOUS)
                reserved = BuildHeader.buildHeader(iReserved, 44);
            else
                throw new ApplicationException("Reserved block only accepts EDF+C or EDF+D, please try again");

            //Reserved field -> uncomment below to override, just plain 44 bytes, this file will not be EDF+ compatible anymore
            // reserved = BuildHeader.buildHeader("", 44);
        }
        private void setNumRecords(string iNumRecords = "-1") //-1 signifies unknwon
        {
            numRecords = BuildHeader.buildHeader(iNumRecords, 8);
        }
        private void setDurationRecord(string iDurationRecord)
        {
            durationRecord = BuildHeader.buildHeader(iDurationRecord, 8);
        }
        private void setNs(string iNs)
        {
            ns = BuildHeader.buildHeader(Convert.ToString(Convert.ToInt32(iNs) + 1), 4); //add 1 so that annotations are enforced under EDF+
        }


        //advanced setters

        private void setLabels(List<string> iLabels)
        {
            if (iLabels.Count == Convert.ToInt32(ns))
            {
                foreach (string label in iLabels)
                {
                    labels.Add(BuildHeader.buildHeader(label, 16));
                }
            }

        }
        private void settransducerTypes(List<string> itransducerTypes)
        {
            if (itransducerTypes.Count == Convert.ToInt32(ns))
            {
                foreach (string transducerType in itransducerTypes)
                {
                    transducerTypes.Add(BuildHeader.buildHeader(transducerType, 80));
                }
            }

        }
        private void setphysicalDimensions(List<string> iphysicalDimensions)
        {
            if (iphysicalDimensions.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalDimension in iphysicalDimensions)
                {
                    physicalDimensions.Add(BuildHeader.buildHeader(physicalDimension, 8));
                }
            }

        }
        private void setphysicalMinimums(List<string> iphysicalMinimums)
        {
            if (iphysicalMinimums.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalMinimum in iphysicalMinimums)
                {
                    physicalMinimums.Add(BuildHeader.buildHeader(physicalMinimum, 8));
                }
            }

        }
        private void setphysicalMaximums(List<string> iphysicalmaximums)
        {
            if (iphysicalmaximums.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalmaximum in iphysicalmaximums)
                {
                    physicalMaximums.Add(BuildHeader.buildHeader(physicalmaximum, 8));
                }
            }

        }
        private void setdigitalMinimums(List<string> idigitalMinimums)
        {
            if (idigitalMinimums.Count == Convert.ToInt32(ns))
            {
                foreach (string digitalMinimum in idigitalMinimums)
                {
                    digitalMinimums.Add(BuildHeader.buildHeader(digitalMinimum, 8));
                }
            }

        }
        private void setdigitalmaximums(List<string> idigitalmaximums)
        {
            if (idigitalmaximums.Count == Convert.ToInt32(ns))
            {
                foreach (string digitalmaximum in idigitalmaximums)
                {
                    digitalMaximums.Add(BuildHeader.buildHeader(digitalmaximum, 8));
                }
            }

        }
        private void setprefilterings(List<string> iprefilterings)
        {
            if (iprefilterings.Count == Convert.ToInt32(ns))
            {
                foreach (string prefiltering in iprefilterings)
                {
                    prefilterings.Add(BuildHeader.buildHeader(prefiltering, 80));
                }
            }

        }
        private void setnumSamplesPerRecords(List<string> inumSamplesPerRecords)
        {
            if (inumSamplesPerRecords.Count == Convert.ToInt32(ns))
            {
                foreach (string numSamplesPerRecord in inumSamplesPerRecords)
                {
                    numSamplesPerRecords.Add(BuildHeader.buildHeader(numSamplesPerRecord, 8));
                }
            }

        }

      






    }
}
