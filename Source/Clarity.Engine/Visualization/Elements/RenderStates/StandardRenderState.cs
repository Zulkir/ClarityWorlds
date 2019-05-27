using System;
using System.Collections.Concurrent;
using Clarity.Common.CodingUtilities.Patterns;

namespace Clarity.Engine.Visualization.Elements.RenderStates
{
    public static class StandardRenderState
    {
        public static IStandardRenderState Default { get; } = New();
        private static ConcurrentDictionary<StandardRenderStateData, StandardRenderState<object>> GlobalCache { get; } = 
            new ConcurrentDictionary<StandardRenderStateData, StandardRenderState<object>>();

        public static StandardRenderState<object> New() => new StandardRenderState<object>(null);
        public static StandardRenderState<TMaster> New<TMaster>(TMaster master) => new StandardRenderState<TMaster>(master);
        public static StandardRenderState<object> FromGlobalCache(StandardRenderStateData data) =>
            GlobalCache.GetOrAdd(data, x => new StandardRenderState<object>(x));
    }

    public class StandardRenderState<TMaster> : IStandardRenderState
    {
        private readonly TMaster master;

        private ProxyProperty<TMaster, CullFace> cullFaceProp;
        private ProxyProperty<TMaster, PolygonMode> polygonModeProp;
        private ProxyProperty<TMaster, float> zOffsetProp;
        private ProxyProperty<TMaster, float> pointSizeProp;
        private ProxyProperty<TMaster, float> lineWidthProp;

        public CullFace CullFace => cullFaceProp.GetValue(master);
        public PolygonMode PolygonMode => polygonModeProp.GetValue(master);
        public float ZOffset => zOffsetProp.GetValue(master);
        public float PointSize => pointSizeProp.GetValue(master);
        public float LineWidth => lineWidthProp.GetValue(master);

        public StandardRenderState(TMaster master)
        {
            this.master = master;
            SetPointSize(1);
            SetLineWidth(1);
        }

        public StandardRenderState(StandardRenderStateData data)
        {
            SetCullFace(data.CullFace);
            SetPolygonMode(data.PolygonMode);
            SetZOffset(data.ZOffset);
            SetPointSize(data.PointSize);
            SetLineWidth(data.LineWidth);
        }

        public StandardRenderState<TMaster> SetCullFace(CullFace immutableValue) { cullFaceProp = new ProxyProperty<TMaster, CullFace>(immutableValue); return this; }
        public StandardRenderState<TMaster> SetPolygonMode(PolygonMode immutableValue) { polygonModeProp = new ProxyProperty<TMaster, PolygonMode>(immutableValue); return this; }
        public StandardRenderState<TMaster> SetZOffset(float immutableValue) { zOffsetProp = new ProxyProperty<TMaster, float>(immutableValue); return this; }
        public StandardRenderState<TMaster> SetPointSize(float immutableValue) { pointSizeProp = new ProxyProperty<TMaster, float>(immutableValue); return this; }
        public StandardRenderState<TMaster> SetLineWidth(float immutableValue) { lineWidthProp = new ProxyProperty<TMaster, float>(immutableValue); return this; }

        public StandardRenderState<TMaster> SetCullFace(Func<TMaster, CullFace> getter) { cullFaceProp = new ProxyProperty<TMaster, CullFace>(getter); return this; }
        public StandardRenderState<TMaster> SetPolygonMode(Func<TMaster, PolygonMode> getter) { polygonModeProp = new ProxyProperty<TMaster, PolygonMode>(getter); return this; }
        public StandardRenderState<TMaster> SetZOffset(Func<TMaster, float> getter) { zOffsetProp = new ProxyProperty<TMaster, float>(getter); return this; }
        public StandardRenderState<TMaster> SetPointSize(Func<TMaster, float> getter) { pointSizeProp = new ProxyProperty<TMaster, float>(getter); return this; }
        public StandardRenderState<TMaster> SetLineWidth(Func<TMaster, float> getter) { lineWidthProp = new ProxyProperty<TMaster, float>(getter); return this; }

        public StandardRenderStateData GetFallbackData() => GetData();
        public StandardRenderStateData GetData() => new StandardRenderStateData(this);

        public StandardRenderStateImmutabilityFlags GetImmutability()
        {
            var result = StandardRenderStateImmutabilityFlags.None;
            if (cullFaceProp.IsImmutable)
                result |= StandardRenderStateImmutabilityFlags.CullFace;
            if (polygonModeProp.IsImmutable)
                result |= StandardRenderStateImmutabilityFlags.PolygonMode;
            if (zOffsetProp.IsImmutable)
                result |= StandardRenderStateImmutabilityFlags.ZOffset;
            if (pointSizeProp.IsImmutable)
                result |= StandardRenderStateImmutabilityFlags.PointSize;
            if (lineWidthProp.IsImmutable)
                result |= StandardRenderStateImmutabilityFlags.LineWidth;
            return result;
        }

        public StandardRenderState<object> FromGlobalCache()
        {
            if (!ReferenceEquals(master, null))
                throw new InvalidOperationException("Only master-less render states are allowed in the global cache.");
            return StandardRenderState.FromGlobalCache(GetData());
        }
    }
}