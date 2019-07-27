using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Clarity.App.Worlds.Assets;
using Clarity.Engine.Gui.WindowQueries;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using WpfMath;

namespace Clarity.Ext.TextImlets.Latex
{
    public class LatexRtEmbeddingHandler : IRtEmbeddingHandler
    {
        public IReadOnlyList<string> HandledTypesStatic { get; } = new[] { "latex" };
        public IReadOnlyList<string> HandledTypes => HandledTypesStatic;

        private readonly IWindowQueryService windowQueryService;
        private readonly TexFormulaParser parser;

        public LatexRtEmbeddingHandler(IWindowQueryService windowQueryService)
        {
            this.windowQueryService = windowQueryService;
            parser = new TexFormulaParser();
        }

        public IImage BuildImage(IRtEmbeddingSpan embedding)
        {
            if (!TryParseAndRender(embedding.SourceCode, embedding.Style, out var pngBytes, out var error) &&
                !TryParseAndRender("ERROR", embedding.Style, out pngBytes, out var error2))
                throw new Exception(error2);
            Bitmap bitmap;
            using (var stream = new MemoryStream(pngBytes, false))
                bitmap = new Bitmap(stream);
            return new SysDrawImage(bitmap);
        }

        private bool TryParseAndRender(string text, IRtSpanStyle style, out byte[] pngBytes, out string error)
        {
            try
            {
                var formula = parser.Parse(text);
                pngBytes = formula.RenderToPng(1.6 * style.Size, 0, 0, "Arial");
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                pngBytes = null;
                error = ex.Message;
                return false;
            }
        }

        public bool TryHandleMouseEvent(IRtEmbeddingSpan embedding, IMouseEvent args)
        {
            if (!args.IsLeftDoubleClickEvent() || args.KeyModifiers != KeyModifiers.None)
                return false;
            windowQueryService.QueryTextMutable("Formula Editor", embedding.SourceCode, x => embedding.SourceCode = x);
            return true;
        }
    }
}