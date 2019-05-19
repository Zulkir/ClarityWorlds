using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AaBox
    {
        public Vector3 Center;
        public Size3 HalfSize;

        public AaBox(Vector3 center, Size3 halfSize)
        {
            Center = center;
            HalfSize = halfSize;
        }

        public bool Contains(Vector3 point)
        {
            return
                Math.Abs(point.X - Center.X) <= HalfSize.Width &&
                Math.Abs(point.Y - Center.Y) <= HalfSize.Height &&
                Math.Abs(point.Z - Center.Z) <= HalfSize.Depth;
        }

        public static AaBox FromCenter(Vector3 center, Size3 halfSize) => new AaBox
        {
            Center = center,
            HalfSize = halfSize
        };

        public static AaBox BoundingBox(IEnumerable<Vector3> points)
        {
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var point in points)
            {
                CodingHelper.UpdateIfLess(ref min.X, point.X);
                CodingHelper.UpdateIfLess(ref min.Y, point.Y);
                CodingHelper.UpdateIfLess(ref min.Z, point.Z);
                CodingHelper.UpdateIfGreater(ref max.X, point.X);
                CodingHelper.UpdateIfGreater(ref max.Y, point.Y);
                CodingHelper.UpdateIfGreater(ref max.Z, point.Z);
            }
            var halfSizeVec = (max - min) / 2;
            if (halfSizeVec.X < 0)
                throw new ArgumentException("'points' parameter cannot be empty.");
            return new AaBox
            {
                Center = (min + max) / 2,
                HalfSize = new Size3(halfSizeVec)
            };
        }

        public static AaBox BoundingBox(IEnumerable<Sphere> spheres)
        {
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var sphere in spheres)
            {
                CodingHelper.UpdateIfLess(ref min.X, sphere.Center.X - sphere.Radius);
                CodingHelper.UpdateIfLess(ref min.Y, sphere.Center.Y - sphere.Radius);
                CodingHelper.UpdateIfLess(ref min.Z, sphere.Center.Z - sphere.Radius);
                CodingHelper.UpdateIfGreater(ref max.X, sphere.Center.X + sphere.Radius);
                CodingHelper.UpdateIfGreater(ref max.Y, sphere.Center.Y + sphere.Radius);
                CodingHelper.UpdateIfGreater(ref max.Z, sphere.Center.Z + sphere.Radius);
            }
            var halfSizeVec = (max - min) / 2;
            if (halfSizeVec.X < 0)
                throw new ArgumentException("'spheres' parameter cannot be empty.");
            return new AaBox
            {
                Center = (min + max) / 2,
                HalfSize = new Size3(halfSizeVec)
            };
        }
    }
}