using System;
using Clarity.Common.CodingUtilities.Patterns;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Media.Models.Explicit.Embedded;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.Engine.Visualization.Elements
{
    public static class ModelVisualElement
    {
        public static IModel3D DefaultModel { get; } = CubeModelFactory.GenerateNew().WithSource(x => new GeneratedResourceSource(x, typeof(IModel3D)));
        public static ModelVisualElement<object> New() => new ModelVisualElement<object>(null);
        public static ModelVisualElement<TMaster> New<TMaster>(TMaster master) => new ModelVisualElement<TMaster>(master);
    }

    public class ModelVisualElement<TMaster> : IModelVisualElement
    {
        private readonly TMaster master;

        private ProxyProperty<TMaster, bool> hideProp;
        private ProxyProperty<TMaster, IModel3D> modelProp;
        private ProxyProperty<TMaster, int> modelPartIndexProp;
        private ProxyProperty<TMaster, IMaterial> materialProp;
        private ProxyProperty<TMaster, IRenderState> renderStateProp;
        private ProxyProperty<TMaster, Transform> transformProp;
        private ProxyProperty<TMaster, Vector3> nonUniformScaleProp;
        private ProxyProperty<TMaster, TransformSpace> transformSpaceProp;

        public StandardVisualGroup VisualGroup { get; private set; }
        public Func<TMaster, Transform, ICamera, float> GetDistanceToCameraSq { get; set; }
        public float DistanceToCameraSq(Transform elemTransform, ICamera camera) => GetDistanceToCameraSq(master, elemTransform, camera);

        public IModel3D Model => modelProp.GetValue(master);
        public int ModelPartIndex => modelPartIndexProp.GetValue(master);
        public IMaterial Material => materialProp.GetValue(master);
        public IRenderState RenderState => renderStateProp.GetValue(master);
        public Transform Transform => transformProp.GetValue(master);
        public Vector3 NonUniformScale => nonUniformScaleProp.GetValue(master);
        public TransformSpace TransformSpace => transformSpaceProp.GetValue(master);
        public bool Hide => hideProp.GetValue(master);

        public ModelVisualElement(TMaster master)
        {
            this.master = master;
            SetModel(ModelVisualElement.DefaultModel);
            SetMaterial(StandardMaterial.Default);
            SetRenderState(StandardRenderState.Default);
            SetTransform(Transform.Identity);
            SetNonUniformScale(new Vector3(1, 1, 1));
            GetDistanceToCameraSq = (m, t, c) => (t.Offset - c.GetEye()).LengthSquared();
        }

        public ModelVisualElementImmutabilityFlags GetImmutability()
        {
            var result = ModelVisualElementImmutabilityFlags.None;
            if (modelProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.Model;
            if (modelPartIndexProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.ModelPartIndex;
            if (materialProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.Material;
            if (renderStateProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.RenderState;
            if (transformProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.Transform;
            if (nonUniformScaleProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.NonUniformTransform;
            if (transformSpaceProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.TransformSpace;
            if (hideProp.IsImmutable)
                result |= ModelVisualElementImmutabilityFlags.Hide;
            return result;
        }

        public ModelVisualElement<TMaster> SetBasicVisualGroup(StandardVisualGroup visualGroup) { VisualGroup = visualGroup; return this; }
        public ModelVisualElement<TMaster> SetModel(IModel3D immutableValue) { modelProp = new ProxyProperty<TMaster, IModel3D>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetModelPartIndex(int immutableValue) { modelPartIndexProp = new ProxyProperty<TMaster, int>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetMaterial(IMaterial immutableValue) { materialProp = new ProxyProperty<TMaster, IMaterial>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetRenderState(IRenderState immutableValue) { renderStateProp = new ProxyProperty<TMaster, IRenderState>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetTransform(Transform immutableValue) { transformProp = new ProxyProperty<TMaster, Transform>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetNonUniformScale(Vector3 immutableValue) { nonUniformScaleProp = new ProxyProperty<TMaster, Vector3>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetTransformSpace(TransformSpace immutableValue) { transformSpaceProp = new ProxyProperty<TMaster, TransformSpace>(immutableValue); return this; }
        public ModelVisualElement<TMaster> SetHide(bool immutableValue) { hideProp = new ProxyProperty<TMaster, bool>(immutableValue); return this; }

        public ModelVisualElement<TMaster> SetModel(Func<TMaster, IModel3D> getter) { modelProp = new ProxyProperty<TMaster, IModel3D>(getter); return this; }
        public ModelVisualElement<TMaster> SetModelPartIndex(Func<TMaster, int> getter) { modelPartIndexProp = new ProxyProperty<TMaster, int>(getter); return this; }
        public ModelVisualElement<TMaster> SetMaterial(Func<TMaster, IMaterial> getter) { materialProp = new ProxyProperty<TMaster, IMaterial>(getter); return this; }
        public ModelVisualElement<TMaster> SetRenderState(Func<TMaster, IRenderState> getter) { renderStateProp = new ProxyProperty<TMaster, IRenderState>(getter); return this; }
        public ModelVisualElement<TMaster> SetTransform(Func<TMaster, Transform> getter) { transformProp = new ProxyProperty<TMaster, Transform>(getter); return this; }
        public ModelVisualElement<TMaster> SetNonUniformScale(Func<TMaster, Vector3> getter) { nonUniformScaleProp = new ProxyProperty<TMaster, Vector3>(getter); return this; }
        public ModelVisualElement<TMaster> SetTransformSpace(Func<TMaster, TransformSpace> getter) { transformSpaceProp = new ProxyProperty<TMaster, TransformSpace>(getter); return this; }
        public ModelVisualElement<TMaster> SetHide(Func<TMaster, bool> getter) { hideProp = new ProxyProperty<TMaster, bool>(getter); return this; }

        public ModelVisualElement<TMaster> SetGetDistanceToCameraSq(Func<TMaster, Transform, ICamera, float> getDistanceToCameraSq) { GetDistanceToCameraSq = getDistanceToCameraSq; return this; }
    }
}