using EDFLibrary.EDFData.Types.TAL;
using System;

namespace EDFReaderWriter.EDFLibrary.EDFData.Generator
{
    class DefaultAnnotationGenerator
    {
        public Byte[][] annotationSignals;
        public DefaultAnnotationGenerator(int numRecords, int recordDuration, int numAnnotationBytes)
        {
            annotationSignals = new Byte[numRecords][];
            for (int i = 0; i < numRecords; i++)
            {
                bool firstTal = true;
                char onsetSign = '+';
                double onset = recordDuration * (i);
                double duration = 0;
                string[] annotations = new string[1]{"Start of Data Record"};
                EDFTALAnnotation annotation = new EDFTALAnnotation(firstTal, onsetSign,onset,duration,annotations,numAnnotationBytes);
                annotationSignals[i] = annotation.builtAnnotation.ToArray();
            }


        }
    }
}
