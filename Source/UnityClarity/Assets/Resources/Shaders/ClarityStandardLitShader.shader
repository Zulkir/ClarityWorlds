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
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            bool _UseTexture;
            bool _UseNormalMap;
            bool _IgnoreLighting;
            bool _NoSpecular;

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
                    /*
                    if (ScrollingEnabled)
                    {
                        float amountToShow = 0.5;
                        float lowerBorderY = ScrollingAmount - (amountToShow / 2.0);
                        float upperBorderY = ScrollingAmount + (amountToShow / 2.0);
                        float scalingScrollingAmount = 2.0 * (ScrollingAmount - 0.25);
                        float yAtLowerBorder = saturate(scalingScrollingAmount + 0.5 * (lowerBorderY - scalingScrollingAmount));
                        float yAtUpperBorder = saturate(scalingScrollingAmount + 0.5 * (upperBorderY - scalingScrollingAmount));
                        float upperBorderLength = 1.0 - upperBorderY;
                        //float virtualY = -1.0 + 2.0 * v_tex_coord.y;
                        if (v_tex_coord.y < lowerBorderY)
                        {
                            float locAmount = texCoord.y / lowerBorderY;
                            float curvedAmount = sqrt(locAmount);
                            texCoord.x = saturate(0.5 + (v_tex_coord.x - 0.5) * pow(1.0 / curvedAmount, 1.0 / 16.0));
                            texCoord.y = curvedAmount * yAtLowerBorder;

                        }
                        else if (v_tex_coord.y < upperBorderY)
                        {
                            texCoord.y = saturate(scalingScrollingAmount + 0.5 * (v_tex_coord.y - scalingScrollingAmount));
                        }
                        else
                        {
                            float negY = 1.0 - texCoord.y;
                            float negBorderY = 1.0 - upperBorderY;
                            float negYAtBorder = 1.0 - yAtUpperBorder;
                            //texCoord.y = 1.0 - (negY / negBorderY * negYAtBorder);
                            float locAmount = negY / negBorderY;
                            float curvedAmount = sqrt(locAmount);
                            texCoord.x = saturate(0.5 + (v_tex_coord.x - 0.5) * pow(1.0 / curvedAmount, 1.0 / 16.0));
                            texCoord.y = 1.0 - curvedAmount * negYAtBorder;
                        }
                    }
                    */
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

                /*
                if (BlackIsTransparent)
                {
                    preDecorationColor.a = max(max(preDecorationColor.r, preDecorationColor.g), preDecorationColor.b);
                }
                */

                /*
                if (WhiteIsTransparent)
                {
                    preDecorationColor.a = pow(1.0 - min(min(preDecorationColor.r, preDecorationColor.g), preDecorationColor.b), 1 / 4.4);
                }
                */

                /*
                if (IsEdited || IsSelected)
                {
                    float t = (Time + v_tex_coord.x * 2 + v_tex_coord.y * 2) * 3.14;
                    float2 dtdx = float2(dFdx(v_tex_coord));
                    float2 dtdy = float2(dFdy(v_tex_coord));
                    float distanceTX = min(v_tex_coord.x, 1.0 - v_tex_coord.x);
                    float distanceTY = min(v_tex_coord.y, 1.0 - v_tex_coord.y);
                    float2 biDistanceHX = abs(float2(distanceTX / dtdx.x, distanceTX / dtdy.x));
                    float2 biDistanceHY = abs(float2(distanceTY / dtdx.y, distanceTY / dtdy.y));
                    float2 biDistanceH = min(biDistanceHX, biDistanceHY);
                    float distanceH = min(biDistanceH.x, biDistanceH.y);
                    float amount = mix(0.0, 1.0, clamp(1.0 - pow(distanceH / 6, 4), 0.0, 1.0));

                    if (IsEdited)
                    {
                        float decorationMono = sin((v_tex_coord.x + v_tex_coord.y) * 300);
                        float4 decoration = float4(decorationMono, decorationMono, decorationMono, 1.0);
                        out_color = mix(preDecorationColor, decoration, amount);
                    }
                    else if (IsSelected)
                    {
                        float4 decoration = float4(poscos(t - 0.0 * 6.28), poscos(t - 0.33 * 6.28), poscos(t - 0.66 * 6.28), 1.0);
                        out_color = mix(preDecorationColor, decoration, amount);
                    }
                }
                else
                {
                    out_color = preDecorationColor;
                }
                */

                return preDecorationColor;
            }
            ENDCG
        }
    }
}
