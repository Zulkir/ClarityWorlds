using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public interface IUcRenderingInfra
    {
        Material DefaultLitMaterial { get; }
        Material DefaultUnlitColMaterial { get; }
        Material DefaultUnlitTexMaterial { get; }
    }
}