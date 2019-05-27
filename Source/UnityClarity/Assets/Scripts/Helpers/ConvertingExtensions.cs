using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Visualization.Elements.RenderStates;
using u = UnityEngine;
using ur = UnityEngine.Rendering;

namespace Assets.Scripts.Helpers
{
    public static class ConvertingExtensions
    {
        #region Vector-like types
        public static Vector2 ToClarity(this u.Vector2 uv) =>
            new Vector2(uv.x, uv.y);

        public static Vector3 ToClarity(this u.Vector3 uv, bool invertZ) =>
            new Vector3(uv.x, uv.y, invertZ ? -uv.z : uv.z);

        public static Vector4 ToClarity(this u.Vector4 uv, bool invertZ) =>
            new Vector4(uv.x, uv.y, invertZ ? -uv.z : uv.z, uv.w);

        public static Color4 ToClarity(this u.Color uc) =>
            new Color4(uc.r, uc.g, uc.b, uc.a);

        public static u.Vector2 ToUnity(this Vector2 cv, bool oneMinusV) =>
            new u.Vector2(cv.X, oneMinusV ? 1f - cv.Y : cv.Y);

        public static u.Vector3 ToUnity(this Vector3 cv, bool invertZ) =>
            new u.Vector3(cv.X, cv.Y, invertZ ? -cv.Z : cv.Z);

        public static u.Vector4 ToUnity4(this Vector3 cv, bool invertZ) =>
            new u.Vector4(cv.X, cv.Y, invertZ ? -cv.Z : cv.Z, 1);

        public static u.Vector4 ToUnity(this Vector4 cv, bool invertZ) =>
            new u.Vector4(cv.X, cv.Y, invertZ ? -cv.Z : cv.Z, cv.W);

        public static u.Quaternion ToUnity(this Quaternion cq, bool invertZ) =>
            invertZ
                ? new u.Quaternion(cq.X, cq.Y, -cq.Z, cq.W)
                : new u.Quaternion(cq.X, cq.Y, -cq.Z, cq.W);

        public static u.Color ToUnity(this Color4 cc) =>
            new u.Color(cc.R, cc.G, cc.B, cc.A);
        #endregion

        #region Enums

        public static ur.CullMode ToUnity(this CullFace cCullFace)
        {
            switch (cCullFace)
            {
                case CullFace.None: return ur.CullMode.Off;
                case CullFace.Back: return ur.CullMode.Back;
                case CullFace.Front: return ur.CullMode.Front;
                default: throw new ArgumentOutOfRangeException(nameof(cCullFace), cCullFace, null);
            }
        }
        #endregion

