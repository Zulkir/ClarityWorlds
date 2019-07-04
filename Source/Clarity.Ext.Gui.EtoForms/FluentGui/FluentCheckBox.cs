using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    class FluentCheckBox<T> : IFluentControl<T>
    {
        private readonly CheckBox etoControl;
        private readonly Func<T> getObject;
        private readonly Func<T,bool? > getValue;
        private readonly Action<T, bool?> setValue;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentCheckBox(string text, Func<T> getObject, Func<T, bool?> getValue,Action<T, bool?> setValue)
        {
            this.getValue = getValue;
            this.getObject = getObject;
            this.setValue = setValue;
            etoControl = new CheckBox{Text = text};
            etoControl.CheckedChanged += OnCheck;
        }

        private void OnCheck(object sender, EventArgs e)
        {
            if (!suppressEvents)
                setValue(getObject(), etoControl.Checked);
        }

        public void Update()
        {
            var eValue = getValue(getObject());
            if (etoControl.Checked == eValue)
                return;
            suppressEvents = true;
            etoControl.Checked = eValue;
            suppressEvents = false;
        }
    }
}
