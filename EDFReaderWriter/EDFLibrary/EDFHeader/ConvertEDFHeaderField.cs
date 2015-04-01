using System;

namespace EDFLibrary.EDFHeader
{
    /// <summary>
    /// Gets the pure integer out from the specified header field by stripping all leading/trailing whitespace and doing a normal conversion
    /// </summary>
    public static class ConvertEDFHeaderField
    {
        public static int toInt32(string field)
        {
            try
            {
                return Convert.ToInt32(field.Trim());
            }
            catch (FormatException ex)
            {
                throw new FormatException("Conversion of header field to int failed! Recieved: " + field,ex);
            }
            
        }
    }
}
