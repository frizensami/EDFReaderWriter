using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary.EDFHeader
{
       
    public static class BuildHeader
    {

        private static string generateSpaces(int length)
        {
            string outSpaces = "";
            for (int i = 0; i < length; i++)
                outSpaces += " ";

            return outSpaces;
       } 

        public static string buildHeader(string content, int length)
        {
            string outString = "";

            if (content.Length <= length)
                outString += content + generateSpaces(length - content.Length); //generate whatever num of spaces to fill in the blanks
            else
                throw new ApplicationException("Header exceeded allocated size!");


            return outString;
        }
    }
}
