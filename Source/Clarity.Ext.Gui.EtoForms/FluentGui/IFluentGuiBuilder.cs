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
        void TextBox<TVal>(Expression<Func<T, TVal>> path, Func<TVal, string> valToStr, Func<string, TVal> strToVal);
        void TextBox(Expression<Func<T, string>> path);
        void TextBox(Expression<Func<T, int>> path);
        void TextBox(Expression<Func<T, float>> path);
        void TextBox(Expression<Func<T, double>> path);
        void DropDown<TValue>(Expression<Func<T, TValue>> path, Dictionary<string, TValue> values);
        void Slider(Expression<Func<T, float>> path, float minValue, float maxValue, int numSteps);
        void NumericUpDown(Expression<Func<T, int>> path, int minValue, int maxValue);
        void NumericUpDown(Expression<Func<T, float>> path, float minValue, float maxValue);
        void NumericUpDown(Expression<Func<T, double>> path, double minValue, double maxValue);
        void NumericUpDown<TVal>(Expression<Func<T, TVal>> path, TVal minValue, TVal maxValue, Func<TVal, double> valToDouble, Func<double, TVal> doubleToVal);
    }
}