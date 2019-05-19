using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public class FontFamilyCache : IFontFamilyCache
    {
        private readonly Dictionary<string, FontFamily> dict = new Dictionary<string, FontFamily>();

        public FontFamily GetFontFamily(string fontFamilyName) => 
            dict.GetOrAdd(fontFamilyName, x => Fonts.AvailableFontFamilies.Single(y => y.Name == x));
    }
}