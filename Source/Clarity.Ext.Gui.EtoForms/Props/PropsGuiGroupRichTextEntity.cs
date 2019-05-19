using System;
using System.Linq;
using Clarity.Common.Numericals.Colors;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;
using FontDecoration = Clarity.Engine.Media.Text.Rich.FontDecoration;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupRichTextEntity : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly DropDown cTransparencyMode;
        private readonly ColorPicker cBackgroundColor;
        private readonly Slider cBackgroundOpacity;
        private readonly DropDown cAlignment;
        private readonly DropDown cDirection;
        private readonly NumericUpDown cTabCount;
        private readonly NumericUpDown cMarginUp;
        private readonly CheckBox cBold;
        private readonly CheckBox cItalic;
        private readonly CheckBox cUnderline;
        private readonly CheckBox cStrikeThrough;
        private readonly DropDown cFontFamily;
        private readonly NumericUpDown cFontSize;
        private readonly ColorPicker cTextColor;
        private IRichTextComponent boundComponent;
        private int suppressControlEvents = 0;

        private bool IgnoreEvents => boundComponent == null || suppressControlEvents > 0;

        public PropsGuiGroupRichTextEntity(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cTransparencyMode = new DropDown
            {
                Width = 120,
                DataStore = new []
                {
                    RtTransparencyMode.Opaque,
                    RtTransparencyMode.Native,
                    RtTransparencyMode.BlackIsTransparent,
                    RtTransparencyMode.WhiteIsTransparent,
                }.Select(x => (object)x)
            };
            cTransparencyMode.SelectedKeyChanged += OnTransparencyModeChanged;

            cBackgroundColor = new ColorPicker();
            cBackgroundColor.ValueChanged += OnBackgroundChanged;

            cBackgroundOpacity = new Slider
            {
                MinValue = 0,
                MaxValue = 255,
                TickFrequency = 256,
                SnapToTick = false
            };
            cBackgroundOpacity.ValueChanged += OnBackgroundChanged;

            cAlignment = new DropDown
            {
                Width = 120,
                DataStore = new[]
                {
                    RtParagraphAlignment.Left,
                    RtParagraphAlignment.Center,
                    RtParagraphAlignment.Right
                }.Select(x => (object)x)
            };
            cAlignment.SelectedKeyChanged += OnAlignmentChanged;

            cDirection = new DropDown
            {
                Width = 120,
                DataStore = new[]
                {
                    RtParagraphDirection.LeftToRight,
                    RtParagraphDirection.RightToLeft
                }.Select(x => (object)x)
            };
            cDirection.SelectedKeyChanged += OnDirectionChanged;

            cTabCount = new NumericUpDown
            {
                MinValue = 0,
                MaxValue = 16
            };
            cTabCount.ValueChanged += OnTabCountChanged;

            cMarginUp = new NumericUpDown
            {
                MinValue = 0,
                MaxValue = 1000
            };
            cMarginUp.ValueChanged += OnMarginUpChanged;

            cBold = new CheckBox { Text = "Bold" };
            cBold.CheckedChanged += OnBoldChanged;

            cItalic = new CheckBox { Text = "Italic" };
            cItalic.CheckedChanged += OnItalicChanged;

            cUnderline = new CheckBox { Text = "Underline" };
            cUnderline.CheckedChanged += OnUnderlineChanged;

            cStrikeThrough = new CheckBox { Text = "Strikethrough" };
            cStrikeThrough.CheckedChanged += OnStrikethroughChanged;

            cFontFamily = new DropDown
            {
                Width = 120,
                DataStore = Fonts.AvailableFontFamilies.OrderBy(r => r.Name)
            };
            cFontFamily.SelectedIndexChanged += OnFontFamilyChanged;

            cFontSize = new NumericUpDown
            {
                MinValue = 1,
                MaxValue = 200
            };
            cFontSize.ValueChanged += OnFontSizeChanged;

            cTextColor = new ColorPicker();
            cTextColor.ValueChanged += OnTextColorChanged;
            
            var layout = new TableLayout(
                new TableRow(new TableCell(new Label { Text = "Bgnd Mode" }), new TableCell(cTransparencyMode)),
                new TableRow(new TableCell(new Label { Text = "Bgnd Color" }), new TableCell(cBackgroundColor)),
                new TableRow(new TableCell(new Label { Text = "Bgnd Opacity" }), new TableCell(cBackgroundOpacity)),
                new TableRow(new TableCell(new Label { Text = "Alignment" }), new TableCell(cAlignment)),
                new TableRow(new TableCell(new Label { Text = "Direction" }), new TableCell(cDirection)),
                new TableRow(new TableCell(new Label { Text = "Tabs" }), new TableCell(cTabCount)),
                new TableRow(new TableCell(new Label { Text = "MarginUp" }), new TableCell(cMarginUp)),
                new TableRow(new TableCell(new Label {Text = "Font"}), new TableCell(cFontFamily)),
                new TableRow(new TableCell(new Label {Text = "Size"}), new TableCell(cFontSize)),
                new TableRow(new TableCell(new Label {Text = "Color"}), new TableCell(cTextColor)),
                new TableRow(new TableCell(cBold), new TableCell(cItalic)),
                new TableRow(new TableCell(cUnderline), new TableCell(cStrikeThrough)))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Rich Text",
                Content = layout
            };
        }

        

        public bool Actualize(ISceneNode node)
        {
            boundComponent = node.SearchComponent<IRichTextComponent>();
            if (boundComponent == null)
                return false;

            suppressControlEvents++;
            ActualizeNonText();
            suppressControlEvents--;
            return true;
        }

        private void ActualizeNonText()
        {
            suppressControlEvents++;
            var range = boundComponent.SelectionRange;

            if (range.HasValue)
            {
                cBold.Checked = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value,
                    x => x.Style.FontDecoration.HasFlags(FontDecoration.Bold), out var commonBold)
                    ? commonBold
                    : (bool?)null;

                cItalic.Checked = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value,
                    x => x.Style.FontDecoration.HasFlags(FontDecoration.Italic), out var commonItalic)
                    ? commonItalic
                    : (bool?)null;

                cUnderline.Checked = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value,
                    x => x.Style.FontDecoration.HasFlag(FontDecoration.Underline), out var commonUnderline)
                    ? commonUnderline
                    : (bool?)null;

                cStrikeThrough.Checked = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value,
                    x => x.Style.FontDecoration.HasFlag(FontDecoration.Strikethrough), out var commonStrikethrough)
                    ? commonStrikethrough
                    : (bool?)null;

                cFontFamily.SelectedKey = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value, 
                    x => x.Style.FontFamily, out var commonFontFamily)
                    ? commonFontFamily
                    : null;

                cFontSize.Value = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value, 
                    x => x.Style.Size, out var commonFontSize)
                    ? commonFontSize
                    : 1;

                cTextColor.Value = boundComponent.TextBox.Text.TryGetCommonSpanProperty(range.Value,
                    x => x.Style.TextColor, out var commonTextColor)
                    ? Color.FromArgb(commonTextColor.ToArgb())
                    : new Color(0, 0, 0, 0);

                cAlignment.SelectedValue = boundComponent.TextBox.Text.TryGetCommonParagraphProperty(range.Value,
                    x => (int)x.Style.Alignment, out var commonAlignment)
                    ? (RtParagraphAlignment)commonAlignment
                    : default(RtParagraphAlignment);

                cDirection.SelectedValue = boundComponent.TextBox.Text.TryGetCommonParagraphProperty(range.Value,
                    x => (int)x.Style.Direction, out var commonDirection)
                    ? (RtParagraphDirection)commonDirection
                    : default(RtParagraphDirection);

                cTabCount.Value = boundComponent.TextBox.Text.TryGetCommonParagraphProperty(range.Value, 
                    x => x.Style.TabCount, out var commonTabCount)
                    ? commonTabCount
                    : 0;

                cMarginUp.Value = boundComponent.TextBox.Text.TryGetCommonParagraphProperty(range.Value, 
                    x => x.Style.MarginUp, out var commonMarginUp)
                    ? commonMarginUp
                    : 0;
            }
            else
            {
                cBold.Checked = boundComponent.InputTextStyle.FontDecoration.HasFlags(FontDecoration.Bold);
                cItalic.Checked = boundComponent.InputTextStyle.FontDecoration.HasFlags(FontDecoration.Italic);
                cUnderline.Checked = boundComponent.InputTextStyle.FontDecoration.HasFlags(FontDecoration.Underline);
                cStrikeThrough.Checked = boundComponent.InputTextStyle.FontDecoration.HasFlags(FontDecoration.Strikethrough);
                cFontFamily.SelectedKey = boundComponent.InputTextStyle.FontFamily;
                cFontSize.Value = boundComponent.InputTextStyle.Size;
                cTextColor.Value = Color.FromArgb(boundComponent.InputTextStyle.TextColor.ToArgb());
                var para = boundComponent.TextBox.Text.Paragraphs[boundComponent.CursorPosition.ParaIndex];
                cAlignment.SelectedValue = para.Style.Alignment;
                cDirection.SelectedValue = para.Style.Direction;
                cTabCount.Value = para.Style.TabCount;
                cMarginUp.Value = para.Style.MarginUp;
            }

            cTransparencyMode.SelectedValue = boundComponent.TextBox.Text.Style.TransparencyMode;
            ActualizeBackgroundControls();

            suppressControlEvents--;
        }

        private void OnTransparencyModeChanged(object sender, EventArgs eventArgs)
        {
            if (IgnoreEvents)
                return;
            var mode = (RtTransparencyMode)cTransparencyMode.SelectedValue;
            boundComponent.TextBox.Text.Style.TransparencyMode = mode;
            switch (boundComponent.TextBox.Text.Style.TransparencyMode)
            {
                case RtTransparencyMode.Opaque:
                    boundComponent.TextBox.Text.Style.BackgroundColor = new Color4(boundComponent.TextBox.Text.Style.BackgroundColor.RGB, 1f);
                    break;
                case RtTransparencyMode.Native:
                    boundComponent.TextBox.Text.Style.BackgroundColor = new Color4(boundComponent.TextBox.Text.Style.BackgroundColor.RGB, 0f);
                    break;
                case RtTransparencyMode.BlackIsTransparent:
                    boundComponent.TextBox.Text.Style.BackgroundColor = Color4.Black;
                    break;
                case RtTransparencyMode.WhiteIsTransparent:
                    boundComponent.TextBox.Text.Style.BackgroundColor = Color4.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ActualizeBackgroundControls();
        }

        private void ActualizeBackgroundControls()
        {
            switch (boundComponent.TextBox.Text.Style.TransparencyMode)
            {
                case RtTransparencyMode.Opaque:
                    cBackgroundColor.Enabled = true;
                    cBackgroundOpacity.Enabled = false;
                    break;
                case RtTransparencyMode.Native:
                    cBackgroundColor.Enabled = true;
                    cBackgroundOpacity.Enabled = true;
                    break;
                case RtTransparencyMode.BlackIsTransparent:
                    cBackgroundColor.Enabled = false;
                    cBackgroundOpacity.Enabled = false;
                    break;
                case RtTransparencyMode.WhiteIsTransparent:
                    cBackgroundColor.Enabled = false;
                    cBackgroundOpacity.Enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var backgroundColor = boundComponent.TextBox.Text.Style.BackgroundColor;
            cBackgroundOpacity.Value = (int)(backgroundColor.A * 255.9999f);
            cBackgroundColor.Value = Color.FromArgb(backgroundColor.RGB.ToArgb());
        }

        private void OnBackgroundChanged(object sender, EventArgs eventArgs)
        {
            if (IgnoreEvents)
                return;
            var rgb = new Color4(cBackgroundColor.Value.ToArgb()).RGB;
            var alpha = cBackgroundOpacity.Value / 255f;
            boundComponent.TextBox.Text.Style.BackgroundColor = new Color4(rgb, alpha);
        }

        private void OnAlignmentChanged(object sender, EventArgs eventArgs) => 
            OnParaPropertyChanged((RtParagraphAlignment) cAlignment.SelectedValue,
                (p, v) => undoRedo.Common.ChangeProperty(p.Style, x => x.Alignment, v));

        private void OnDirectionChanged(object sender, EventArgs eventArgs) =>
            OnParaPropertyChanged((RtParagraphDirection)cDirection.SelectedValue,
                (p, v) => undoRedo.Common.ChangeProperty(p.Style, x => x.Direction, v));

        private void OnTabCountChanged(object sender, EventArgs eventArgs) =>
            OnParaPropertyChanged((int)cTabCount.Value,
                (p, v) => undoRedo.Common.ChangeProperty(p.Style, x => x.TabCount, v));

        private void OnMarginUpChanged(object sender, EventArgs eventArgs) =>
            OnParaPropertyChanged((int)cMarginUp.Value,
                (p, v) => undoRedo.Common.ChangeProperty(p.Style, x => x.MarginUp, v));

        private void OnBoldChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged(cBold.Checked ?? false, (s, v) =>
        {
            if (v)
                s.FontDecoration |= FontDecoration.Bold;
            else
                s.FontDecoration &= ~FontDecoration.Bold;
        });

        private void OnItalicChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged(cItalic.Checked ?? false, (s, v) =>
        {
            if (v)
                s.FontDecoration |= FontDecoration.Italic;
            else
                s.FontDecoration &= ~FontDecoration.Italic;
        });

        private void OnUnderlineChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged(cUnderline.Checked ?? false, (s, v) =>
        {
            if (v)
                s.FontDecoration |= FontDecoration.Underline;
            else
                s.FontDecoration &= ~FontDecoration.Underline;
        });

        private void OnStrikethroughChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged(cStrikeThrough.Checked ?? false, (s, v) =>
        {
            if (v)
                s.FontDecoration |= FontDecoration.Strikethrough;
            else
                s.FontDecoration &= ~FontDecoration.Strikethrough;
        });

        private void OnFontFamilyChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged(cFontFamily.SelectedKey, (s, v) => { s.FontFamily = v; });

        private void OnFontSizeChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged((float)cFontSize.Value, (s, v) => { s.Size = v; });

        private void OnTextColorChanged(object sender, EventArgs eventArgs) => OnSpanPropertyChanged(cTextColor.Value.ToArgb(), (s, v) => { s.TextColor = new Color4(v); });

        private void OnParaPropertyChanged<T>(T value, Action<IRtParagraph, T> setAction)
        {
            if (IgnoreEvents)
                return;

            var range = boundComponent.SelectionRange;
            if (range.HasValue)
                for (int i = range.Value.FirstCharPos.ParaIndex; i <= range.Value.LastCharPos.ParaIndex; i++)
                    setAction(boundComponent.TextBox.Text.Paragraphs[i], value);
            else
                setAction(boundComponent.TextBox.Text.Paragraphs[boundComponent.CursorPosition.ParaIndex], value);
        }

        private void OnSpanPropertyChanged<T>(T value, Action<IRtSpanStyle, T> setAction)
        {
            if (IgnoreEvents)
                return;

            var oldSpanRange = boundComponent.SelectionRange;
            if (oldSpanRange.HasValue)
            {
                var newSpanRange = boundComponent.TextBox.Text.SplitRange(oldSpanRange.Value);
                foreach (var span in boundComponent.TextBox.Text.EnumerateSpans(newSpanRange))
                    setAction(span.Style, value);
            }
            else
            {
                setAction(boundComponent.InputTextStyle, value);
            }
        }
    }
}