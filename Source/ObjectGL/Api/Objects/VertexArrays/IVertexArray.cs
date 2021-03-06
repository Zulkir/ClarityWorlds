﻿#region License
/*
Copyright (c) 2012-2016 ObjectGL Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using ObjectGL.Api.Objects.Resources.Buffers;

namespace ObjectGL.Api.Objects.VertexArrays
{
    public interface IVertexArray : IGLObject
    {
        void SetElementArrayBuffer(IBuffer buffer);
        void SetVertexAttributeF(uint index, IBuffer buffer, VertexAttributeDimension dimension, VertexAttribPointerType type, bool normalized, int stride, int offset, uint divisor = 0);
        void SetVertexAttributeI(uint index, IBuffer buffer, VertexAttributeDimension dimension, VertexAttribIPointerType type, int stride, int offset, uint divisor = 0);
        void DisableVertexAttribute(uint index);
        void DisableVertexAttributesStartingFrom(uint startIndex);
    }
}