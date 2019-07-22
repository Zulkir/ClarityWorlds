using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentGuiBuilder<T>
    {
        T GetObject();
        IFluentTableBuilder<T> Table();
        IFluentTableBuilder<TChild> Table<TChild>(Func<T, TChild> getChild);
        IFluentGuiBuilder<TChild> Panel<TChild>(Func<T, TChild> getChild, Func<TChild, bool> visible);
        IFluentGuiBuilder<TChild> GroupBox<TChild>(string name, Func<T, TChild> getChild, Func<TChild, bool> visible);
        void Label(string text);
        void Label(Func<T, string> getValue);
        void ColorPicker(Expression<Func<T, Color3>> path);
        void ColorPicker(Expression<Func<T, Color4>> path);
        void CheckBox(string text, Expression<Func<T, bool?>> path);
        void CheckBox(string text, Expression<Func<T, bool>> path);
        void Button(string text, Action<T> onClick);
        void TextBox(Expression<Func<T, string>> path);
        void DropDown<TValue>(Expression<Func<T, TValue>> path, Dictionary<string, TValue> values);
        void Slider(Expression<Func<T, float>> path, float minValue, float maxValue, int numSteps);
        void NumericUpDown(Expression<Func<T, double>> path, double minValue, double maxValue);
    }
}