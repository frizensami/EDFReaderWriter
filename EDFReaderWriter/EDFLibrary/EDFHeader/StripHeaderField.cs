using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFHeader
{
    //TODO change this to a toInt overriden method!
    /// <summary>
    /// Gets the pure integer out from the specified header field
    /// </summary>
    public static class StripHeaderField
    {
        public static int stripHeaderField(string field)
        {
            return Convert.ToInt32(field.Trim());
        }
    }
}
