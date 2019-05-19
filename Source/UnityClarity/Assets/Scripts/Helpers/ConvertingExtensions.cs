using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Interaction.Input.Keyboard;

namespace Assets.Scripts.Helpers
{
    public static class ConvertingExtensions
    {
        #region Vector-like types
        public static Vector2 ToClarity(this UnityEngine.Vector2 uv) =>
            new Vector2(uv.x, uv.y);

        public static Vector3 ToClarity(this UnityEngine.Vector3 uv, bool invertZ) =>
            new Vector3(uv.x, uv.y, invertZ ? -uv.z : uv.z);

        public static Vector4 ToClarity(this UnityEngine.Vector4 uv, bool invertZ) =>
            new Vector4(uv.x, uv.y, invertZ ? -uv.z : uv.z, uv.w);

        public static Color4 ToClarity(this UnityEngine.Color uc) =>
            new Color4(uc.r, uc.g, uc.b, uc.a);

        public static UnityEngine.Vector2 ToUnity(this Vector2 cv) =>
            new UnityEngine.Vector2(cv.X, cv.Y);

        public static UnityEngine.Vector3 ToUnity(this Vector3 cv, bool invertZ) =>
            new UnityEngine.Vector3(cv.X, cv.Y, invertZ ? -cv.Z : cv.Z);

        public static UnityEngine.Vector4 ToUnity(this Vector4 cv, bool invertZ) =>
            new UnityEngine.Vector4(cv.X, cv.Y, invertZ ? -cv.Z : cv.Z, cv.W);

        public static UnityEngine.Quaternion ToUnity(this Quaternion cq, bool invertZ) =>
            invertZ
                ? new UnityEngine.Quaternion(cq.X, cq.Y, -cq.Z, cq.W)
                : new UnityEngine.Quaternion(cq.X, cq.Y, -cq.Z, cq.W);

        public static UnityEngine.Color ToUnity(this Color4 cc) =>
            new UnityEngine.Color(cc.R, cc.G, cc.B, cc.A);
        #endregion

