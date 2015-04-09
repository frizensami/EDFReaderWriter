using EDFLibrary.EDFData.Types.TAL;
using System;

namespace EDFLibrary.EDFLibrary.EDFData.Generator
{
    /// <summary>
    /// In case of no annotations, to make the file EDF+ compliant, we need to generate one annotation per data record with the timestamp at the start of the record.
    /// </summary>
    class DefaultAnnotationGenerator
    {
        private Byte[][] annotationSignals;

        public DefaultAnnotationGenerator(int numRecords, int recordDuration, int numAnnotationBytes)
        {
            annotationSignals = new Byte[numRecords][];
            for (int i = 0; i < numRecords; i++)
            {
                //define basic parameters for the default annotation
                char onsetSign = '+';
                double onset = recordDuration * i;
                string[] annotations = new string[1]{"Start of Data Record"}; //use this as default annotation for every record

                //create the actual annotation
                EDFTALAnnotation annotation = new EDFTALAnnotation(onsetSign,onset, annotations,numAnnotationBytes);
                 

                annotationSignals[i] = annotation.builtAnnotation.ToArray();
            }

        }

        public Byte[][] getAnnotationSignals()
        {
            return annotationSignals;
        }
    }
}
