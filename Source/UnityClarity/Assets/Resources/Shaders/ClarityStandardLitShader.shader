// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Clarity/ClarityStandardLitShader"
{
    Properties
    {
        _CameraPosition ("Camera Position", Vector) = (0, 0, 0, 0)
        _LightPosition ("Light Position", Vector) = (0, 0, 0, 0)

        _Color ("Diffuse Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset] _DiffuseMap ("Texture", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Texture", 2D) = "bump" {}

        _UseTexture ("Use Texture", Int) = 0
        _UseNormalMap("Use Normal Map", Int) = 0
        _IgnoreLighting ("Ignore Lighting", Int) = 0
        _NoSpecular("No Specular", Int) = 0

        _Cull ("Cull Mode", Int) = 2

        _ZWrite ("Z Write", Int) = 1

        _BlendSrc ("Blend Source", Int) = 1
        _BlendDst ("Blend Dest", Int) = 0

        _IsPulsating ("Is Pulsating", Int) = 0
        _PulsatingColor ("Pulsating COlor", Color) = (1, 0.5, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100

        ZWrite [_ZWrite]
        Blend [_BlendSrc] [_BlendDst]

        Pass
        {
            Cull [_Cull]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct VertexIn
            {
                float3 Position : POSITION;
                float3 Normal : NORMAL;
                float3 Tangent : TANGENT;
                float2 TexCoord : TEXCOORD0;
            };

            struct V2F
            {
                float4 ClipPosition : SV_POSITION;
                float3 WorldPosition : TEXTCOORD0;
                float3 WorldNormal : TEXTCOORD1;
                float3 WorldTangent : TEXTCOORD2;
                float2 TexCoord : TEXCOORD3;
            };

            
            
            V2F vert (VertexIn i)
            {
                V2F o;
                float4 worldPosition = mul(UNITY_MATRIX_M, float4(i.Position, 1));

                o.ClipPosition = mul(UNITY_MATRIX_VP, worldPosition);
                o.WorldPosition = worldPosition.xyz;
                o.WorldNormal = UnityObjectToWorldNormal(i.Normal);
                o.WorldTangent = mul(unity_ObjectToWorld, i.Tangent);
                o.TexCoord = i.TexCoord;

                // todo: adjust z and/or y ?

                return o;
            }
            
            float3 _CameraPosition;
            float3 _LightPosition;

            sampler2D _DiffuseMap;
            sampler2D _NormalMap;
            float4 _Color;
            float4 _PulsatingColor;

            bool _UseTexture;
            bool _UseNormalMap;
            bool _IgnoreLighting;
            bool _NoSpecular;
            bool _IsPulsating;

            fixed4 frag (V2F i) : SV_Target
            {
                float3 toEye = normalize(_CameraPosition - i.WorldPosition);
                float3 toLight = normalize(_LightPosition - i.WorldPosition);
                float3 toLight2 = normalize(float3(3, 1, -2));
                float3 toLight3 = -normalize(float3(3, 1, -2));
                float3 normal = normalize(i.WorldNormal);
                float3 tangent = normalize(i.WorldTangent);

                float4 materialColor;
                if (_UseTexture)
                {
                    float2 texCoord = i.TexCoord;
                    materialColor = _Color * tex2D(_DiffuseMap, texCoord);

                    if (_UseNormalMap)
                    {
                        float3 binormal = normalize(cross(normal, tangent));
                        float3 localNormal = 2 * tex2D(_NormalMap, texCoord).xyz - float3(1, 1, 1);
                        float3 newNormal = float3(
                            localNormal.x * tangent.x + localNormal.y * binormal.x + localNormal.z * normal.x,
                            localNormal.x * tangent.y + localNormal.y * binormal.y + localNormal.z * normal.y,
                            localNormal.x * tangent.z + localNormal.y * binormal.z + localNormal.z * normal.z);
                        normal = normalize(newNormal);
                    }
                }
                else
                {
                    materialColor = _Color;
                }

                float4 preDecorationColor;
                if (_IgnoreLighting)
                {
                    preDecorationColor = materialColor;
                }
                else
                {
                    float diffuseFactorRaw = saturate(dot(toLight, normal)) + 0.5 * saturate(dot(toLight2, normal)) + 0.5 * saturate(dot(toLight3, normal));
                    float diffuseFactor = clamp(diffuseFactorRaw, 0.0f, 1.0f);
                    float specularFactor = 0.0;
                    if (!_NoSpecular)
                        specularFactor = pow(clamp(dot(toEye, reflect(-toLight,  normal)), 0.0f, 1.0f), 72.0f);
                    preDecorationColor = float4(
                        materialColor.xyz * clamp(diffuseFactor, 0.5f, 1.0f) +
                        float3(1.0, 1.0, 1.0) * clamp(specularFactor, 0.0, 0.3),
                        materialColor.a);
                }

                if (_IsPulsating)
                {
                    float2 dtdx = float2(0.1, 0.1);
                    float2 dtdy = float2(0.1, 0.1);
                    float distanceTX = min(i.TexCoord.x, 1.0 - i.TexCoord.x);
                    float distanceTY = min(i.TexCoord.y, 1.0 - i.TexCoord.y);
                    float2 biDistanceHX = abs(float2(distanceTX / dtdx.x, distanceTX / dtdy.x));
                    float2 biDistanceHY = abs(float2(distanceTY / dtdx.y, distanceTY / dtdy.y));
                    float2 biDistanceH = min(biDistanceHX, biDistanceHY);
                    float distanceH = min(biDistanceH.x, biDistanceH.y);
                    float amount = saturate((1.0 - distanceH) * (0.8 + 0.4 * cos(_Time.y * 5)));
                    return lerp(preDecorationColor, _PulsatingColor, amount);
                }
                else 
                {
                    return preDecorationColor;
                }
            }
            ENDCG
        }
    }
}
