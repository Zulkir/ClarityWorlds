using System;
using Clarity.Common.Numericals;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentSlider<T> : IFluentControl<T>
    {
        private readonly Func<T> getObject;
        private readonly Func<T, float> getValue;
        private readonly Action<T, float> setValue;
        private readonly float minValue;
        private readonly float maxValue;
        private readonly int numSteps;
        private readonly Slider etoControl;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentSlider(Func<T> getObject, Func<T, float> getValue, Action<T, float> setValue, 
            float minValue, float maxValue, int numSteps)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.numSteps = numSteps;
            etoControl = new Slider
            {
                MinValue = 0,
                MaxValue = numSteps - 1,
                TickFrequency = numSteps,
                SnapToTick = false
            };
            etoControl.ValueChanged += OnValueChanged;
        }

        private float ValToReal(int internalVal) => MathHelper.Lerp(minValue, maxValue, (float)internalVal / (numSteps - 1));
        private int ValToInternal(float realVal) => (int)Math.Round((numSteps - 1) * (realVal - minValue) / (maxValue - minValue));

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (!suppressEvents)
                setValue(getObject(), ValToReal(etoControl.Value));
        }

        public void Update()
        {
            var value = getValue(getObject());
            var internalVal = ValToInternal(value);
            if (etoControl.Value == internalVal)
                return;
            suppressEvents = true;
            etoControl.Value = internalVal;
            suppressEvents = false;
        }
    }
}