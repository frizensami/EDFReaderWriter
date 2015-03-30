using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFData.Types
{
    class EDFTALAnnotation
    {
        private char onsetSign;
        private double onset { get; set; }
        private double duration { get; set; }
        private const byte beforeDuration = 21;
        private const byte endOfTAL = 20;
        private const byte endOfLastTAL = 0;
        public EDFTALAnnotation(char onsetSign, double onset, double duration)
        {
            this.onsetSign = onsetSign;
            this.onset = onset;
            this.duration = duration;
        }

    }
}
