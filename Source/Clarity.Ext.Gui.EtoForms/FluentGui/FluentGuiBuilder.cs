using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentGuiBuilder<T> : IFluentGuiBuilder<T>
    {
        public IFluentContainerControl<T> Control { get; }

        public FluentGuiBuilder(IFluentContainerControl<T> control)
        {
            Control = control;
        }

        public T GetObject() => Control.GetObject();

        public IFluentGuiBuilder<TChild> Panel<TChild>(Func<T, TChild> getChild, Func<TChild, bool> visible)
        {
            var panel = new FluentPanel<TChild>(() => getChild(GetObject()), visible);
            Control.Children.Add(panel);
            return panel.Build();
        }
     
        public IFluentGuiBuilder<TChild> GroupBox<TChild>(string name, Func<T, TChild> getChild, Func<TChild, bool> visible)
        {
            var panel = new FluentGroupBox<TChild>(name, () => getChild(GetObject()), visible);
            Control.Children.Add(panel);
            return panel.Build();
        }

        public void Label(Func<T, string> getValue)
        {
            Control.Children.Add(new FluentLabel<T>(GetObject, getValue));
        }

        public void ColorPicker(Expression<Func<T, Color4>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            Control.Children.Add(new FluentColorPicker<T>(GetObject, x => (Color4)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void CheckBox(string text, Expression<Func<T, bool?>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            Control.Children.Add(new FluentCheckBox<T>(text, GetObject, x => (bool?)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void CheckBox(string text, Expression<Func<T, bool>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            Control.Children.Add(new FluentCheckBox<T>(text, GetObject, x => (bool?) prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void Button(string text, Action<T> onClick)
        {
            var button = new FluentButton<T>(text, GetObject, onClick);
            Control.Children.Add(button);
        }

        public void TextBox(Expression<Func<T, string>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            Control.Children.Add(new FluentTextBox<T>(GetObject, x => (string)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void DropDown<TValue>(Expression<Func<T, TValue>> path, IReadOnlyDictionary<string, TValue> values)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            Control.Children.Add(new FluentDropDown<T, TValue>(GetObject, x => (TValue)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v), values));
        }
    }
}