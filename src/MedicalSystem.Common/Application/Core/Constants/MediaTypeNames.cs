namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// File MIME types
/// </summary>
public static class MediaTypeNames
{
    /// <summary>
    /// MIME types for image files
    /// </summary>
    public static class Image
    {
        /// <summary>JPEG image</summary>
        public const string Jpeg = "image/jpeg";

        /// <summary>PNG image</summary>
        public const string Png = "image/png";

        /// <summary>SVG image</summary>
        public const string Svg = "image/svg+xml";
    }

    /// <summary>
    /// MIME types for archive files
    /// </summary>
    public static class Archive
    {
        /// <summary>ZIP archive</summary>
        public const string Zip = "application/zip";

        /// <summary>RAR archive</summary>
        public const string Rar = "application/vnd.rar";

        /// <summary>7-Zip archive</summary>
        public const string SevenZip = "application/x-7z-compressed";

        /// <summary>TAR archive</summary>
        public const string Tar = "application/x-tar";
    }

    /// <summary>
    /// MIME types for data files
    /// </summary>
    public static class Data
    {
        /// <summary>JSON data</summary>
        public const string Json = "application/json";

        /// <summary>XML data</summary>
        public const string Xml = "application/xml";

        /// <summary>CSV data</summary>
        public const string Csv = "text/csv";
    }

    /// <summary>
    /// MIME types for document files
    /// </summary>
    public static class Document
    {
        /// <summary>PDF document</summary>
        public const string Pdf = "application/pdf";

        /// <summary>MS Word document</summary>
        public const string MsWord = "application/msword";

        /// <summary>MS Word (OpenXML) document</summary>
        public const string MsWordX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    }

    /// <summary>
    /// MIME types for spreadsheet files
    /// </summary>
    public static class Spreadsheet
    {
        /// <summary>MS Excel spreadsheet</summary>
        public const string MsExcel = "application/vnd.ms-excel";

        /// <summary>MS Excel (OpenXML) spreadsheet</summary>
        public const string MsExcelX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }

    /// <summary>
    /// MIME types for slide presentation files
    /// </summary>
    public static class Slide
    {
        /// <summary>MS PowerPoint presentation</summary>
        public const string MsPowerpoint = "application/vnd.ms-powerpoint";

        /// <summary>MS PowerPoint (OpenXML) presentation</summary>
        public const string MsPowerpointX = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
    }

    /// <summary>
    /// MIME types for audio files
    /// </summary>
    public static class Audio
    {
        /// <summary>MP3 audio</summary>
        public const string Mp3 = "audio/mpeg";

        /// <summary>OGG audio</summary>
        public const string Ogg = "audio/ogg";

        /// <summary>WAV audio</summary>
        public const string Wav = "audio/wav";
    }

    /// <summary>
    /// MIME types for video files
    /// </summary>
    public static class Video
    {
        /// <summary>MP4 video</summary>
        public const string Mp4 = "video/mp4";

        /// <summary>MPEG video</summary>
        public const string Mpeg = "video/mpeg";

        /// <summary>AVI video</summary>
        public const string Avi = "video/x-msvideo";

        /// <summary>OGG video</summary>
        public const string Ogg = "video/ogg";
    }
}
