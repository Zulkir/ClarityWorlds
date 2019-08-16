using System;
using System.Collections.Generic;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentTextBox<TObj, TVal> : IFluentControl<TObj>
    {
        private readonly Func<TObj> getObject;
        private readonly Func<TObj, TVal> getValue;
        private readonly Action<TObj, TVal> setValue;
        private readonly Func<TVal, string> valToStr;
        private readonly Func<string, TVal> strToVal;
        private readonly TextBox etoControl;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public TObj GetObject() => getObject();

        public FluentTextBox(Func<TObj> getObject, 
            Func<TObj, TVal> getValue, Action<TObj, TVal> setValue, 
            Func<TVal, string> valToStr, Func<string, TVal> strToVal)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            this.valToStr = valToStr;
            this.strToVal = strToVal;
            etoControl = new TextBox();
            etoControl.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!suppressEvents)
                setValue(getObject(), strToVal(etoControl.Text));
        }

        public void Update()
        {
            if (etoControl.HasFocus)
                return;
            var value = getValue(getObject());
            if (EqualityComparer<TVal>.Default.Equals(strToVal(etoControl.Text), value))
                return;
            suppressEvents = true;
            etoControl.Text = valToStr(value);
            suppressEvents = false;
        }
    }
}
