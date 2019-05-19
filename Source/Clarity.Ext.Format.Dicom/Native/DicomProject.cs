using System.Runtime.InteropServices;

namespace Clarity.Ext.Format.Dicom.Native
{
    public static unsafe class DicomProject
    {
        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerSingleDicomFile(byte* FileName, void** DicomManager);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerMultiDicomFiles(byte* Folder, void** DicomManager);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetTagListLength(void* DicomManager, int SliceNumber, int* length);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetTagList(void* DicomManager, int SliceNumber, TagInfoStruct** TagList);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetTagValueLong(void* DicomManager, int SliceNumber, ushort GroupID, ushort TagID, long* TagValue, int Index);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetNumberOfSlices(void* DicomManager, int* NumberOfSlices);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetDimensions(void* DicomManager, int SliceNumber, int* Width, int* Height, int* Depth);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetPixelDataBuffer(void* DicomManager, int SliceNumber, byte* buffer, int* bufferSize, int* WrittenData);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetNormalizedPixelDataBuffer(void* DicomManager, int SliceNumber, double* buffer, int* bufferSize, int* WrittenData);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetMonochromePixelDataBufferOfSlice(void* DicomManager, int SliceNumber, byte* buffer, int* bufferSize, int* WrittenData);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerGetMonochromePixelDataBufferOfSliceNormalized(void* DicomManager, int SliceNumber, double* buffer, int* bufferSize, int* WrittenData);

        [DllImport("Dicom_project.dll")]
        public static extern DicomResult AnalyzerKillDicomAnalyzer(void** DicomManager);
    }
}