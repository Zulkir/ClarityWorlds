using System;
using System.Collections.Concurrent;
using Clarity.Common.CodingUtilities.Patterns;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements.Samplers;

namespace Clarity.Engine.Visualization.Elements.Materials
{
    public static class StandardMaterial
    {
        public static StandardMaterial<object> Default { get; } = New();
        private static ConcurrentDictionary<StandardMaterialData, StandardMaterial<object>> GlobalCache { get; } = 
            new ConcurrentDictionary<StandardMaterialData, StandardMaterial<object>>();

        public static StandardMaterial<object> New() => new StandardMaterial<object>(null);
        public static StandardMaterial<TMaster> New<TMaster>(TMaster master) => new StandardMaterial<TMaster>(master);
        public static StandardMaterial<object> FromGlobalCache(StandardMaterialData data) =>
            GlobalCache.GetOrAdd(data, x => new StandardMaterial<object>(x));
    }

    public class StandardMaterial<TMaster> : IStandardMaterial
    {
        private readonly TMaster master;

        private ProxyProperty<TMaster, Color4> diffuseColorProp;
        private ProxyProperty<TMaster, IImage> diffuseMapProp;
        private ProxyProperty<TMaster, IImage> normalMapProp;
        private ProxyProperty<TMaster, IImageSampler> samplerProp;
        private ProxyProperty<TMaster, bool> ignoreLightingProp;
        private ProxyProperty<TMaster, bool> noSpecularProp;
        private ProxyProperty<TMaster, HighlightEffect> highlightEffectProp;
        private ProxyProperty<TMaster, RtTransparencyMode> rtTransparencyModeProp;

        private ProxyProperty<TMaster, bool?> hasTransparencyOverrideProp;

        public StandardMaterial(TMaster master)
        {
            this.master = master;
            SetDiffuseColor(Color4.White);
            SetSampler(ImageSampler.Default);
        }

        public StandardMaterial(StandardMaterialData data)
        {
            SetDiffuseColor(data.DiffuseColor);
            SetDiffuseMap(data.DiffuseMap);
            SetNormalMap(data.NormalMap);
            SetSampler(data.Sampler);
            SetIgnoreLighting(data.IgnoreLighting);
            SetNoSpecular(data.NoSpecular);
            SetHighlightEffect(data.HighlightEffect);
            SetRtTransparencyMode(data.RtTransparencyMode);
        }

        public Color4 DiffuseColor => diffuseColorProp.GetValue(master);
        public IImage DiffuseMap => diffuseMapProp.GetValue(master);
        public IImage NormalMap => normalMapProp.GetValue(master);
        public IImageSampler Sampler => samplerProp.GetValue(master);
        public bool IgnoreLighting => ignoreLightingProp.GetValue(master);
        public bool NoSpecular => noSpecularProp.GetValue(master);
        public HighlightEffect HighlightEffect => highlightEffectProp.GetValue(master);
        public RtTransparencyMode RtTransparencyMode => rtTransparencyModeProp.GetValue(master);

        public bool HasTransparency => hasTransparencyOverrideProp.GetValue(master) ?? DiffuseColor.A != 1f || (DiffuseMap?.HasTransparency ?? false);

        public StandardMaterial<TMaster> SetDiffuseColor(Color4 immutableValue) { diffuseColorProp = new ProxyProperty<TMaster, Color4>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetDiffuseMap(IImage immutableValue) { diffuseMapProp = new ProxyProperty<TMaster, IImage>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetNormalMap(IImage immutableValue) { normalMapProp = new ProxyProperty<TMaster, IImage>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetSampler(IImageSampler immutableValue) { samplerProp = new ProxyProperty<TMaster, IImageSampler>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetIgnoreLighting(bool immutableValue) { ignoreLightingProp = new ProxyProperty<TMaster, bool>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetNoSpecular(bool immutableValue) { noSpecularProp = new ProxyProperty<TMaster, bool>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetHighlightEffect(HighlightEffect immutableValue) { highlightEffectProp = new ProxyProperty<TMaster, HighlightEffect>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetRtTransparencyMode(RtTransparencyMode immutableValue) { rtTransparencyModeProp = new ProxyProperty<TMaster, RtTransparencyMode>(immutableValue); return this; }
        public StandardMaterial<TMaster> SetHasTransparencyOverride(bool? immutableValue) { hasTransparencyOverrideProp = new ProxyProperty<TMaster, bool?>(immutableValue); return this; }

        public StandardMaterial<TMaster> SetDiffuseColor(Func<TMaster, Color4> getter) { diffuseColorProp = new ProxyProperty<TMaster, Color4>(getter); return this; }
        public StandardMaterial<TMaster> SetDiffuseMap(Func<TMaster, IImage> getter) { diffuseMapProp = new ProxyProperty<TMaster, IImage>(getter); return this; }
        public StandardMaterial<TMaster> SetNormalMap(Func<TMaster, IImage> getter) { normalMapProp = new ProxyProperty<TMaster, IImage>(getter); return this; }
        public StandardMaterial<TMaster> SetSampler(Func<TMaster, IImageSampler> getter) { samplerProp = new ProxyProperty<TMaster, IImageSampler>(getter); return this; }
        public StandardMaterial<TMaster> SetIgnoreLighting(Func<TMaster, bool> getter) { ignoreLightingProp = new ProxyProperty<TMaster, bool>(getter); return this; }
        public StandardMaterial<TMaster> SetNoSpecular(Func<TMaster, bool> getter) { noSpecularProp = new ProxyProperty<TMaster, bool>(getter); return this; }
        public StandardMaterial<TMaster> SetHighlightEffect(Func<TMaster, HighlightEffect> getter) { highlightEffectProp = new ProxyProperty<TMaster, HighlightEffect>(getter); return this; }
        public StandardMaterial<TMaster> SetRtTransparencyMode(Func<TMaster, RtTransparencyMode> getter) { rtTransparencyModeProp = new ProxyProperty<TMaster, RtTransparencyMode>(getter); return this; }
        public StandardMaterial<TMaster> SetHasTransparencyOverride(Func<TMaster, bool?> getter) { hasTransparencyOverrideProp = new ProxyProperty<TMaster, bool?>(getter); return this; }

        public StandardMaterialData GetData() =>
            new StandardMaterialData(this);

        public StandardMaterialImmutabilityFlags GetImmutability()
        {
            var result = StandardMaterialImmutabilityFlags.None;

            if (hasTransparencyOverrideProp.IsImmutable && hasTransparencyOverrideProp.GetValue(master).HasValue ||
                diffuseMapProp.IsImmutable && (diffuseMapProp.GetValue(master)?.Volatility ?? ResourceVolatility.Immutable) == ResourceVolatility.Immutable)
                result |= StandardMaterialImmutabilityFlags.HasTransparency;
            
            if (diffuseColorProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.DiffuseColor;
            if (diffuseMapProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.DiffuseMap;
            if (normalMapProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.NormalMap;
            if (samplerProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.Sampler;
            if (ignoreLightingProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.IgnoreLighting;
            if (noSpecularProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.NoSpecular;
            if (highlightEffectProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.HighlightEffect;
            if (rtTransparencyModeProp.IsImmutable)
                result |= StandardMaterialImmutabilityFlags.RtTransparencyMode;
            return result;
        }

        public StandardMaterial<object> FromGlobalCache()
        {
            if (!ReferenceEquals(master, null))
                throw new InvalidOperationException("Only master-less materials are allowed in the global cache.");
            return StandardMaterial.FromGlobalCache(GetData());
        }
    }
}