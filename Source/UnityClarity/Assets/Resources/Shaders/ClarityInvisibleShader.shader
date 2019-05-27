// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Clarity/ClarityInvisibleShader"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct VertexIn
            {
            };

            struct V2F
            {
                float4 ClipPosition : SV_POSITION;
            };
            
            V2F vert (VertexIn i)
            {
                V2F o;
                o.ClipPosition = float4(0, 0, 0, 0);
                return o;
            }

            fixed4 frag (V2F i) : SV_Target
            {
                discard;
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