        public static Key ToClarity(this UnityEngine.KeyCode ukc)
        {
            switch (ukc)
            {
                case UnityEngine.KeyCode.None: return Key.None;
                case UnityEngine.KeyCode.A: return Key.A;
                case UnityEngine.KeyCode.B: return Key.B;
                case UnityEngine.KeyCode.C: return Key.C;
                case UnityEngine.KeyCode.D: return Key.D;
                case UnityEngine.KeyCode.E: return Key.E;
                case UnityEngine.KeyCode.F: return Key.F;
                case UnityEngine.KeyCode.G: return Key.G;
                case UnityEngine.KeyCode.H: return Key.H;
                case UnityEngine.KeyCode.I: return Key.I;
                case UnityEngine.KeyCode.J: return Key.J;
                case UnityEngine.KeyCode.K: return Key.K;
                case UnityEngine.KeyCode.L: return Key.L;
                case UnityEngine.KeyCode.M: return Key.M;
                case UnityEngine.KeyCode.N: return Key.N;
                case UnityEngine.KeyCode.O: return Key.O;
                case UnityEngine.KeyCode.P: return Key.P;
                case UnityEngine.KeyCode.Q: return Key.Q;
                case UnityEngine.KeyCode.R: return Key.R;
                case UnityEngine.KeyCode.S: return Key.S;
                case UnityEngine.KeyCode.T: return Key.T;
                case UnityEngine.KeyCode.U: return Key.U;
                case UnityEngine.KeyCode.V: return Key.V;
                case UnityEngine.KeyCode.W: return Key.W;
                case UnityEngine.KeyCode.X: return Key.X;
                case UnityEngine.KeyCode.Y: return Key.Y;
                case UnityEngine.KeyCode.Z: return Key.Z;
                case UnityEngine.KeyCode.F1: return Key.F1;
                case UnityEngine.KeyCode.F2: return Key.F2;
                case UnityEngine.KeyCode.F3: return Key.F3;
                case UnityEngine.KeyCode.F4: return Key.F4;
                case UnityEngine.KeyCode.F5: return Key.F5;
                case UnityEngine.KeyCode.F6: return Key.F6;
                case UnityEngine.KeyCode.F7: return Key.F7;
                case UnityEngine.KeyCode.F8: return Key.F8;
                case UnityEngine.KeyCode.F9: return Key.F9;
                case UnityEngine.KeyCode.F10: return Key.F10;
                case UnityEngine.KeyCode.F11: return Key.F11;
                case UnityEngine.KeyCode.F12: return Key.F12;
                case UnityEngine.KeyCode.Alpha0: return Key.D0;
                case UnityEngine.KeyCode.Alpha1: return Key.D1;
                case UnityEngine.KeyCode.Alpha2: return Key.D2;
                case UnityEngine.KeyCode.Alpha3: return Key.D3;
                case UnityEngine.KeyCode.Alpha4: return Key.D4;
                case UnityEngine.KeyCode.Alpha5: return Key.D5;
                case UnityEngine.KeyCode.Alpha6: return Key.D6;
                case UnityEngine.KeyCode.Alpha7: return Key.D7;
                case UnityEngine.KeyCode.Alpha8: return Key.D8;
                case UnityEngine.KeyCode.Alpha9: return Key.D9;
                case UnityEngine.KeyCode.Minus: return Key.Minus;
                case UnityEngine.KeyCode.Plus: return Key.Plus;
                case UnityEngine.KeyCode.BackQuote: return Key.Grave;
                case UnityEngine.KeyCode.Insert: return Key.Insert;
                case UnityEngine.KeyCode.Home: return Key.Home;
                case UnityEngine.KeyCode.PageUp: return Key.PageUp;
                case UnityEngine.KeyCode.PageDown: return Key.PageDown;
                case UnityEngine.KeyCode.Delete: return Key.Delete;
                case UnityEngine.KeyCode.End: return Key.End;
                case UnityEngine.KeyCode.KeypadDivide: return Key.Divide;
                case UnityEngine.KeyCode.KeypadPeriod: return Key.Decimal;
                case UnityEngine.KeyCode.Backspace: return Key.Backspace;
                case UnityEngine.KeyCode.UpArrow: return Key.Up;
                case UnityEngine.KeyCode.DownArrow: return Key.Down;
                case UnityEngine.KeyCode.LeftArrow: return Key.Left;
                case UnityEngine.KeyCode.RightArrow: return Key.Right;
                case UnityEngine.KeyCode.Tab: return Key.Tab;
                case UnityEngine.KeyCode.Space: return Key.Space;
                case UnityEngine.KeyCode.CapsLock: return Key.CapsLock;
                case UnityEngine.KeyCode.ScrollLock: return Key.ScrollLock;
                case UnityEngine.KeyCode.Print: return Key.PrintScreen;
                case UnityEngine.KeyCode.Numlock: return Key.NumberLock;
                case UnityEngine.KeyCode.Return: return Key.Enter;
                case UnityEngine.KeyCode.Escape: return Key.Escape;
                case UnityEngine.KeyCode.Menu: return Key.Menu;
                case UnityEngine.KeyCode.Backslash: return Key.Backslash;
                case UnityEngine.KeyCode.Equals: return Key.Equal;
                case UnityEngine.KeyCode.Semicolon: return Key.Semicolon;
                case UnityEngine.KeyCode.Quote: return Key.Quote;
                case UnityEngine.KeyCode.Comma: return Key.Comma;
                case UnityEngine.KeyCode.Period: return Key.Period;
                case UnityEngine.KeyCode.Slash: return Key.ForwardSlash;
                case UnityEngine.KeyCode.RightBracket: return Key.RightBracket;
                case UnityEngine.KeyCode.LeftBracket: return Key.LeftBracket;
            }
            return Key.None;
        }

