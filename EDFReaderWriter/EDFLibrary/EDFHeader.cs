using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary
{
    class EDFHeader
    {

        //basic info
        string edfVersion; //0 by default
        string localPatientIdent; //patient identification
        string localRecordingIdent; //equipment used, technician name, etc, check spec
        string startDate; //start date of record
        string startTime; //start time of record
        string reserved; //reserved block, only accept EDF+C or EDF+D
        string numRecords; //-1 if unknown
        string durationRecord; //duration of one record in seconds
        string ns; //number of signals in one data record


        //info with size dependant on ns
        List<string> labels;
        List<string> transducerTypes;
        List<string> physicalDimensions;
        List<string> physicalMinimums;
        List<string> physicalMaximums;
        List<string> digitalMinimums;
        List<string> digitalMaximums;
        List<string> prefilterings;
        List<string> numSamplesPerRecords;

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
                                        
           labels                        = new List<string>();
           transducerTypes               = new List<string>();
           physicalDimensions            = new List<string>();
           physicalMinimums              = new List<string>();
           physicalMaximums              = new List<string>();
           digitalMinimums               = new List<string>();
           digitalMaximums               = new List<string>();
           prefilterings                  = new List<string>();
           numSamplesPerRecords           = new List<string>();
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
        private void setNSDependantData(List<string> iLabels, List<string> iTransducerTypes, List<string> iPhysicalDimensions, 
                                        List<string> iPhysicalMinimums, 
                                        List<string> iPhysicalMaximums, List<string> iDigitalMinimums, List<string> iDigitalMaximums,           List<string> iPrefilterings, 
                                        List<string> iNumSamplesPerRecords)
        {
            setLabels(iLabels);
            settransducerTypes(iTransducerTypes);
            setphysicalDimensions(iPhysicalDimensions);
            setphysicalMinimums(iPhysicalMinimums);
            setphysicalMaximums(iPhysicalMaximums);
            setdigitalMinimums(iPhysicalMinimums);
            setdigitalmaximums(iPhysicalMaximums);
            setprefilterings(iPrefilterings);
            setnumSamplesPerRecords(iNumSamplesPerRecords);
        }

        /// <summary>
        /// returns the final header
        /// </summary>
        /// <returns></returns>
        private string generateEDFHeader()
        {
            string header = "";
            //initialise the numbytes block - just for the start
            numbytes = Convert.ToString(256 + Convert.ToInt32(ns) * 256);
            

            //build the end reserved block
            reserved2 = BuildHeader.buildHeader("", 32);

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
                throw new ApplicationException("Reserved block only accepted EDF+C or EDF+D, please try again");
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
            ns = BuildHeader.buildHeader(iNs, 4);
        }


    //advanced setters

        public void setLabels(List<string> iLabels)
        {
            if (iLabels.Count == Convert.ToInt32(ns))
            {
                foreach (string label in iLabels)
                {
                    labels.Add(BuildHeader.buildHeader(label, 16));
                }
            }
                
        }
        public void settransducerTypes(List<string> itransducerTypes)
        {
            if (itransducerTypes.Count == Convert.ToInt32(ns))
            {
                foreach (string transducerType in itransducerTypes)
                {
                    transducerTypes.Add(BuildHeader.buildHeader(transducerType, 80));
                }
            }

        }
        public void setphysicalDimensions(List<string> iphysicalDimensions)
        {
            if (iphysicalDimensions.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalDimension in iphysicalDimensions)
                {
                    physicalDimensions.Add(BuildHeader.buildHeader(physicalDimension, 8));
                }
            }

        }
        public void setphysicalMinimums(List<string> iphysicalMinimums)
        {
            if (iphysicalMinimums.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalMinimum in iphysicalMinimums)
                {
                    physicalMinimums.Add(BuildHeader.buildHeader(physicalMinimum, 8));
                }
            }

        }
        public void setphysicalMaximums(List<string> iphysicalmaximums)
        {
            if (iphysicalmaximums.Count == Convert.ToInt32(ns))
            {
                foreach (string physicalmaximum in iphysicalmaximums)
                {
                    physicalMaximums.Add(BuildHeader.buildHeader(physicalmaximum, 8));
                }
            }

        }
        public void setdigitalMinimums(List<string> idigitalMinimums)
        {
            if (idigitalMinimums.Count == Convert.ToInt32(ns))
            {
                foreach (string digitalMinimum in idigitalMinimums)
                {
                    digitalMinimums.Add(BuildHeader.buildHeader(digitalMinimum, 8));
                }
            }

        }
        public void setdigitalmaximums(List<string> idigitalmaximums)
        {
            if (idigitalmaximums.Count == Convert.ToInt32(ns))
            {
                foreach (string digitalmaximum in idigitalmaximums)
                {
                    digitalMaximums.Add(BuildHeader.buildHeader(digitalmaximum, 8));
                }
            }

        }
        public void setprefilterings(List<string> iprefilterings)
        {
            if (iprefilterings.Count == Convert.ToInt32(ns))
            {
                foreach (string prefiltering in iprefilterings)
                {
                    prefilterings.Add(BuildHeader.buildHeader(prefiltering, 80));
                }
            }

        }
        public void setnumSamplesPerRecords(List<string> inumSamplesPerRecords)
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
