using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Core.AppCore.WorldTree
{
    [TrwSerialize]
    public struct Rectangle3D
    {
        [TrwSerialize] public Vector3 Corner;
        [TrwSerialize] public Vector3 DirX;
        [TrwSerialize] public Vector3 DirY;
        [TrwSerialize] public float LenX;
        [TrwSerialize] public float LenY;

        public Rectangle3D(Vector3 corner, Vector3 dirX, Vector3 dirY, float lenX, float lenY)
        {
            Corner = corner;
            DirX = dirX;
            DirY = dirY;
            LenX = lenX;
            LenY = lenY;
        }

        public Rectangle3D Normalize()
        {
            var result = new Rectangle3D(Corner, DirX, DirY, LenX, LenY);
            if (result.LenX < 0)
            {
                result.LenX = -result.LenX;
                result.DirX = -result.DirX;
            }
            if (result.LenY < 0)
            {
                result.LenY = -result.LenY;
                result.DirY = -result.DirY;
            }
            if (result.DirX.X < 0)
            {
                result.Corner += result.DirX * LenX;
                result.DirX = -result.DirX;
            }
            if (result.DirY.Y < 0)
            {
                result.Corner += result.DirY * LenY;
                result.DirY = -result.DirY;
            }
            if (Math.Abs(result.DirX.X) < Math.Abs(result.DirY.X))
            {
                Swap(ref result.DirX, ref result.DirY);
                Swap(ref result.LenX, ref result.LenY);
            }
            return result;
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            var c = a;
            a = b;
            b = c;
        }
    }
}