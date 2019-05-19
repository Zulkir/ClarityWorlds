Shader "Example/Diffuse Texture" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
    }
        SubShader{
        Tags{ "RenderType" = "Opaque" }
        CGPROGRAM
#pragma surface surf MyDiffuse noambient nodynlightmap 

        struct Input {
        float2 uv_MainTex;
    };

    half4 LightingMyDiffuse(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
    {

        float divisor = max(s.Albedo.r, max(s.Albedo.g, s.Albedo.b));
        s.Albedo /= divisor;

        half4 c;
        float wrap = 0.0f;
        float diffuseFactor = max((dot(s.Normal, lightDir) + wrap) / (1 + wrap), 0);
        float specularIntensity = 0.0f;

        s.Albedo = saturate(s.Albedo);
        c.rgb = (s.Albedo * (diffuseFactor)/2);
        c.a = s.Alpha;

        return c;
    }

    sampler2D _MainTex;
    fixed4 _Color;

    void surf(Input IN, inout SurfaceOutput o) {
        o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
    }
    ENDCG
    }
        Fallback "Diffuse"
}