        #region Keys
        public static Key ToClarity(this u.KeyCode ukc)
        {
            switch (ukc)
            {
                case u.KeyCode.None: return Key.None;
                case u.KeyCode.A: return Key.A;
                case u.KeyCode.B: return Key.B;
                case u.KeyCode.C: return Key.C;
                case u.KeyCode.D: return Key.D;
                case u.KeyCode.E: return Key.E;
                case u.KeyCode.F: return Key.F;
                case u.KeyCode.G: return Key.G;
                case u.KeyCode.H: return Key.H;
                case u.KeyCode.I: return Key.I;
                case u.KeyCode.J: return Key.J;
                case u.KeyCode.K: return Key.K;
                case u.KeyCode.L: return Key.L;
                case u.KeyCode.M: return Key.M;
                case u.KeyCode.N: return Key.N;
                case u.KeyCode.O: return Key.O;
                case u.KeyCode.P: return Key.P;
                case u.KeyCode.Q: return Key.Q;
                case u.KeyCode.R: return Key.R;
                case u.KeyCode.S: return Key.S;
                case u.KeyCode.T: return Key.T;
                case u.KeyCode.U: return Key.U;
                case u.KeyCode.V: return Key.V;
                case u.KeyCode.W: return Key.W;
                case u.KeyCode.X: return Key.X;
                case u.KeyCode.Y: return Key.Y;
                case u.KeyCode.Z: return Key.Z;
                case u.KeyCode.F1: return Key.F1;
                case u.KeyCode.F2: return Key.F2;
                case u.KeyCode.F3: return Key.F3;
                case u.KeyCode.F4: return Key.F4;
                case u.KeyCode.F5: return Key.F5;
                case u.KeyCode.F6: return Key.F6;
                case u.KeyCode.F7: return Key.F7;
                case u.KeyCode.F8: return Key.F8;
                case u.KeyCode.F9: return Key.F9;
                case u.KeyCode.F10: return Key.F10;
                case u.KeyCode.F11: return Key.F11;
                case u.KeyCode.F12: return Key.F12;
                case u.KeyCode.Alpha0: return Key.D0;
                case u.KeyCode.Alpha1: return Key.D1;
                case u.KeyCode.Alpha2: return Key.D2;
                case u.KeyCode.Alpha3: return Key.D3;
                case u.KeyCode.Alpha4: return Key.D4;
                case u.KeyCode.Alpha5: return Key.D5;
                case u.KeyCode.Alpha6: return Key.D6;
                case u.KeyCode.Alpha7: return Key.D7;
                case u.KeyCode.Alpha8: return Key.D8;
                case u.KeyCode.Alpha9: return Key.D9;
                case u.KeyCode.Minus: return Key.Minus;
                case u.KeyCode.Plus: return Key.Plus;
                case u.KeyCode.BackQuote: return Key.Grave;
                case u.KeyCode.Insert: return Key.Insert;
                case u.KeyCode.Home: return Key.Home;
                case u.KeyCode.PageUp: return Key.PageUp;
                case u.KeyCode.PageDown: return Key.PageDown;
                case u.KeyCode.Delete: return Key.Delete;
                case u.KeyCode.End: return Key.End;
                case u.KeyCode.KeypadDivide: return Key.Divide;
                case u.KeyCode.KeypadPeriod: return Key.Decimal;
                case u.KeyCode.Backspace: return Key.Backspace;
                case u.KeyCode.UpArrow: return Key.Up;
                case u.KeyCode.DownArrow: return Key.Down;
                case u.KeyCode.LeftArrow: return Key.Left;
                case u.KeyCode.RightArrow: return Key.Right;
                case u.KeyCode.Tab: return Key.Tab;
                case u.KeyCode.Space: return Key.Space;
                case u.KeyCode.CapsLock: return Key.CapsLock;
                case u.KeyCode.ScrollLock: return Key.ScrollLock;
                case u.KeyCode.Print: return Key.PrintScreen;
                case u.KeyCode.Numlock: return Key.NumberLock;
                case u.KeyCode.Return: return Key.Enter;
                case u.KeyCode.Escape: return Key.Escape;
                case u.KeyCode.Menu: return Key.Menu;
                case u.KeyCode.Backslash: return Key.Backslash;
                case u.KeyCode.Equals: return Key.Equal;
                case u.KeyCode.Semicolon: return Key.Semicolon;
                case u.KeyCode.Quote: return Key.Quote;
                case u.KeyCode.Comma: return Key.Comma;
                case u.KeyCode.Period: return Key.Period;
                case u.KeyCode.Slash: return Key.ForwardSlash;
                case u.KeyCode.RightBracket: return Key.RightBracket;
                case u.KeyCode.LeftBracket: return Key.LeftBracket;
            }
            return Key.None;
        }

