using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    class FluentDropDown<TObject, TValue> : IFluentControl<TObject>
    {
        private readonly DropDown etoControl;
        private readonly Func<TObject> getObject;
        private readonly Func<TObject, TValue> getValue;
        private readonly Action<TObject, TValue> setValue;
        private readonly IReadOnlyDictionary<string, TValue> values;
        private bool suppressEvents;

        private TValue chosenValue;

        public bool IsVisible => true;
        public Control EtoControl => etoControl;
        public TObject GetObject() => getObject();

        public FluentDropDown(Func<TObject> getObject, Func<TObject, TValue> getValue, 
            Action<TObject, TValue> setValue, IReadOnlyDictionary<string, TValue> values)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            this.values = values;

            etoControl = new DropDown
            {
                DataStore = values.Keys.ToList(),
            };
            etoControl.SelectedValueChanged += OnValueChanged;
            chosenValue = (TValue)etoControl.SelectedValue;
        }

        private void OnValueChanged(object sender, EventArgs e)  
        {
            if (suppressEvents)
                return;
            var key = (string)etoControl.SelectedValue;
            var newValue = key != null ? values[key] : default(TValue);
            chosenValue = newValue;
            setValue(getObject(), newValue);
        }

        public void Update()
        {
            var value = getValue(getObject());
            if (EqualityComparer<TValue>.Default.Equals(chosenValue, value))
                return;
            var valueStr = values
                .Where(x => EqualityComparer<TValue>.Default.Equals(x.Value, value))
                .FirstOrNull()?.Key;
            chosenValue = value;
            suppressEvents = true;
            etoControl.SelectedValue = valueStr;
            suppressEvents = false;
        }
    }
}
