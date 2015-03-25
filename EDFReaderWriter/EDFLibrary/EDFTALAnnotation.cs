using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFReaderWriter.EDFLibrary
{
    class EDFTALAnnotation
    {
        private double onset { get; set; }
        private double duration { get; set; }

        public EDFTALAnnotation(double onset, double duration)
        {
            this.onset = onset;
            this.duration = duration;
        }

    }
}
