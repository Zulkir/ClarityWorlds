namespace Clarity.Ext.Format.Dicom.Native
{
    public struct TagInfoStruct
    {
        public ushort GroupId;
        public ushort Id;

        public TagInfoStruct(ushort groupId, ushort id)
        {
            GroupId = (ushort)groupId;
            Id = (ushort)id;
        }

        public static TagInfoStruct PatientsName { get; } = new TagInfoStruct(0x10, 0x10);
        public static TagInfoStruct Rows { get; } = new TagInfoStruct(0x28, 0x10);
    }
}