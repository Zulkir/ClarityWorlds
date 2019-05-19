using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RichTextBox : AmObjectBase<RichTextBox>, IRichTextBox
    {
        private readonly IRichTextBoxLayoutBuilder layoutBuilder;

        public abstract IntSize2 Size { get; set; }
        public float PixelScaling { get; set; }
        public abstract IRichText Text { get; set; }
        public abstract Vector2[] BorderCurve { get; set; }

        private IRichTextBoxLayout layout;
        private bool layoutIsDirty;

        public IRichTextBoxLayout Layout
        {
            get
            {
                if (!layoutIsDirty)
                    return layout;
                layout = layoutBuilder.Build(Text, Size);
                layoutIsDirty = false;
                return layout;
            }
        }

        public ICacheContainer CacheContainer { get; }

        protected RichTextBox(IRichTextBoxLayoutBuilder layoutBuilder)
        {
            this.layoutBuilder = layoutBuilder;
            PixelScaling = 512;
            CacheContainer = new CacheContainer();
            Text = AmFactory.Create<RichText>();
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            foreach (var cache in CacheContainer.GetAll())
                cache.OnMasterEvent(message);
            layoutIsDirty = true;
        }
    }
}