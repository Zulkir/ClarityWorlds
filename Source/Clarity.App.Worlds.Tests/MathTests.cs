using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using NUnit.Framework;

namespace Clarity.App.Worlds.Tests
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void TestConstruction()
        {
            var set = IntSet32.Range(7, 15).Without(11).Without(16).With(29).With(31);
            Assert.That(set.ToString(), Is.EqualTo("00000001111011110111110000000101"));
        }

        [Test]
        public void QuaternionAssociativity()
        {
            var q1 = Quaternion.RotationX(MathHelper.PiOver2);
            var q2 = Quaternion.RotationY(MathHelper.PiOver2);
            var x = Vector3.UnitX;
            var y = Vector3.UnitY;
            var z = Vector3.UnitZ;
            Assert.That(AbsDiff(x * q1 * q2, x * (q1 * q2)), Is.LessThan(0.00001f));
            Assert.That(AbsDiff(y * q1 * q2, y * (q1 * q2)), Is.LessThan(0.00001f));
            Assert.That(AbsDiff(z * q1 * q2, z * (q1 * q2)), Is.LessThan(0.00001f));
        }

        [Test]
        public void QuaternionRotatesAsMatrix()
        {
            var q = Quaternion.RotationAxis(new Vector3(123, 234, 345), 1.1f);
            var m = q.ToMatrix3x3();
            var x = Vector3.UnitX;
            var y = Vector3.UnitY;
            var z = Vector3.UnitZ;
            Assert.That(AbsDiff(x * q, x * m), Is.LessThan(0.00001f));
            Assert.That(AbsDiff(y * q, y * m), Is.LessThan(0.00001f));
            Assert.That(AbsDiff(z * q, z * m), Is.LessThan(0.00001f));
        }

        [Test]
        public void QuaternionRotationFrame()
        {
            var q = Quaternion.RotationToFrame(Vector3.UnitY, Vector3.UnitZ);
            var x = Vector3.UnitX;
            var y = Vector3.UnitY;
            var z = Vector3.UnitZ;
            Assert.That(AbsDiff(x * q, y), Is.LessThan(0.00001f));
            Assert.That(AbsDiff(y * q, z), Is.LessThan(0.00001f));
            Assert.That(AbsDiff(z * q, x), Is.LessThan(0.00001f));
        }

        private static float AbsDiff(Vector3 v1, Vector3 v2) { return (v1 - v2).Length(); }
    }
}