        public static UnityEngine.KeyCode ToUnity(this Key ckc)
        {
            switch (ckc)
            {
                case Key.None: return UnityEngine.KeyCode.None;
                case Key.A: return UnityEngine.KeyCode.A;
                case Key.B: return UnityEngine.KeyCode.B;
                case Key.C: return UnityEngine.KeyCode.C;
                case Key.D: return UnityEngine.KeyCode.D;
                case Key.E: return UnityEngine.KeyCode.E;
                case Key.F: return UnityEngine.KeyCode.F;
                case Key.G: return UnityEngine.KeyCode.G;
                case Key.H: return UnityEngine.KeyCode.H;
                case Key.I: return UnityEngine.KeyCode.I;
                case Key.J: return UnityEngine.KeyCode.J;
                case Key.K: return UnityEngine.KeyCode.K;
                case Key.L: return UnityEngine.KeyCode.L;
                case Key.M: return UnityEngine.KeyCode.M;
                case Key.N: return UnityEngine.KeyCode.N;
                case Key.O: return UnityEngine.KeyCode.O;
                case Key.P: return UnityEngine.KeyCode.P;
                case Key.Q: return UnityEngine.KeyCode.Q;
                case Key.R: return UnityEngine.KeyCode.R;
                case Key.S: return UnityEngine.KeyCode.S;
                case Key.T: return UnityEngine.KeyCode.T;
                case Key.U: return UnityEngine.KeyCode.U;
                case Key.V: return UnityEngine.KeyCode.V;
                case Key.W: return UnityEngine.KeyCode.W;
                case Key.X: return UnityEngine.KeyCode.X;
                case Key.Y: return UnityEngine.KeyCode.Y;
                case Key.Z: return UnityEngine.KeyCode.Z;
                case Key.F1: return UnityEngine.KeyCode.F1;
                case Key.F2: return UnityEngine.KeyCode.F2;
                case Key.F3: return UnityEngine.KeyCode.F3;
                case Key.F4: return UnityEngine.KeyCode.F4;
                case Key.F5: return UnityEngine.KeyCode.F5;
                case Key.F6: return UnityEngine.KeyCode.F6;
                case Key.F7: return UnityEngine.KeyCode.F7;
                case Key.F8: return UnityEngine.KeyCode.F8;
                case Key.F9: return UnityEngine.KeyCode.F9;
                case Key.F10: return UnityEngine.KeyCode.F10;
                case Key.F11: return UnityEngine.KeyCode.F11;
                case Key.F12: return UnityEngine.KeyCode.F12;
                case Key.D0: return UnityEngine.KeyCode.Alpha0;
                case Key.D1: return UnityEngine.KeyCode.Alpha1;
                case Key.D2: return UnityEngine.KeyCode.Alpha2;
                case Key.D3: return UnityEngine.KeyCode.Alpha3;
                case Key.D4: return UnityEngine.KeyCode.Alpha4;
                case Key.D5: return UnityEngine.KeyCode.Alpha5;
                case Key.D6: return UnityEngine.KeyCode.Alpha6;
                case Key.D7: return UnityEngine.KeyCode.Alpha7;
                case Key.D8: return UnityEngine.KeyCode.Alpha8;
                case Key.D9: return UnityEngine.KeyCode.Alpha9;
                case Key.Minus: return UnityEngine.KeyCode.Minus;
                case Key.Plus: return UnityEngine.KeyCode.Plus;
                case Key.Grave: return UnityEngine.KeyCode.BackQuote;
                case Key.Insert: return UnityEngine.KeyCode.Insert;
                case Key.Home: return UnityEngine.KeyCode.Home;
                case Key.PageUp: return UnityEngine.KeyCode.PageUp;
                case Key.PageDown: return UnityEngine.KeyCode.PageDown;
                case Key.Delete: return UnityEngine.KeyCode.Delete;
                case Key.End: return UnityEngine.KeyCode.End;
                case Key.Divide: return UnityEngine.KeyCode.KeypadDivide;
                case Key.Decimal: return UnityEngine.KeyCode.KeypadPeriod;
                case Key.Backspace: return UnityEngine.KeyCode.Backspace;
                case Key.Up: return UnityEngine.KeyCode.UpArrow;
                case Key.Down: return UnityEngine.KeyCode.DownArrow;
                case Key.Left: return UnityEngine.KeyCode.LeftArrow;
                case Key.Right: return UnityEngine.KeyCode.RightArrow;
                case Key.Tab: return UnityEngine.KeyCode.Tab;
                case Key.Space: return UnityEngine.KeyCode.Space;
                case Key.CapsLock: return UnityEngine.KeyCode.CapsLock;
                case Key.ScrollLock: return UnityEngine.KeyCode.ScrollLock;
                case Key.PrintScreen: return UnityEngine.KeyCode.Print;
                case Key.NumberLock: return UnityEngine.KeyCode.Numlock;
                case Key.Enter: return UnityEngine.KeyCode.Return;
                case Key.Escape: return UnityEngine.KeyCode.Escape;
                case Key.Menu: return UnityEngine.KeyCode.Menu;
                case Key.Backslash: return UnityEngine.KeyCode.Backslash;
                case Key.Equal: return UnityEngine.KeyCode.Equals;
                case Key.Semicolon: return UnityEngine.KeyCode.Semicolon;
                case Key.Quote: return UnityEngine.KeyCode.Quote;
                case Key.Comma: return UnityEngine.KeyCode.Comma;
                case Key.Period: return UnityEngine.KeyCode.Period;
                case Key.ForwardSlash: return UnityEngine.KeyCode.Slash;
                case Key.RightBracket: return UnityEngine.KeyCode.RightBracket;
                case Key.LeftBracket: return UnityEngine.KeyCode.LeftBracket;
            }
            return UnityEngine.KeyCode.None;
        }
    }
}