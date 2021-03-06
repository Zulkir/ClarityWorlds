﻿using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentLabel<T> : IFluentControl<T>
    {
        private readonly Func<T> getObject;
        private readonly Func<T, string> getValue;
        private readonly Label etoControl;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentLabel(Func<T> getObject, Func<T, string> getValue)
        {
            this.getObject = getObject;
            this.getValue = getValue;
            etoControl = new Label();
        }

        public void Update()
        {
            var value = getValue(getObject());
            if (etoControl.Text != value)
                etoControl.Text = value;
        }
    }
}