using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcRenderingInfra : IUcRenderingInfra
    {
        public Shader ClarityStandardShader { get; }
        public Shader ClarityInvisibleShader { get; }

        public UcRenderingInfra()
        {
            ClarityStandardShader = Resources.Load<Shader>("Shaders/ClarityStandardLitShader");
            ClarityInvisibleShader = Resources.Load<Shader>("Shaders/ClarityInvisibleShader");
        }
    }
}