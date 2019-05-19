using System;
using Clarity.Common.CodingUtilities.Patterns;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Engine.Visualization.Graphics
{
    public class CgModelVisualElement : CgModelVisualElement<object>
    {
        public CgModelVisualElement() : base(null) { }
    }

    public class CgModelVisualElement<TMaster> : ICgModelVisualElement
    {
        private readonly TMaster master;

        public ICgVisualElement Fallback => this;

        private ProxyProperty<TMaster, IFlexibleModel> modelProp;
        private ProxyProperty<TMaster, int> modelPartIndexProp;
        private ProxyProperty<TMaster, IMaterial> materialProp;
        private ProxyProperty<TMaster, Transform> transformProp;
        private ProxyProperty<TMaster, Vector3> nonUniformScaleProp;
        private ProxyProperty<TMaster, CgTransformSpace> transformSpaceProp;
        private ProxyProperty<TMaster, CgCullFace> cullFaceProp;
        private ProxyProperty<TMaster, CgPolygonMode> polygonModeProp;
        private ProxyProperty<TMaster, float> zOffsetProp;
        private ProxyProperty<TMaster, CgHighlightEffect> highlightEffectProp;
        private ProxyProperty<TMaster, bool> hideProp;

        public CgBasicVisualGroup BasicVisualGroup { get; private set; }
        public ICgVisualElementExtension NextExtension { get; set; }
        public Func<TMaster, Transform, ICamera, float> GetDistanceToCameraSq { get; set; }
        public float DistanceToCameraSq(Transform elemTransform, ICamera camera) => GetDistanceToCameraSq(master, elemTransform, camera);

        public IFlexibleModel Model => modelProp.GetValue(master);
        public int ModelPartIndex => modelPartIndexProp.GetValue(master);
        public IMaterial Material => materialProp.GetValue(master);
        public Transform Transform => transformProp.GetValue(master);
        public Vector3 NonUniformScale => nonUniformScaleProp.GetValue(master);
        public CgTransformSpace TransformSpace => transformSpaceProp.GetValue(master);
        public CgCullFace CullFace => cullFaceProp.GetValue(master);
        public CgPolygonMode PolygonMode => polygonModeProp.GetValue(master);
        public float ZOffset => zOffsetProp.GetValue(master);
        public CgHighlightEffect HighlightEffect => highlightEffectProp.GetValue(master);
        public bool Hide => hideProp.GetValue(master);

        public CgModelVisualElement(TMaster master)
        {
            this.master = master;
            SetTransform(Transform.Identity);
            SetNonUniformScale(new Vector3(1, 1, 1));
            GetDistanceToCameraSq = (m, t, c) => (t.Offset - c.GetEye()).LengthSquared();
        }

        public CgModelVisualElementImmutabilityFlags GetImmutability()
        {
            var result = CgModelVisualElementImmutabilityFlags.None;
            if (modelProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.Model;
            if (modelPartIndexProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.ModelPartIndex;
            if (materialProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.Material;
            if (transformProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.Transform;
            if (nonUniformScaleProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.NonUniformTransform;
            if (transformSpaceProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.TransformSpace;
            if (cullFaceProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.CullFace;
            if (polygonModeProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.PolygonMode;
            if (zOffsetProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.ZOffset;
            if (highlightEffectProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.HighlightEffect;
            if (hideProp.IsImmutable)
                result |= CgModelVisualElementImmutabilityFlags.Hide;
            return result;
    }

        public CgModelVisualElement<TMaster> SetBasicVisualGroup(CgBasicVisualGroup visualGroup) { BasicVisualGroup = visualGroup; return this; }
        public CgModelVisualElement<TMaster> SetModel(IFlexibleModel immutableValue) { modelProp = new ProxyProperty<TMaster, IFlexibleModel>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetModelPartIndex(int immutableValue) { modelPartIndexProp = new ProxyProperty<TMaster, int>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetMaterial(IMaterial immutableValue) { materialProp = new ProxyProperty<TMaster, IMaterial>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetTransform(Transform immutableValue) { transformProp = new ProxyProperty<TMaster, Transform>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetNonUniformScale(Vector3 immutableValue) { nonUniformScaleProp = new ProxyProperty<TMaster, Vector3>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetTransformSpace(CgTransformSpace immutableValue) { transformSpaceProp = new ProxyProperty<TMaster, CgTransformSpace>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetCullFace(CgCullFace immutableValue) { cullFaceProp = new ProxyProperty<TMaster, CgCullFace>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetPolygonMode(CgPolygonMode immutableValue) { polygonModeProp = new ProxyProperty<TMaster, CgPolygonMode>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetZOffset(float immutableValue) { zOffsetProp = new ProxyProperty<TMaster, float>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetHighlightEffect(CgHighlightEffect immutableValue) { highlightEffectProp = new ProxyProperty<TMaster, CgHighlightEffect>(immutableValue); return this; }
        public CgModelVisualElement<TMaster> SetHide(bool immutableValue) { hideProp = new ProxyProperty<TMaster, bool>(immutableValue); return this; }

        public CgModelVisualElement<TMaster> SetModel(Func<TMaster, IFlexibleModel> getter) { modelProp = new ProxyProperty<TMaster, IFlexibleModel>(getter); return this; }
        public CgModelVisualElement<TMaster> SetModelPartIndex(Func<TMaster, int> getter) { modelPartIndexProp = new ProxyProperty<TMaster, int>(getter); return this; }
        public CgModelVisualElement<TMaster> SetMaterial(Func<TMaster, IMaterial> getter) { materialProp = new ProxyProperty<TMaster, IMaterial>(getter); return this; }
        public CgModelVisualElement<TMaster> SetTransform(Func<TMaster, Transform> getter) { transformProp = new ProxyProperty<TMaster, Transform>(getter); return this; }
        public CgModelVisualElement<TMaster> SetNonUniformScale(Func<TMaster, Vector3> getter) { nonUniformScaleProp = new ProxyProperty<TMaster, Vector3>(getter); return this; }
        public CgModelVisualElement<TMaster> SetTransformSpace(Func<TMaster, CgTransformSpace> getter) { transformSpaceProp = new ProxyProperty<TMaster, CgTransformSpace>(getter); return this; }
        public CgModelVisualElement<TMaster> SetCullFace(Func<TMaster, CgCullFace> getter) { cullFaceProp = new ProxyProperty<TMaster, CgCullFace>(getter); return this; }
        public CgModelVisualElement<TMaster> SetPolygonMode(Func<TMaster, CgPolygonMode> getter) { polygonModeProp = new ProxyProperty<TMaster, CgPolygonMode>(getter); return this; }
        public CgModelVisualElement<TMaster> SetZOffset(Func<TMaster, float> getter) { zOffsetProp = new ProxyProperty<TMaster, float>(getter); return this; }
        public CgModelVisualElement<TMaster> SetHighlightEffect(Func<TMaster, CgHighlightEffect> getter) { highlightEffectProp = new ProxyProperty<TMaster, CgHighlightEffect>(getter); return this; }
        public CgModelVisualElement<TMaster> SetHide(Func<TMaster, bool> getter) { hideProp = new ProxyProperty<TMaster, bool>(getter); return this; }

        public CgModelVisualElement<TMaster> SetGetDistanceToCameraSq(Func<TMaster, Transform, ICamera, float> getDistanceToCameraSq) { GetDistanceToCameraSq = getDistanceToCameraSq; return this; }
    }
}