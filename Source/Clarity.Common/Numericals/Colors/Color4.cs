using System;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Colors
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct Color4 : IEquatable<Color4>
    {
        public Vector4 Raw;

        #region Constructors
        /// <summary>
        /// Creates a <see cref="Color4"/> using float parameters in range (0.0f - 1.0f).
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        /// <param name="alpha">Alpha component</param>
        public Color4(float red, float green, float blue, float alpha = 1.0f)
        {
            Raw.X = red;
            Raw.Y = green;
            Raw.Z = blue;
            Raw.W = alpha;
        }

        /// <summary>
        /// Creates a <see cref="Color4"/> using int parameters in range (0 - 255).
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        /// <param name="alpha">Alpha component</param>
        public Color4(int red, int green, int blue, int alpha = 255)
        {
            Raw.X = red / 255.0f;
            Raw.Y = green / 255.0f;
            Raw.Z = blue / 255.0f;
            Raw.W = alpha / 255.0f;
        }

        /// <summary>
        /// Creates a <see cref="Color4"/> using <see cref="Color3"/> and float alpha in range (0.0f - 1.0f).
        /// </summary>
        /// <param name="color3">RGB components</param>
        /// <param name="alpha">Alpha component</param>
        public Color4(Color3 color3, float alpha)
        {
            Raw = new Vector4(color3.ToVector3(), alpha);
        }

        /// <summary>
        /// Creates a <see cref="Color4"/> using <see cref="Color3"/> and float alpha in range (0 - 255).
        /// </summary>
        /// <param name="color3">RGB components</param>
        /// <param name="alpha">Alpha component</param>
        public Color4(Color3 color3, int alpha)
        {
            Raw = new Vector4(color3.ToVector3(), alpha / 255.0f);
        }

        /// <summary>
        /// Creates a <see cref="Color4"/> using <see cref="int"/> ARGB value.
        /// </summary>
        /// <param name="argb">ARGB value</param>
        public Color4(int argb)
        {
            var a = ((argb >> 24) & 0xff) / 255f;
            var r = ((argb >> 16) & 0xff) / 255f;
            var g = ((argb >> 8) & 0xff) / 255f;
            var b = (argb & 0xff) / 255f;
            Raw = new Vector4(r, g, b, a);
        }

        /// <summary>
        /// Creates a <see cref="Color4"/> from a raw vector.
        /// </summary>
        /// <param name="raw">Raw vector</param>
        public Color4(Vector4 raw)
        {
            Raw = raw;
        }
        #endregion

        public float R { get { return Raw.X; } set { Raw.X = value; } }
        public float G { get { return Raw.Y; } set { Raw.Y = value; } }
        public float B { get { return Raw.Z; } set { Raw.Z = value; } }
        public float A { get { return Raw.W; } set { Raw.W = value; } }

        public float this[int index]
        {
            get { return Raw[index]; }
            set { Raw[index] = value; }
        }

        public Color3 RGB
        {
            get { return new Color3(R, G, B); }
            set { Raw.Xyz = value.ToVector3(); }
        }

        #region Equality, Hash, String
        public override int GetHashCode() { return Raw.GetHashCode(); }
        public override string ToString() { return Raw.ToString(); }
        public static bool Equals(Color4 s1, Color4 s2) { return s1.Raw == s2.Raw; }
        public static bool operator ==(Color4 s1, Color4 s2) { return Equals(s1, s2); }
        public static bool operator !=(Color4 s1, Color4 s2) { return !Equals(s1, s2); }
        public bool Equals(Color4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Color4 && Equals((Color4)obj); }
        #endregion

        #region Convertions
        public Color4 FromSrgb() { return new Color4(FromSrgb(R), FromSrgb(G), FromSrgb(B), A); }

        private static float FromSrgb(float c)
        {
            return c <= 0.04045f
                ? c / 12.92f
                : MathHelper.Pow((c + 0.055f) / 1.055f, 2.4f);
        }

        public Vector4 ToVector4() { return Raw; }
        public static explicit operator Vector4(Color4 c) { return c.Raw; }
        public static explicit operator Color4(Vector4 v) { return new Color4(v); }

        public int ToArgb()
        {
            return 
                ((int)(A * 255.99f) << 24) |
                ((int)(R * 255.99f) << 16) |
                ((int)(G * 255.99f) << 8) |
                (int)(B * 255.99f);
        }
        #endregion

        #region Math
        public static Color4 operator *(float scale, Color4 c) { return c.ScaleBy(scale); }
        public static Color4 operator *(Color4 c, float scale) { return c.ScaleBy(scale); }
        public Color4 ScaleBy(float scale)
        {
            return (Color4)Raw.ScaleBy(scale);
        }
        
        public static Color4 Lerp(Color4 first, Color4 second, float amount)
        {
            return (Color4)Vector4.Lerp(first.Raw, second.Raw, amount);
        }
        #endregion

        #region Constant Colors
        public static Color4 Transparent { get { return new Color4(); } }

        public static Color4 Black { get { return Color3.Black.ToColor4(); } }
        public static Color4 White { get { return Color3.White.ToColor4(); } }
        public static Color4 Red { get { return Color3.Red.ToColor4(); } }
        public static Color4 Green { get { return Color3.Green.ToColor4(); } }
        public static Color4 Blue { get { return Color3.Blue.ToColor4(); } }
        public static Color4 Yellow { get { return Color3.Yellow.ToColor4(); } }
        public static Color4 Magenta { get { return Color3.Magneta.ToColor4(); } }
        public static Color4 Cyan { get { return Color3.Cyan.ToColor4(); } }

        public static Color4 CornflowerBlue { get { return Color3.CornflowerBlue.ToColor4(); } }
        public static Color4 Violet { get { return Color3.Violet.ToColor4(); } }
        public static Color4 Orange { get { return Color3.Orange.ToColor4(); } }
        #endregion
    }
}
