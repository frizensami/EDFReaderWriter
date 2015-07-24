using System;
using System.Collections.Generic;

namespace EDFLibrary.EDFData.Types.TAL
{
    class EDFTALAnnotation
    {
        

        //TODO do up this annotation to allow for definition of last and first TALs, and more than 1 tal per record (totalbytes currently limits this artifically.
        public char onsetSign { get; set; }
        public double onset { get; set; }
        public double duration { get; set; }
        public string[] annotations { get; set; }
        public int totalBytes { get; set; }
        public int remainingBytes { get; set; }

        private const byte beforeDuration = 21;
        private const byte beforeAnnotation = 20;
        private const byte afterAnnotation = 20;
        private const byte endOfTAL = 0;

        public List<byte> builtAnnotation;

        
        /// <summary>
        /// Constructor for an EDF TAL Annotation that is both first and last TAL --> Used for default annotations
        /// </summary>
        /// <param name="firstTAL"></param>
        /// <param name="onsetSign"></param>
        /// <param name="onset"></param>
        /// <param name="annotations"></param>
        public EDFTALAnnotation(char onsetSign, double onset, string[] annotations, int totalBytes)
        {
            builtAnnotation = new List<byte>();
            this.onsetSign = onsetSign;
            this.onset = onset;
            this.duration = duration;
            this.annotations = annotations;
            this.totalBytes = totalBytes;

            

            builtAnnotation.Add((Byte)onsetSign); //write onset sign (+/-)

            builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(Convert.ToString(onset))); // convert onset to bytes and add to the byte array

            builtAnnotation.Add(beforeAnnotation);
            builtAnnotation.Add(beforeAnnotation);




            for (int i = 0; i < annotations.Length; i++)
            {
                builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(annotations[i]));
                builtAnnotation.Add(afterAnnotation);
            }



            if ((totalBytes - builtAnnotation.Count) > 0)
            {
                
                int remainingBytes = totalBytes - builtAnnotation.Count;
                //Console.WriteLine("total: {0}, remaining: {1}", totalBytes, remainingBytes);

                for (int i = 0; i < remainingBytes; i++)
                {
                    builtAnnotation.Add(endOfTAL);
                }
            }
            else
            {
                throw new ArgumentException("Not enough bytes remaining to complete annotation! Have " + totalBytes + " bytes!");
            }
        }
        /// <summary>
        /// Constructor for first or middle tal, not last tal. 
        /// </summary>
        /// <param name="firstTAL"></param>
        /// <param name="onsetSign"></param>
        /// <param name="onset"></param>
        /// <param name="duration"></param>
        /// <param name="annotations"></param>
        /// <param name="totalBytes"></param>
        public EDFTALAnnotation(bool firstTAL, char onsetSign, double onset, double duration, string[] annotations)
        {
            builtAnnotation = new List<byte>();
            this.onsetSign = onsetSign;
            this.onset = onset;
            this.duration = duration;
            this.annotations = annotations;



            builtAnnotation.Add((Byte)onsetSign); //write onset sign (+/-)

            builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(Convert.ToString(onset))); // convert onset to bytes and add to the byte array
            if (firstTAL) //add two 20s to the tal after onset if so
            {
                builtAnnotation.Add(beforeAnnotation);
                builtAnnotation.Add(beforeAnnotation);
            }
            else
            {
                if (duration != 0)
                {
                    builtAnnotation.Add(beforeDuration);
                    builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(Convert.ToString(duration))); //  convert duration to bytes and add to the byte array
                    builtAnnotation.Add(beforeAnnotation);
                }
                else
                {
                    builtAnnotation.Add(beforeAnnotation);
                }
            }


            for (int i = 0; i < annotations.Length; i++)
            {
                builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(annotations[i]));
                builtAnnotation.Add(afterAnnotation);
            }



            if (totalBytes > 0)
            {

                int remainingBytes = totalBytes - builtAnnotation.Count;
                //Console.WriteLine("total: {0}, remaining: {1}", totalBytes, remainingBytes);

                for (int i = 0; i < remainingBytes; i++)
                {
                    builtAnnotation.Add(endOfTAL);
                }
            }
            else
            {
                throw new ArgumentException("Not enough bytes remaining to complete annotation! Have " + totalBytes + " bytes!");
            }
        }

        /// <summary>
        /// Constructor for LAST TAL of a NORMAL TAL RECORD -> aka has a first tal already defined / this is not the only TAL in the record
        /// </summary>
        /// <param name="firstTAL"></param>
        /// <param name="onsetSign"></param>
        /// <param name="onset"></param>
        /// <param name="duration"></param>
        /// <param name="annotations"></param>
        public EDFTALAnnotation(char onsetSign, double onset, double duration, string[] annotations, int remainingBytes)
        {
            builtAnnotation = new List<byte>();
            this.onsetSign = onsetSign;
            this.onset = onset;
            this.duration = duration;
            this.annotations = annotations;
            this.remainingBytes = remainingBytes;
            



            builtAnnotation.Add((Byte)onsetSign); //write onset sign (+/-)

            builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(Convert.ToString(onset))); // convert onset to bytes and add to the byte array

            builtAnnotation.Add(beforeDuration);
            builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(Convert.ToString(duration))); //  convert duration to bytes and add to the byte array
            builtAnnotation.Add(beforeAnnotation);


            for (int i = 0; i < annotations.Length; i++)
            {
                builtAnnotation.AddRange(new System.Text.ASCIIEncoding().GetBytes(annotations[i]));
                builtAnnotation.Add(afterAnnotation);
            }

            for (int i = 0; i < remainingBytes; i++)
            {
                builtAnnotation.Add(endOfTAL);
            }
        
            }

    }
}
