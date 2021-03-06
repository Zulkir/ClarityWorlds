﻿using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentButton<T> : IFluentControl<T>
    {
        private readonly Button etoControl;
        private readonly Func<T> getObject;
        private readonly Action<T> onClick;

        public Control EtoControl => etoControl;
        public bool IsVisible => true;
        public T GetObject() => getObject();

        public FluentButton(string text, Func<T> getObject, Action<T> onClick)
        {
            this.getObject = getObject;
            this.onClick = onClick;
            etoControl = new Button{Text = text};
            etoControl.Click += OnButtonClicked;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
           onClick(getObject());
        }

        public void Update()
        {
        }
    }
}