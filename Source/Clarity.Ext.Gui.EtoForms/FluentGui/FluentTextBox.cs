using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentTextBox<T> : IFluentControl<T>
    {
        private readonly Func<T> getObject;
        private readonly Func<T, string> getValue;
        private readonly Action<T, string> setValue;
        private readonly TextBox etoControl;
        private bool suppressEvents;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentTextBox(Func<T> getObject, Func<T, string> getValue, Action<T, string> setValue)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            this.setValue = setValue;
            etoControl = new TextBox();
            etoControl.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            if (!suppressEvents)
                setValue(getObject(), etoControl.Text);
        }

        public void Update()
        {
            var value = getValue(getObject());
            if (etoControl.Text == value)
                return;
            suppressEvents = true;
            etoControl.Text = value;
            suppressEvents = false;
        }
    }
}
