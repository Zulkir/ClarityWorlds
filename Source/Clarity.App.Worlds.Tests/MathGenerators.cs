using System;
using System.Collections.Generic;
using Clarity.Common.Numericals;
using NUnit.Framework;

namespace Clarity.App.Worlds.Tests
{
    public class MathGenerators
    {
        [Test]
        public void MakeSectionGreen()
        {
            Assert.That(true, Is.True);
        }

        [Test]
        [Ignore]
        public void GenerateDeterminants4x4()
        {
            for (int r1 = 0; r1 < 4; r1++)
            for (int r2 = r1 + 1; r2 < 4; r2++)
            for (int c1 = 0; c1 < 4; c1++)
            for (int c2 = c1 + 1; c2 < 4; c2++)
            {
                var rowSet = IntSet32.Range(0, 4).Without(r1).Without(r2);
                int dr1 = rowSet.First();
                int dr2 = rowSet.Last();
                var colSet = IntSet32.Range(0, 4).Without(c1).Without(c2);
                int dc1 = colSet.First();
                int dc2 = colSet.Last();

                Console.WriteLine("private float D{0}{1}{2}{3} {{ get {{ return M{4}{5} * M{6}{7} - M{4}{7} * M{6}{5}; }} }}",
                    r1, r2, c1, c2, dr1, dc1, dr2, dc2);
            }

            Console.WriteLine();

            for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
            {
                int dr = r != 0 ? 0 : 1;
                var dcols = IntSet32.Range(0, 4).Without(c);
                var memberList = new List<string>();
                foreach (int dc in dcols)
                {
                    int dr1 = System.Math.Min(r, dr);
                    int dr2 = System.Math.Max(r, dr);
                    int dc1 = System.Math.Min(c, dc);
                    int dc2 = System.Math.Max(c, dc);

                    memberList.Add(string.Format("M{0}{1} * D{2}{3}{4}{5}", dr, dc, dr1, dr2, dc1, dc2));
                }
                Console.WriteLine("private float D{0}{1} {{ get {{ return {2} - {3} + {4}; }} }}",
                    r, c, memberList[0], memberList[1], memberList[2]);
            }
        }

        [Test]
        [Ignore]
        public void GenerateDeterminants4x3()
        {
            for (int r1 = 0; r1 < 4; r1++)
            for (int r2 = r1 + 1; r2 < 4; r2++)
            for (int c1 = 0; c1 < 4; c1++)
            for (int c2 = c1 + 1; c2 < 4; c2++)
            {
                var rowSet = IntSet32.Range(0, 4).Without(r1).Without(r2);
                int dr1 = rowSet.First();
                int dr2 = rowSet.Last();
                var colSet = IntSet32.Range(0, 4).Without(c1).Without(c2);
                int dc1 = colSet.First();
                int dc2 = colSet.Last();

                Console.WriteLine("private float D{0}{1}{2}{3} {{ get {{ return M{4}{5} * M{6}{7} - M{4}{7} * M{6}{5}; }} }}",
                    r1, r2, c1, c2, dr1, dc1, dr2, dc2);
            }

            Console.WriteLine();

            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                {
                    int dc = c != 3 ? 3 : 2;
                    var drows = IntSet32.Range(0, 4).Without(r);
                    var memberList = new List<string>();
                    foreach (int dr in drows)
                    {
                        int dr1 = System.Math.Min(r, dr);
                        int dr2 = System.Math.Max(r, dr);
                        int dc1 = System.Math.Min(c, dc);
                        int dc2 = System.Math.Max(c, dc);

                        memberList.Add(string.Format("M{0}{1} * D{2}{3}{4}{5}", dr, dc, dr1, dr2, dc1, dc2));
                    }
                    Console.WriteLine("private float D{0}{1} {{ get {{ return {2} - {3} + {4}; }} }}",
                        r, c, memberList[0], memberList[1], memberList[2]);
                }
        }

        [Test]
        [Ignore]
        public void GenerateDeterminants3x3()
        {
            for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
            {
                var rowSet = IntSet32.Range(0, 3).Without(r);
                int dr1 = rowSet.First();
                int dr2 = rowSet.Last();
                var colSet = IntSet32.Range(0, 3).Without(c);
                int dc1 = colSet.First();
                int dc2 = colSet.Last();

                Console.WriteLine("private float D{0}{1} {{ get {{ return M{2}{3} * M{4}{5} - M{2}{5} * M{4}{3}; }} }}",
                    r, c, dr1, dc1, dr2, dc2);
            }
        }
    }
}