using EDFLibrary.Utility_Classes;
using System;
using System.Collections.Generic;

namespace EDFLibrary.EDFHeader
{
    /// <summary>
    /// Header definition for the EDF File. Call the EDFHeader constructor to set the basic arguments first, then call setNSDependantData to set the rest of the data. Finally, generateHeader returns the string representation of the header
    /// </summary>
    public class EDFHeader
    {
        //TODO Error checking and string verification!
        //TODO Add constants that define how many chars each field has
        private const int NUM_CHARS_VERSION = 8;
        private const int NUM_CHARS_PATIENT = 80;
        private const int NUM_CHARS_RECORDING = 80;
        private const int NUM_CHARS_START_DATE = 8;
        private const int NUM_CHARS_START_TIME = 8;
        private const int NUM_CHARS_NUM_BYTES = 8;
        private const int NUM_CHARS_RESERVED = 44;
        private const int NUM_CHARS_DATA_RECORDS = 8;
        private const int NUM_CHARS_DURATION_RECORD = 8;
        private const int NUM_CHARS_NS = 4;

        private const int NUM_CHARS_LABELS = 16;
        private const int NUM_CHARS_TRANSDUCER_TYPE = 80;
        private const int NUM_CHARS_PHYSICAL_DIMENSION = 8;
        private const int NUM_CHARS_PHYSICAL_MINIMUM = 8;
        private const int NUM_CHARS_PHYSICAL_MAXIMUM = 8;
        private const int NUM_CHARS_DIGITAL_MINUMUM = 8;
        private const int NUM_CHARS_DIGITAL_MAXIMUM = 8;
        private const int NUM_CHARS_PREFILTERING = 80;
        private const int NUM_CHARS_NUM_SAMPLES_PER_RECORD = 8;
        private const int NUM_CHARS_RESERVED_2 = 32;

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

        //path info
        public string FilePath { get; set; }

        public List<EDFSignal> edfSignals;

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
            //check if the data conforms to the expected number of signals
            int intNs = ConvertEDFHeaderField.toInt32(ns);
            if (iLabels.Count != intNs || iTransducerTypes.Count != intNs || iPhysicalDimensions.Count != intNs || iPhysicalMaximums.Count != intNs || iPhysicalMinimums.Count != intNs || iDigitalMinimums.Count != intNs || iDigitalMaximums.Count != intNs || iPrefilterings.Count != intNs || iNumSamplesPerRecords.Count != intNs)
            {
                throw new ArgumentException("One or more of the lists provuded in setNSDependantData do not have the expected number of elements i.e. the number of signals defined earlier");
            }


            //Initialise all advanced header field lists
            labels = new List<string>();
            transducerTypes = new List<string>();
            physicalDimensions = new List<string>();
            physicalMinimums = new List<string>();
            physicalMaximums = new List<string>();
            digitalMinimums = new List<string>();
            digitalMaximums = new List<string>();
            prefilterings = new List<string>();
            numSamplesPerRecords = new List<string>();

            //initialise EDF Signal list
            edfSignals = new List<EDFSignal>();


