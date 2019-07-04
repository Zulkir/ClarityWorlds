using System;
using Clarity.Common.Numericals.Colors;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentColorPicker<T> : IFluentControl
    {
        private readonly ColorPicker etoControl;
        private readonly Func<T> getObject;
        private readonly Func<T, Color4> getValue;
        private readonly Action<T, Color4> setValue;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;

        public FluentColorPicker(Func<T> getObject, Func<T, Color4> getValue, Action<T, Color4> setValue)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            etoControl = new ColorPicker();           
            etoControl.ValueChanged += OnValueChanged;
        }

        public void Update()
        {
            var eValue = Color.FromArgb(getValue(getObject()).ToArgb());
            if (etoControl.Value == eValue)
                return;
            suppressEvents = true;
            etoControl.Value = eValue;
            suppressEvents = false;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (suppressEvents)
                return;
            var eColor = etoControl.Value;
            var cColor = new Color4(eColor.R, eColor.G, eColor.B, eColor.A);
            setValue(getObject(), cColor);
        }
    }
}