using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDFLibrary
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
            outString += content + generateSpaces(length - content.Length);
            return outString;
        }
    }
}
