﻿using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Ext.Rendering.Ogl3.Uniforms
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MaterialUniform
    {
        public Color4 Color;
        public Color4 PulsatingColor;
        public Bool32 UseTexture;
        public Bool32 UseNormalMap;
        public Bool32 IgnoreLighting;
        public Bool32 IsEdited;
        public Bool32 IsSelected;
        public Bool32 BlackIsTransparent;
        public Bool32 WhiteIsTransparent;
        public Bool32 NoSpecular;
        public Bool32 ScrollingEnabled;
        public float ScrollingAmount;
        public Bool32 IsPulsating;
    }
}