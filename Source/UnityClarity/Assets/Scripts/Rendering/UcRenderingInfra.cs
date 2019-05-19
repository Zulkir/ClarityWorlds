using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcRenderingInfra : IUcRenderingInfra
    {
        public Material DefaultLitMaterial { get; }
        public Material DefaultUnlitColMaterial { get; }
        public Material DefaultUnlitTexMaterial { get; }

        public UcRenderingInfra()
        {
            DefaultLitMaterial = Resources.Load<Material>("Materials/DefaultLit");
            DefaultUnlitColMaterial = Resources.Load<Material>("Materials/DefaultUnlitCol");
            DefaultUnlitTexMaterial = Resources.Load<Material>("Materials/DefaultUnlitTex");
        }
    }
}