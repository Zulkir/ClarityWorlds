using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.HandlerFamilies
{
    public class NumericalsTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (type == typeof(Vector4))
            {
                handler = new Vector4TrwHandler();
                return true;
            }
            if (type == typeof(Vector3))
            {
                handler = new Vector3TrwHandler();
                return true;
            }
            if (type == typeof(Vector2))
            {
                handler = new Vector2TrwHandler();
                return true;
            }
            if (type == typeof(Transform))
            {
                handler = new TransformTrwHandler();
                return true;
            }
            if (type == typeof(Color4))
            {
                handler = new ProxyTrwHandler<Color4, Vector4>(x => x.Raw, x => new Color4(x), false);
                return true;
            }
            if (type == typeof(Quaternion))
            {
                handler = new ProxyTrwHandler<Quaternion, Vector4>(x => x.Raw, x => new Quaternion(x), false);
                return true;
            }
            if (type == typeof(AaRectangle2))
            {
                handler = new ProxyTrwHandler<AaRectangle2, Vector4>(
                    x => new Vector4(x.Center.X, x.Center.Y, x.HalfWidth, x.HalfHeight), 
                    x => new AaRectangle2(new Vector2(x.X, x.Y), x.Z, x.W),
                    false);
                return true;
            }
            if (type == typeof(IntSize2))
            {
                handler = new ProxyTrwHandler<IntSize2, Vector2>(x => new Vector2(x.Width, x.Height), x => new IntSize2((int)x.X, (int)x.Y), false);
                return true;
            }

            handler = null;
            return false;
        }
    }
}