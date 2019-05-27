using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public class FontFamilyCache : IFontFamilyCache
    {
        private readonly Dictionary<string, FontFamily> dict = new Dictionary<string, FontFamily>();

        // todo: handle more correctly
        public FontFamily GetFontFamily(string fontFamilyName) => 
            dict.GetOrAdd(fontFamilyName, x => Fonts.AvailableFontFamilies.FirstOrDefault(y => y.Name == x) ?? Fonts.AvailableFontFamilies.First(y => y.Name == "Arial"));
    }
}