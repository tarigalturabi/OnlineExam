namespace KFU.Common
{
    public class ApplicationStaticContent
    { 
        public string UploadFilePath { get; set; } = Constants.NullString;
        public string CSSPrintPath { get; set; } = Constants.NullString;
        public string HeaderImagePath { get; set; } = Constants.NullString;
        public string PathKeys { get; set; } = Constants.NullString;
        public string LogPath { get; set; } = Constants.NullString;
        public SortedDictionary<string, SortedDictionary<string, string[]>> StaticMenu { get; set; }
    }
}
