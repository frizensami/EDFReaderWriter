
namespace EDFLibrary.EDFHeader
{
    /// <summary>
    /// This class defines three constants, EDF+C and EDF+D as the possible formats for an EDF+ file, EDF_NOTPLUS as the mark of a non EDF plus file 
    /// EDF+C -> EDF Continuous, EDF+D is EDF Discontinuous. Continuous is supported by EDF, Discontinuous is not.
    /// </summary>
    class EDFReserved
    {
        
        public const string EDF_CONTINUOUS = "EDF+C";
        public const string EDF_DISCONTINUOUS = "EDF+D";
    }
}
