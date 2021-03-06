﻿using System;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Gui.MessageBoxes;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using ef = Eto.Forms;
using ed = Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms
{
    public static class Converters
    {
        public static Key ToClarity(this ef.Keys key) => (Key)((int)key & 0x7F);
        public static ef.Keys ToEto(this Key key) => (ef.Keys)key;
        public static ef.Keys ToEto(Key key, KeyModifiers modifiers)
        {
            var result = key.ToEto();
            if (CodingHelper.HasFlag((int)modifiers, (int)KeyModifiers.Control))
                result |= ef.Keys.Control;
            if (CodingHelper.HasFlag((int)modifiers, (int)KeyModifiers.Shift))
                result |= ef.Keys.Shift;
            if (CodingHelper.HasFlag((int)modifiers, (int)KeyModifiers.Alt))
                result |= ef.Keys.Alt;
            return result;
        }

        public static ed.Color ToEto(Color4 color) => new ed.Color(color.R, color.G, color.B, color.A);

        public static void ToEto(FontDecoration fontDecoration, out ed.FontStyle etoStyle, out ed.FontDecoration etoDecoration)
        {
            etoStyle = ed.FontStyle.None;
            etoDecoration = ed.FontDecoration.None;
            if (fontDecoration.HasFlags(FontDecoration.Bold))
                etoStyle |= ed.FontStyle.Bold;
            if (fontDecoration.HasFlags(FontDecoration.Italic))
                etoStyle |= ed.FontStyle.Italic;
            if (fontDecoration.HasFlags(FontDecoration.Underline))
                etoDecoration |= ed.FontDecoration.Underline;
            if (fontDecoration.HasFlags(FontDecoration.Strikethrough))
                etoDecoration |= ed.FontDecoration.Strikethrough;
        }

        public static IntSize2 ToClarity(this ed.Size eSize) =>
            new IntSize2(eSize.Width, eSize.Height);

        public static Size2 ToClarity(this ed.SizeF etoSize) => 
            new Size2(etoSize.Width, etoSize.Height);

        public static ef.MessageBoxType ToEto(this MessageBoxType cType) =>
            (ef.MessageBoxType)cType;

        public static ef.MessageBoxButtons ToEto(this MessageBoxButtons cButtons) =>
            (ef.MessageBoxButtons)cButtons;

        public static ed.Image ToEto(this IImage cImage)
        {
            var eImage = new ed.Bitmap(cImage.Size.Width, cImage.Size.Height, ed.PixelFormat.Format32bppRgba);
            using (var eData = eImage.Lock())
            {
                if (eData.ScanWidth != cImage.Size.Width * 4)
                    throw new NotImplementedException();
                var rawData = cImage.GetRawData();
                Marshal.Copy(rawData, 0, eData.Data, rawData.Length);
            }
            return eImage;
        }
    }
}