using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary
{
    class EDFHeader
    {
        string version;
        string localPatientIdent;
        string localRecordingIdent;
        string startdate;
        

        public EDFHeader()
        {
            version = BuildHeader.buildHeader("0", 8);
            Console.WriteLine(version);
        }
    }
}
