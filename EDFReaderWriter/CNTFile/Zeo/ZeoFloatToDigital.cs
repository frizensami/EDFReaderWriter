using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNTFile.Zeo
{
    public static class ZeoFloatToDigital
    {
        public static Int16 zeoFloatToDigital(float floatValue)
        {
            return ((Int16)((floatValue * 32768) / 318)); //conversion  
        }
    }
}
