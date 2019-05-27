using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    class FluentCheckBox<T> : IFluentControl
    {
        private readonly CheckBox etoControl;
        private readonly Func<T> getObject;
        private readonly Func<T,bool? > getValue;
        private readonly Action<T, bool?> setValue;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;

        public FluentCheckBox(string text, Func<T> getObject, Func<T, bool?> getValue,Action<T, bool?> setValue)
        {
            this.getValue = getValue;
            this.getObject = getObject;
            this.setValue = setValue;
            etoControl = new CheckBox{Text = text};
            etoControl.CheckedChanged += OnCheck; //onIgnoreLighningChanged
        }

        private void OnCheck(object sender, EventArgs e)
        {
            setValue(getObject(), etoControl.Checked);
        }

        public void Update()
        {
            var eValue = getValue(getObject());
            if (etoControl.Checked != eValue)
                etoControl.Checked = eValue;
        }
    }
}
