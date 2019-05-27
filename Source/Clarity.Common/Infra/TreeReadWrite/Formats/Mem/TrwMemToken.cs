namespace Clarity.Common.Infra.TreeReadWrite.Formats.Mem
{
    public struct TrwMemToken
    {
        public TrwTokenType Type;
        public double Num;
        public string Str;

        public TrwMemToken(TrwTokenType type, double num, string str)
        {
            Type = type;
            Num = num;
            Str = str;
        }

        public bool ValueAsBool => Num != 0;
        public int ValueAsSInt32 => (int)Num;
        public double ValueAsFloat64 => Num;
        public string ValueAsString => Str;

        public static TrwMemToken StartObject() => new TrwMemToken(TrwTokenType.StartObject, 0, null);
        public static TrwMemToken EndObject() => new TrwMemToken(TrwTokenType.EndObject, 0, null);
        public static TrwMemToken StartArray() => new TrwMemToken(TrwTokenType.StartArray, 0, null);
        public static TrwMemToken EndArray() => new TrwMemToken(TrwTokenType.EndArray, 0, null);
        public static TrwMemToken PropertyName(string name) => new TrwMemToken(TrwTokenType.PropertyName, 0, name);
        public static TrwMemToken Null() => new TrwMemToken(TrwTokenType.Null, 0, null);
        public static TrwMemToken Boolean(bool val) => new TrwMemToken(TrwTokenType.Boolean, val ? 1 : 0, null);
        public static TrwMemToken Integer(int val) => new TrwMemToken(TrwTokenType.Integer, val, null);
        public static TrwMemToken Float(double val) => new TrwMemToken(TrwTokenType.Float, val, null);
        public static TrwMemToken String(string str) => new TrwMemToken(TrwTokenType.String, 0, str);
    }
}