            //copy from the lists to the header fields
            setLabels(iLabels);
            settransducerTypes(iTransducerTypes);
            setphysicalDimensions(iPhysicalDimensions);
            setphysicalMinimums(iPhysicalMinimums);
            setphysicalMaximums(iPhysicalMaximums);
            setdigitalMinimums(iDigitalMinimums);
            setdigitalmaximums(iDigitalMaximums);
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
                edfSignals.Add(signal);

            }
        }

        
        /// <summary>
        /// Returns final header
        /// </summary>
        /// <returns></returns>
        public string generateEDFHeader()
        {
            string header = "";
            //initialise the numbytes block based on the formula 256 + 256 * ns
            numbytes = BuildEDFHeaderField.buildEDFHeaderField((Convert.ToString(256 + Convert.ToInt32(ns) * 256)), NUM_CHARS_NUM_BYTES);


            //build the end reserved block, 32 bytes * ns
            reserved2 = BuildEDFHeaderField.buildEDFHeaderField("", Convert.ToInt32(ns) * NUM_CHARS_RESERVED_2);

            //add basics
            header += edfVersion + localPatientIdent + localRecordingIdent + startDate + startTime + numbytes + reserved + numRecords +
                      durationRecord + ns;

            //add advanced fields
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

            //add final reserved header block
            header += reserved2;

            return header;
        }

        //basic field setters
        private void setVersion(string iEdfVersion)
        {
            edfVersion = BuildEDFHeaderField.buildEDFHeaderField(iEdfVersion, NUM_CHARS_VERSION);
        }
        private void setLocalPatientIdent(string iLocalPatientIdent)
        {
            localPatientIdent = BuildEDFHeaderField.buildEDFHeaderField(iLocalPatientIdent, NUM_CHARS_PATIENT);
        }
        private void setLocalRecordingIdent(string iLocalRecordingIdent)
        {
            localRecordingIdent = BuildEDFHeaderField.buildEDFHeaderField(iLocalRecordingIdent, NUM_CHARS_RECORDING);
        }
        private void setStartDate(string iStartDate)
        {
            startDate = BuildEDFHeaderField.buildEDFHeaderField(iStartDate, NUM_CHARS_START_DATE);
        }
        private void setStartTime(string iStartTime)
        {
            startTime = BuildEDFHeaderField.buildEDFHeaderField(iStartTime, NUM_CHARS_START_TIME);
        }
        private void setReserved(string iReserved) //special setter, ensure than reserved falls within the accepted fields
        {


            if (iReserved == EDFReserved.EDF_CONTINUOUS || iReserved == EDFReserved.EDF_DISCONTINUOUS)
                reserved = BuildEDFHeaderField.buildEDFHeaderField(iReserved, NUM_CHARS_RESERVED);
            else
                throw new ArgumentException("Reserved block only accepts EDF+C or EDF+D, please try again");

            //Reserved field -> uncomment below to override, just plain 44 bytes, this file will not be EDF+ compatible anymore
            // reserved = BuildHeader.buildHeader("", 44);
        }
        private void setNumRecords(string iNumRecords = "-1") //-1 signifies unknwon
        {
            numRecords = BuildEDFHeaderField.buildEDFHeaderField(iNumRecords, NUM_CHARS_DATA_RECORDS);
        }
        private void setDurationRecord(string iDurationRecord)
        {
            durationRecord = BuildEDFHeaderField.buildEDFHeaderField(iDurationRecord, NUM_CHARS_DURATION_RECORD);
        }
        private void setNs(string iNs)
        {
            ns = BuildEDFHeaderField.buildEDFHeaderField(Convert.ToString(Convert.ToInt32(iNs) + 1), NUM_CHARS_NS); //add 1 so that annotations are enforced under EDF+
        }


        //advanced setters

        private void setLabels(List<string> iLabels)
        {
            if (iLabels.Count == Convert.ToInt32(ns))
            {
                foreach (string label in iLabels)
                {
                    labels.Add(BuildEDFHeaderField.buildEDFHeaderField(label, NUM_CHARS_LABELS));
                }
            }

        }
        private void settransducerTypes(List<string> itransducerTypes)
        {
            if (itransducerTypes.Count == Convert.ToInt32(ns))
            {
                foreach (string transducerType in itransducerTypes)
                {
                    transducerTypes.Add(BuildEDFHeaderField.buildEDFHeaderField(transducerType, NUM_CHARS_TRANSDUCER_TYPE));
                }
            }

        }
        private void setphysicalDimensions(List<string> iphysicalDimensions)
        {
            if (iphysicalDimensions.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalDimension in iphysicalDimensions)
                {
                    physicalDimensions.Add(BuildEDFHeaderField.buildEDFHeaderField(physicalDimension, NUM_CHARS_PHYSICAL_DIMENSION));
                }
            }

        }
        private void setphysicalMinimums(List<string> iphysicalMinimums)
        {
            if (iphysicalMinimums.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalMinimum in iphysicalMinimums)
                {
                    physicalMinimums.Add(BuildEDFHeaderField.buildEDFHeaderField(physicalMinimum, NUM_CHARS_PHYSICAL_MINIMUM));
                }
            }

        }
        private void setphysicalMaximums(List<string> iphysicalmaximums)
        {
            if (iphysicalmaximums.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalmaximum in iphysicalmaximums)
                {
                    physicalMaximums.Add(BuildEDFHeaderField.buildEDFHeaderField(physicalmaximum, NUM_CHARS_PHYSICAL_MAXIMUM));
                }
            }

        }
        private void setdigitalMinimums(List<string> idigitalMinimums)
        {
            if (idigitalMinimums.Count == Convert.ToInt32(ns))
            {
                foreach (string digitalMinimum in idigitalMinimums)
                {
                    digitalMinimums.Add(BuildEDFHeaderField.buildEDFHeaderField(digitalMinimum, NUM_CHARS_DIGITAL_MINUMUM));
                }
            }

        }
        private void setdigitalmaximums(List<string> idigitalmaximums)
        {
            if (idigitalmaximums.Count == Convert.ToInt32(ns))
            {
                foreach (string digitalmaximum in idigitalmaximums)
                {
                    digitalMaximums.Add(BuildEDFHeaderField.buildEDFHeaderField(digitalmaximum, NUM_CHARS_DIGITAL_MAXIMUM));
                }
            }

        }
        private void setprefilterings(List<string> iprefilterings)
        {
            if (iprefilterings.Count == Convert.ToInt32(ns))
            {
                foreach (string prefiltering in iprefilterings)
                {
                    prefilterings.Add(BuildEDFHeaderField.buildEDFHeaderField(prefiltering, NUM_CHARS_PREFILTERING));
                }
            }

        }
        private void setnumSamplesPerRecords(List<string> inumSamplesPerRecords)
        {
            if (inumSamplesPerRecords.Count == Convert.ToInt32(ns))
            {
                foreach (string numSamplesPerRecord in inumSamplesPerRecords)
                {
                    numSamplesPerRecords.Add(BuildEDFHeaderField.buildEDFHeaderField(numSamplesPerRecord, NUM_CHARS_NUM_SAMPLES_PER_RECORD));
                }
            }

        }








    }
}
