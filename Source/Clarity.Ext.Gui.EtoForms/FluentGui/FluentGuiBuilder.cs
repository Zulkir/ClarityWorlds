using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentGuiBuilder<T> : IFluentGuiBuilder<T>
    {
        private readonly Func<T> getObject;
        private readonly Action<IFluentControl> addControl;
        private readonly Action onChildLayoutChanged;

        public FluentGuiBuilder(Func<T> getObject, Action<IFluentControl> addControl, Action onChildLayoutChanged)
        {
            this.getObject = getObject;
            this.addControl = addControl;
            this.onChildLayoutChanged = onChildLayoutChanged;
        }

        public T GetObject() => getObject();

        public IFluentTableBuilder<T> Table() => Table(x => x);
        public IFluentTableBuilder<TChild> Table<TChild>(Func<T, TChild> getChild)
        {
            var table = new FluentTableControl<TChild>(onChildLayoutChanged, () => getChild(GetObject()));
            addControl(table);
            return table.Build();
        }

        public IFluentGuiBuilder<TChild> Panel<TChild>(Func<T, TChild> getChild, Func<TChild, bool> visible)
        {
            var panel = new FluentPanel<TChild>(() => getChild(GetObject()), visible);
            addControl(panel);
            return panel.Build();
        }
     
        public IFluentGuiBuilder<TChild> GroupBox<TChild>(string name, Func<T, TChild> getChild, Func<TChild, bool> visible)
        {
            var groupBox = new FluentGroupBox<TChild>(name, () => getChild(GetObject()), visible);
            addControl(groupBox);
            return groupBox.Build();
        }

        public void Label(string text)
        {
            addControl(new FluentLabel<T>(GetObject, x => text));
        }

        public void Label(Func<T, string> getValue)
        {
            addControl(new FluentLabel<T>(GetObject, getValue));
        }

        public void ColorPicker(Expression<Func<T, Color3>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentColorPicker<T>(GetObject, 
                x => new Color4((Color3)prop.GetValue(x), 1f),
                (x, v) => prop.SetValue(x, v.RGB)));
        }

        public void ColorPicker(Expression<Func<T, Color4>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentColorPicker<T>(GetObject, 
                x => (Color4)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void CheckBox(string text, Expression<Func<T, bool?>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentCheckBox<T>(text, GetObject, 
                x => (bool?)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void CheckBox(string text, Expression<Func<T, bool>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentCheckBox<T>(text, GetObject, 
                x => (bool?) prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void Button(string text, Action<T> onClick)
        {
            var button = new FluentButton<T>(text, GetObject, onClick);
            addControl(button);
        }

        public void TextBox(Expression<Func<T, string>> path)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentTextBox<T>(GetObject, 
                x => (string)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v)));
        }

        public void DropDown<TValue>(Expression<Func<T, TValue>> path, Dictionary<string, TValue> values)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentDropDown<T, TValue>(GetObject, 
                x => (TValue)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v), values));
        }

        public void Slider(Expression<Func<T, float>> path, float minValue, float maxValue, int numSteps)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentSlider<T>(GetObject, 
                x => (float)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v),
                minValue, maxValue, numSteps));
        }

        public void NumericUpDown(Expression<Func<T, double>> path, double minValue, double maxValue)
        {
            var prop = CodingHelper.GetPropertyInfo(path);
            addControl(new FluentNumericUpDown<T>(GetObject,
                x => (double)prop.GetValue(x),
                (x, v) => prop.SetValue(x, v),
                minValue, maxValue));
        }
    }
}