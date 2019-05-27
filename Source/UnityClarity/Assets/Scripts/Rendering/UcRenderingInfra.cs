using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcRenderingInfra : IUcRenderingInfra
    {
        public Shader ClarityStandardShader { get; }

        public UcRenderingInfra()
        {
            ClarityStandardShader = Resources.Load<Shader>("Shaders/ClarityStandardLitShader");
        }
    }
}