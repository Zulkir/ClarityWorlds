using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentGuiBuilder<T> : IFluentGuiBuilder<T>
    {
        private readonly IFluentBuildableControl<T> control;

        public FluentGuiBuilder(IFluentBuildableControl<T> control)
        {
            this.control = control;
        }

        public T GetObject() => control.GetObject();

        public IFluentGuiTableBuilder<T> Table() => Table(x => x);
        public IFluentGuiTableBuilder<TChild> Table<TChild>(Func<T, TChild> getChild)
        {
            var table = new FluentTableControl<TChild>(control.OnChildLayoutChanged, () => getChild(GetObject()));
            control.AddChild(table);
            return table.Build();
        }

        public IFluentGuiBuilder<TChild> Panel<TChild>(Func<T, TChild> getChild, Func<TChild, bool> visible)
        {
            var panel = new FluentPanel<TChild>(() => getChild(GetObject()), visible);
            control.AddChild(panel);
            return panel.Build();
        }
     
        public IFluentGuiBuilder<TChild> GroupBox<TChild>(string name, Func<T, TChild> getChild, Func<TChild, bool> visible)
        {
            var groupBox = new FluentGroupBox<TChild>(name, () => getChild(GetObject()), visible);
            control.AddChild(groupBox);
            return groupBox.Build();
        }

        public void Label(Func<T, string> getValue)
        {
            control.AddChild(new FluentLabel<T>(GetObject, getValue));
        }

        public void ColorPicker(string text, Expression<Func<T, Color4>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            control.AddChild(new FluentColorPicker<T>(text, GetObject, 
                x => (Color4)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void CheckBox(string text, Expression<Func<T, bool?>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            control.AddChild(new FluentCheckBox<T>(text, GetObject, 
                x => (bool?)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void CheckBox(string text, Expression<Func<T, bool>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            control.AddChild(new FluentCheckBox<T>(text, GetObject, 
                x => (bool?) prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void Button(string text, Action<T> onClick)
        {
            var button = new FluentButton<T>(text, GetObject, onClick);
            control.AddChild(button);
        }

        public void TextBox(Expression<Func<T, string>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            control.AddChild(new FluentTextBox<T>(GetObject, 
                x => (string)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void DropDown<TValue>(Expression<Func<T, TValue>> path, IReadOnlyDictionary<string, TValue> values)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            control.AddChild(new FluentDropDown<T, TValue>(GetObject, 
                x => (TValue)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v), values));
        }
    }
}