using System;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Colors
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public struct Color3 : IEquatable<Color3>
    {
        public Vector3 Raw;

        #region Constructors
        /// <summary>
        /// Creates a <see cref="Color3"/> using float parameters in range (0.0f - 1.0f).
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        public Color3(float red, float green, float blue)
        {
            Raw.X = red;
            Raw.Y = green;
            Raw.Z = blue;
        }

        /// <summary>
        /// Creates a <see cref="Color3"/> using int parameters in range (0 - 255).
        /// </summary>
        /// <param name="red">Red component</param>
        /// <param name="green">Green component</param>
        /// <param name="blue">Blue component</param>
        public Color3(int red, int green, int blue)
        {
            Raw.X = red / 255.0f;
            Raw.Y = green / 255.0f;
            Raw.Z = blue / 255.0f;
        }

        /// <summary>
        /// Creates a <see cref="Color3"/> from a raw vector.
        /// </summary>
        /// <param name="raw">Raw vector</param>
        public Color3(Vector3 raw)
        {
            Raw = raw;
        }
        #endregion

        public float R { get { return Raw.X; } set { Raw.X = value; } }
        public float G { get { return Raw.Y; } set { Raw.Y = value; } }
        public float B { get { return Raw.Z; } set { Raw.Z = value; } }

        public float this[int index]
        {
            get { return Raw[index]; }
            set { Raw[index] = value; }
        }

        #region Equality, Hash, String
        public override int GetHashCode() { return Raw.GetHashCode(); }
        public override string ToString() { return Raw.ToString(); }
        public static bool Equals(Color3 s1, Color3 s2) { return s1.Raw == s2.Raw; }
        public static bool operator ==(Color3 s1, Color3 s2) { return Equals(s1, s2); }
        public static bool operator !=(Color3 s1, Color3 s2) { return !Equals(s1, s2); }
        public bool Equals(Color3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Color3 && Equals((Color3)obj); }
        #endregion

        #region Convertions
        public Vector3 ToVector3() { return Raw; }
        public static explicit operator Vector3(Color3 c) { return c.ToVector3(); }
        public static explicit operator Color3(Vector3 v) { return new Color3(v); }

        public Color4 ToColor4() { return new Color4(R, G, B); }

        public int ToArgb() { return ToColor4().ToArgb(); }

        public Color3 SrgbToLinear() => new Color3(
            GraphicsHelper.SrgbToLinear(R), 
            GraphicsHelper.SrgbToLinear(G),
            GraphicsHelper.SrgbToLinear(B));

        public Color3 LinearToSrgb() => new Color3(
            GraphicsHelper.LinearToSrgb(R), 
            GraphicsHelper.LinearToSrgb(G),
            GraphicsHelper.LinearToSrgb(B));
        #endregion

        #region Math
        public static Color3 operator *(float scale, Color3 c) { return c.ScaleBy(scale); }
        public static Color3 operator *(Color3 c, float scale) { return c.ScaleBy(scale); }
        public Color3 ScaleBy(float scale)
        {
            return (Color3)Raw.ScaleBy(scale);
        }
        
        public static Color3 Lerp(Color3 first, Color3 second, float amount)
        {
            return (Color3)Vector3.Lerp(first.Raw, second.Raw, amount);
        }
        #endregion

        #region Constant Colors
        public static Color3 Black { get { return new Color3(0, 0, 0); } }
        public static Color3 White { get { return new Color3(255, 255, 255); } }
        public static Color3 Red { get { return new Color3(255, 0, 0); } }
        public static Color3 Green { get { return new Color3(0, 255, 0); } }
        public static Color3 Blue { get { return new Color3(0, 0, 255); } }
        public static Color3 Yellow { get { return new Color3(255, 255, 0); } }
        public static Color3 Magneta { get { return new Color3(255, 0, 255); } }
        public static Color3 Cyan { get { return new Color3(0, 255, 255); } }

        public static Color3 CornflowerBlue { get { return new Color3(100, 149, 237); } }
        public static Color3 Violet { get { return new Color3(143, 0, 255); } }
        public static Color3 Orange { get { return new Color3(255, 127, 0); } }
        #endregion
    }
}
