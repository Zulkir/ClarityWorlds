using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentNumericUpDown<T> : IFluentControl<T>
    {
        private readonly Func<T> getObject;
        private readonly Func<T, double> getValue;
        private readonly Action<T, double> setValue;
        private readonly NumericUpDown etoControl;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentNumericUpDown(Func<T> getObject, Func<T, double> getValue, Action<T, double> setValue, 
            double minValue, double maxValue)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            etoControl = new NumericUpDown
            {
                MinValue = minValue,
                MaxValue = maxValue
            };
            etoControl.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (!suppressEvents)
                setValue(getObject(), etoControl.Value);
        }

        public void Update()
        {
            if (etoControl.HasFocus)
                return;
            var value = getValue(getObject());
            if (etoControl.Value == value)
                return;
            suppressEvents = true;
            etoControl.Value = value;
            suppressEvents = false;
        }
    }
}