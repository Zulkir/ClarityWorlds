namespace Clarity.Ext.Format.Dicom.Native
{
    public enum DicomResult
    {
        SUCCESS = 0,
        ALLOCERROR = 1,
        UNKNOWNERROR = 2,
        ILLEGALCONVERSION = 3,
        FILENOTLOADED = 4,
        NULLMANAGER = 5,
        INDEXOUTOFBOUND = 6,
        FILENOTFOUND = 7,
        TAGDOESNOTEXIST = 8,
        SLICENOTFOUND = 9,
        INVALIDARGUMENT = 10,
        PIXELLOCNOTAVAILBLE = 11,
        IMAGEISNOTMONOCHROME = 12,
        ERRORCREATINGBMP = 13,
        FOLDERNOTFOUND = 14
    }
}