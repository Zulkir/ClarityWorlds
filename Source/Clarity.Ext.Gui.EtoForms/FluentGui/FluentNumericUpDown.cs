using System;
using System.Collections.Generic;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentNumericUpDown<T, TVal> : IFluentControl<T>
    {
        private readonly Func<T> getObject;
        private readonly Func<T, TVal> getValue;
        private readonly Action<T, TVal> setValue;
        private readonly Func<TVal, double> valToDouble;
        private readonly Func<double, TVal> doubleToVal;
        private readonly NumericUpDown etoControl;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentNumericUpDown(Func<T> getObject, Func<T, TVal> getValue, Action<T, TVal> setValue, 
            TVal minValue, TVal maxValue, Func<TVal, double> valToDouble, Func<double, TVal> doubleToVal)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            this.valToDouble = valToDouble;
            this.doubleToVal = doubleToVal;
            etoControl = new NumericUpDown
            {
                MinValue = valToDouble(minValue),
                MaxValue = valToDouble(maxValue)
            };
            etoControl.ValueChanged += OnValueChanged;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (!suppressEvents)
                setValue(getObject(), doubleToVal(etoControl.Value));
        }

        public void Update()
        {
            if (etoControl.HasFocus)
                return;
            var value = getValue(getObject());
            if (EqualityComparer<TVal>.Default.Equals(doubleToVal(etoControl.Value), value))
                return;
            suppressEvents = true;
            etoControl.Value = valToDouble(value);
            suppressEvents = false;
        }
    }
}