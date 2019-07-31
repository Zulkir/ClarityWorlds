using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct AaRectangle2 : IEquatable<AaRectangle2>
    {
        public Vector2 Center;
        public float HalfWidth;
        public float HalfHeight;

        public float X => Center.X - HalfWidth;
        public float Y => Center.Y - HalfHeight;
        public float Width => HalfWidth * 2;
        public float Height => HalfHeight * 2;

        public float MinX => Center.X - HalfWidth;
        public float MaxX => Center.X + HalfWidth;
        public float MinY => Center.Y - HalfHeight;
        public float MaxY => Center.Y + HalfHeight;

        public Vector2 MinMin => new Vector2(MinX, MinY);
        public Vector2 MaxMin => new Vector2(MaxX, MinY);
        public Vector2 MinMax => new Vector2(MinX, MaxY);
        public Vector2 MaxMax => new Vector2(MaxX, MaxY);

        //public AaRectangle2(float x, float y, float width, float height)
        //{
        //    Center.X = x + width / 2;
        //    Center.Y = y + height / 2;
        //    HalfWidth = width / 2;
        //    HalfHeight = height / 2;
        //}

        public AaRectangle2(Vector2 minCorner, Size2 size)
        {
            HalfWidth = size.Width / 2;
            HalfHeight = size.Height / 2;
            Center = minCorner + new Vector2(HalfWidth, HalfHeight);
        }

        public AaRectangle2(Vector2 center, float halfWidth, float halfHeight)
        {
            Center = center;
            HalfWidth = halfWidth;
            HalfHeight = halfHeight;
        }

        public AaRectangle2(Vector2 corner1, Vector2 corner2)
        {
            Center.X = (corner1.X + corner2.X) / 2;
            Center.Y = (corner1.Y + corner2.Y) / 2;
            HalfWidth = Math.Abs(corner2.X - corner1.X) / 2;
            HalfHeight = Math.Abs(corner2.Y - corner1.Y) / 2;
        }

        public float Area => Width * Height;

        public bool ContainsX(float x, float eps = MathHelper.Eps5) =>
            MinX - eps <= x && x <= MaxX + eps;

        public bool ContainsY(float y, float eps = MathHelper.Eps5) =>
            MinY - eps <= y && y <= MaxY + eps;

        public bool ContainsPoint(float x, float y, float eps = MathHelper.Eps5) => 
            ContainsX(x, eps) && ContainsY(y, eps);

        public bool ContainsPoint(Vector2 point, float eps = MathHelper.Eps5) => 
            ContainsPoint(point.X, point.Y, eps);

        public IEnumerable<LineSegment2> GetSegments()
        {
            yield return new LineSegment2(MinMin, MinMax);
            yield return new LineSegment2(MinMax, MaxMax);
            yield return new LineSegment2(MaxMax, MaxMin);
            yield return new LineSegment2(MaxMin, MinMin);
        }

        public bool IntersectsWith(AaRectangle2 other)
        {
            var centerDiff = Center - other.Center;
            centerDiff = new Vector2(Math.Abs(centerDiff.X), Math.Abs(centerDiff.Y));
            return
                centerDiff.X <= HalfWidth + other.HalfWidth &&
                centerDiff.Y <= HalfHeight + other.HalfHeight;
        }

        public Circle2 GetBoundingCircle()
        {
            var radius = MathHelper.Sqrt(HalfWidth.Sq() + HalfHeight.Sq());
            return new Circle2(Center, radius);
        }

        public float DistanceFrom(Vector2 point)
        {
            var relPoint = point - Center;
            var absRelPoint = new Vector2(Math.Abs(relPoint.X), Math.Abs(relPoint.Y));
            var axisDistances = new Vector2(
                Math.Max(0, absRelPoint.X - HalfWidth),
                Math.Max(0, absRelPoint.Y - HalfHeight));
            return axisDistances.Length();
        }

        public float DistanceToBorderFrom(Vector2 point)
        {
            var relPoint = point - Center;
            var absRelPoint = new Vector2(Math.Abs(relPoint.X), Math.Abs(relPoint.Y));
            var axisDistances = new Vector2(
                Math.Abs(absRelPoint.X - HalfWidth),
                Math.Abs(absRelPoint.Y - HalfHeight));
            return Math.Max(DistanceFrom(point), Math.Min(axisDistances.X, axisDistances.Y));
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return
                X.GetHashCode() ^
                (Y.GetHashCode() << 1) ^
                (Width.GetHashCode() << 2) ^
                (Height.GetHashCode() << 3);
        }

        public override string ToString()
        {
            return $"{{{Center}, {Width.ToString(CultureInfo.InvariantCulture)}, {Height.ToString(CultureInfo.InvariantCulture)}}}";
        }

        public static bool Equals(AaRectangle2 s1, AaRectangle2 s2)
        {
            return
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Width == s2.Width &&
                s1.Height == s2.Height;
        }

        public static bool operator ==(AaRectangle2 s1, AaRectangle2 s2) { return Equals(s1, s2); }
        public static bool operator !=(AaRectangle2 s1, AaRectangle2 s2) { return !Equals(s1, s2); }
        public bool Equals(AaRectangle2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is AaRectangle2 && Equals((AaRectangle2)obj); }
        #endregion

        public static AaRectangle2 FromBounds(float minX, float maxX, float minY, float maxY)
        {
            return new AaRectangle2(new Vector2(minX, minY), new Vector2(maxX, maxY));
        }

        public static AaRectangle2 FromCornerAndDimensions(float minX, float minY, float width, float height)
        {
            return new AaRectangle2(new Vector2(minX, minY), new Vector2(minX, minY) + new Vector2(width, height));
        }

        public static AaRectangle2 FromCenter(Vector2 center, float halfWidth, float halfHeight)
        {
            return new AaRectangle2(center, halfWidth, halfHeight);
        }

        public static AaRectangle2 Lerp(AaRectangle2 left, AaRectangle2 right, float amount)
        {
            return new AaRectangle2(
                Vector2.Lerp(left.Center, right.Center, amount),
                MathHelper.Lerp(left.HalfWidth, right.HalfWidth, amount),
                MathHelper.Lerp(left.HalfHeight, right.HalfHeight, amount));
        }

        public static AaRectangle2 BoundingRect(IEnumerable<Vector2> points)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var point in points)
            {
                CodingHelper.UpdateIfLess(ref minX, point.X);
                CodingHelper.UpdateIfLess(ref minY, point.Y);
                CodingHelper.UpdateIfGreater(ref maxX, point.X);
                CodingHelper.UpdateIfGreater(ref maxY, point.Y);
            }

            return minX != float.MaxValue ? FromBounds(minX, maxX, minY, maxY) : new AaRectangle2();
        }

        public static AaRectangle2 BoundingRect(IEnumerable<AaRectangle2> rects)
        {
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (var rect in rects)
            {
                CodingHelper.UpdateIfLess(ref minX, rect.MinX);
                CodingHelper.UpdateIfLess(ref minY, rect.MinY);
                CodingHelper.UpdateIfGreater(ref maxX, rect.MaxX);
                CodingHelper.UpdateIfGreater(ref maxY, rect.MaxY);
            }

            return minX != float.MaxValue ? FromBounds(minX, maxX, minY, maxY) : new AaRectangle2();
        }
    }
}