        public static u.KeyCode ToUnity(this Key ckc)
        {
            switch (ckc)
            {
                case Key.None: return u.KeyCode.None;
                case Key.A: return u.KeyCode.A;
                case Key.B: return u.KeyCode.B;
                case Key.C: return u.KeyCode.C;
                case Key.D: return u.KeyCode.D;
                case Key.E: return u.KeyCode.E;
                case Key.F: return u.KeyCode.F;
                case Key.G: return u.KeyCode.G;
                case Key.H: return u.KeyCode.H;
                case Key.I: return u.KeyCode.I;
                case Key.J: return u.KeyCode.J;
                case Key.K: return u.KeyCode.K;
                case Key.L: return u.KeyCode.L;
                case Key.M: return u.KeyCode.M;
                case Key.N: return u.KeyCode.N;
                case Key.O: return u.KeyCode.O;
                case Key.P: return u.KeyCode.P;
                case Key.Q: return u.KeyCode.Q;
                case Key.R: return u.KeyCode.R;
                case Key.S: return u.KeyCode.S;
                case Key.T: return u.KeyCode.T;
                case Key.U: return u.KeyCode.U;
                case Key.V: return u.KeyCode.V;
                case Key.W: return u.KeyCode.W;
                case Key.X: return u.KeyCode.X;
                case Key.Y: return u.KeyCode.Y;
                case Key.Z: return u.KeyCode.Z;
                case Key.F1: return u.KeyCode.F1;
                case Key.F2: return u.KeyCode.F2;
                case Key.F3: return u.KeyCode.F3;
                case Key.F4: return u.KeyCode.F4;
                case Key.F5: return u.KeyCode.F5;
                case Key.F6: return u.KeyCode.F6;
                case Key.F7: return u.KeyCode.F7;
                case Key.F8: return u.KeyCode.F8;
                case Key.F9: return u.KeyCode.F9;
                case Key.F10: return u.KeyCode.F10;
                case Key.F11: return u.KeyCode.F11;
                case Key.F12: return u.KeyCode.F12;
                case Key.D0: return u.KeyCode.Alpha0;
                case Key.D1: return u.KeyCode.Alpha1;
                case Key.D2: return u.KeyCode.Alpha2;
                case Key.D3: return u.KeyCode.Alpha3;
                case Key.D4: return u.KeyCode.Alpha4;
                case Key.D5: return u.KeyCode.Alpha5;
                case Key.D6: return u.KeyCode.Alpha6;
                case Key.D7: return u.KeyCode.Alpha7;
                case Key.D8: return u.KeyCode.Alpha8;
                case Key.D9: return u.KeyCode.Alpha9;
                case Key.Minus: return u.KeyCode.Minus;
                case Key.Plus: return u.KeyCode.Plus;
                case Key.Grave: return u.KeyCode.BackQuote;
                case Key.Insert: return u.KeyCode.Insert;
                case Key.Home: return u.KeyCode.Home;
                case Key.PageUp: return u.KeyCode.PageUp;
                case Key.PageDown: return u.KeyCode.PageDown;
                case Key.Delete: return u.KeyCode.Delete;
                case Key.End: return u.KeyCode.End;
                case Key.Divide: return u.KeyCode.KeypadDivide;
                case Key.Decimal: return u.KeyCode.KeypadPeriod;
                case Key.Backspace: return u.KeyCode.Backspace;
                case Key.Up: return u.KeyCode.UpArrow;
                case Key.Down: return u.KeyCode.DownArrow;
                case Key.Left: return u.KeyCode.LeftArrow;
                case Key.Right: return u.KeyCode.RightArrow;
                case Key.Tab: return u.KeyCode.Tab;
                case Key.Space: return u.KeyCode.Space;
                case Key.CapsLock: return u.KeyCode.CapsLock;
                case Key.ScrollLock: return u.KeyCode.ScrollLock;
                case Key.PrintScreen: return u.KeyCode.Print;
                case Key.NumberLock: return u.KeyCode.Numlock;
                case Key.Enter: return u.KeyCode.Return;
                case Key.Escape: return u.KeyCode.Escape;
                case Key.Menu: return u.KeyCode.Menu;
                case Key.Backslash: return u.KeyCode.Backslash;
                case Key.Equal: return u.KeyCode.Equals;
                case Key.Semicolon: return u.KeyCode.Semicolon;
                case Key.Quote: return u.KeyCode.Quote;
                case Key.Comma: return u.KeyCode.Comma;
                case Key.Period: return u.KeyCode.Period;
                case Key.ForwardSlash: return u.KeyCode.Slash;
                case Key.RightBracket: return u.KeyCode.RightBracket;
                case Key.LeftBracket: return u.KeyCode.LeftBracket;
            }
            return u.KeyCode.None;
        }
        #endregion
    }
}