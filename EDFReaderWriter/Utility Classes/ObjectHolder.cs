using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDFLibrary.EDFHeader;
using EDFLibrary;
namespace EDFReaderWriter.Utility_Classes
{
    public static class ObjectHolder
    {
        public static EDFHeader EDFHeaderHolder;
        public static List<EDFSignal> signals = new List<EDFSignal>();
    }
}
