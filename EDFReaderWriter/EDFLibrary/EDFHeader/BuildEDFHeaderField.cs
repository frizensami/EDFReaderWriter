using System;

namespace EDFLibrary.EDFHeader
{
       
    public static class BuildEDFHeaderField
    {
        /// <summary>
        /// Generates a string with a specificied number of spaces
        /// </summary>
        /// <param name="length">The number of spaces to create</param>
        /// <returns></returns>
        private static string generateSpaces(int length)
        {
            string outSpaces = "";
            for (int i = 0; i < length; i++)
                outSpaces += " ";
            
            return outSpaces;
       } 

        /// <summary>
        /// Uses the internal function generateSpaces() to add a string of spaces to the incoming header field
        /// </summary>
        /// <param name="content">The raw header field without spaces</param>
        /// <param name="length">The total number of bytes/chars allocated to this field</param>
        /// <returns></returns>
        public static string buildEDFHeaderField(string content, int length) 
        {
            string outString = "";

            if (content.Length <= length)
                outString += content + generateSpaces(length - content.Length); //generate whatever num of spaces to fill in the blanks
            else
                throw new ArgumentException("Header exceeded allocated size!");


            return outString;
        }
    }
}
