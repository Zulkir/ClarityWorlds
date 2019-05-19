using System;
using System.Globalization;

namespace Clarity.Engine.Visualization.Viewports
{
    public struct ViewportLength : IEquatable<ViewportLength>
    {
        public float Value;
        public ViewportLengthUnit Unit;

        public ViewportLength(float value, ViewportLengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public bool Equals(ViewportLength other) => Value.Equals(other.Value) && Unit == other.Unit;
        public override bool Equals(object obj) => obj is ViewportLength && Equals((ViewportLength)obj);
        public override int GetHashCode() => (Value.GetHashCode() * 397) ^ (int) Unit;

        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture) + UnitToString(Unit);

        private string UnitToString(ViewportLengthUnit unit)
        {
            switch (unit)
            {
                case ViewportLengthUnit.Pixels: return "px";
                case ViewportLengthUnit.ScaledPixels: return "spx";
                case ViewportLengthUnit.Percent: return "%";
                default: throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }
    }
}