using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public interface IUcRenderingInfra
    {
        // todo: move to global objects
        Shader ClarityStandardShader { get; }
        Shader ClarityInvisibleShader { get; }
    }
}