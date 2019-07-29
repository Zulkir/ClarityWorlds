using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Shaders;

namespace Clarity.Ext.Rendering.Ogl3.Handlers
{
    public class ShaderProgramFactory : IShaderProgramFactory
    {
        private readonly IContext context;

        public ShaderProgramFactory(IContext context)
        {
            this.context = context;
        }

        public IShaderProgram CreateDefault()
        {
            #region Shader Text
            const string vertexShaderText =
@"#version 150

layout(std140) uniform Transform
{
    mat4 World;
    mat4 WorldInverseTranspose;
    float ZOffset;
};

layout(std140) uniform Camera
{
    mat4 ViewProjection;
};

in vec3 in_position;
in vec3 in_normal;
in vec2 in_tex_coord;
in vec3 in_tangent;

out vec3 v_world_position;
out vec3 v_world_normal;
out vec2 v_tex_coord;
out vec3 v_world_tangent;

void main()
{
    vec4 worldPosition = vec4(in_position, 1.0f) * World;

    gl_Position = worldPosition * ViewProjection;
    gl_Position.y = -gl_Position.y;
    gl_Position.z = 2.0 * gl_Position.z - gl_Position.w * (1.0 - ZOffset);

    v_world_position = worldPosition.xyz;
    v_world_normal = (vec4(in_normal, 0.0f) * WorldInverseTranspose).xyz;
    v_world_tangent = (vec4(in_tangent, 0.0f) * World).xyz;
    v_tex_coord = in_tex_coord;
}
";

            const string fragmentShaderText =
@"#version 150

layout(std140) uniform CameraExtra
{
    vec3 CameraPosition;
};

layout(std140) uniform Light
{
    vec3 LightPosition;
};

layout(std140) uniform Material
{
    vec4 Color;
    vec4 PulsatingColor;
    bool UseTexture;
    bool UseNormalMap;
    bool IgnoreLighting;
    bool IsEdited;
    bool IsSelected;
    bool BlackIsTransparent;
    bool WhiteIsTransparent;
    bool NoSpecular;
    bool ScrollingEnabled;
    float ScrollingAmount;
    bool IsPulsating;
};

layout(std140) uniform Global
{
    int ScreenWidth;
    int ScreenHeight;
    float Time;
};

uniform sampler2D DiffuseMap;
uniform sampler2D NormalMap;

in vec3 v_world_position;
in vec3 v_world_normal;
in vec3 v_world_tangent;
in vec2 v_tex_coord;

out vec4 out_color;

float poscos(float x)
{
    return 0.5 * cos(x) + 0.5;
}

float saturate(float x)
{
    return clamp(x, 0.0, 1.0);
}

void main()
{
    vec3 toEye = normalize(CameraPosition - v_world_position);
    vec3 toLight = normalize(LightPosition - v_world_position);
    vec3 toLight2 = normalize(vec3(3, 1, 2));
    vec3 toLight3 = -normalize(vec3(3, 1, 2));
    vec3 tangent = normalize(v_world_tangent);
    vec3 normal = normalize(v_world_normal);

    vec4 materialColor;
    if (UseTexture)
    {
        vec2 texCoord = v_tex_coord;
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
                texCoord.y =  saturate(scalingScrollingAmount + 0.5 * (v_tex_coord.y - scalingScrollingAmount));
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
        materialColor = Color * texture(DiffuseMap, texCoord);

        if (UseNormalMap)
        {
            vec3 binormal = normalize(cross(normal, tangent));
            vec3 localNormal = 2 * texture(NormalMap, texCoord).xyz - vec3(1, 1, 1);
            vec3 newNormal = vec3(
                localNormal.x * tangent.x + localNormal.y * binormal.x + localNormal.z * normal.x,
                localNormal.x * tangent.y + localNormal.y * binormal.y + localNormal.z * normal.y,
                localNormal.x * tangent.z + localNormal.y * binormal.z + localNormal.z * normal.z);
            normal = normalize(newNormal);
        }
    }
    else
    {
        materialColor = Color;
    }

    //out_color = texture(NormalMap, v_tex_coord) + 0.01 * vec4(normal, 0);
    //return;

    vec4 preDecorationColor;
    if (IgnoreLighting)
    {
        preDecorationColor = materialColor;
    }
    else
    {
        float diffuseFactorRaw = saturate(dot(toLight, normal)) + 0.1 * saturate(dot(toLight2, normal)) + 0.1 * saturate(dot(toLight3, normal));
        float diffuseFactor = clamp(diffuseFactorRaw, 0.0f, 1.0f);
        float specularFactor = 0.0;
        if (!NoSpecular)
            specularFactor = pow(clamp(dot(toEye, reflect(-toLight,  normal)), 0.0f, 1.0f), 72.0f);
        preDecorationColor = vec4(
                    materialColor.xyz * clamp(diffuseFactor, 0.1f, 1.0f) + 
                    vec3(1.0, 1.0, 1.0) * clamp(specularFactor, 0.0, 0.3), 
                    materialColor.a);
    }
    
    if (BlackIsTransparent)
    {
        preDecorationColor.a = max(max(preDecorationColor.r, preDecorationColor.g), preDecorationColor.b);
    }
    
    if (WhiteIsTransparent)
    {
        preDecorationColor.a = pow(1.0 - min(min(preDecorationColor.r, preDecorationColor.g), preDecorationColor.b), 1/4.4);
    }

    if (IsPulsating)
    {
        vec2 dtdx = vec2(0.1, 0.1);
        vec2 dtdy = vec2(0.1, 0.1);
        float distanceTX = min(v_tex_coord.x, 1.0 - v_tex_coord.x);
        float distanceTY = min(v_tex_coord.y, 1.0 - v_tex_coord.y);
        vec2 biDistanceHX = abs(vec2(distanceTX / dtdx.x, distanceTX / dtdy.x));
        vec2 biDistanceHY = abs(vec2(distanceTY / dtdx.y, distanceTY / dtdy.y));
        vec2 biDistanceH = min(biDistanceHX, biDistanceHY);
        float distanceH = min(biDistanceH.x, biDistanceH.y);
        float amount = saturate((1.0 - distanceH) * (0.8 + 0.4 * cos(Time * 5)));
        out_color = mix(preDecorationColor, PulsatingColor, amount);
    }
    else if (IsEdited)
    {
        vec2 centeredTexCoord = v_tex_coord * 2.0 - vec2(1.0, 1.0);
        vec2 distancesFromCenter = abs(centeredTexCoord);
        vec2 distancesFromBorders = vec2(1.0, 1.0) - distancesFromCenter;
        float distFromBorder = 1.0 - max(distancesFromCenter.x, distancesFromCenter.y);
        vec2 dtdx = vec2(dFdx(distancesFromBorders));
        vec2 dtdy = vec2(dFdy(distancesFromBorders));
        float rateTx = 0.1 / length(vec2(dtdx.x, dtdy.x));
        float rateTy = 0.1 / length(vec2(dtdx.y, dtdy.y));
        float whiteAmount = mod(floor(distancesFromBorders.x * rateTx) + floor(distancesFromBorders.y * rateTy), 2.0);
        float scaledDistanceFromBorder = min(distancesFromBorders.x * rateTx, distancesFromBorders.y * rateTy) / 2;
        out_color = mix(preDecorationColor, vec4(vec3(1,1,1) * saturate(whiteAmount), 1), 0.5 * (1.0 - round(saturate(scaledDistanceFromBorder))));
    }
    else if (IsSelected)
    {
        float t = (Time + v_tex_coord.x * 2 + v_tex_coord.y * 2) * 3.14 ;
        vec2 dtdx = vec2(dFdx(v_tex_coord));
        vec2 dtdy = vec2(dFdy(v_tex_coord));
        float distanceTX = min(v_tex_coord.x, 1.0 - v_tex_coord.x);
        float distanceTY = min(v_tex_coord.y, 1.0 - v_tex_coord.y);
        vec2 biDistanceHX = abs(vec2(distanceTX / dtdx.x, distanceTX / dtdy.x));      
        vec2 biDistanceHY = abs(vec2(distanceTY / dtdx.y, distanceTY / dtdy.y));      
        vec2 biDistanceH = min(biDistanceHX, biDistanceHY);
        float distanceH = min(biDistanceH.x, biDistanceH.y);        
        float amount = mix(0.0, 1.0, clamp(1.0 - pow(distanceH / 6, 4), 0.0, 1.0));
        // Rainbow:
        vec4 decoration = vec4(poscos(t - 0.0 * 6.28), poscos(t - 0.33 * 6.28), poscos(t - 0.66 * 6.28), 1.0);
        out_color = mix(preDecorationColor, decoration, amount);
    }
    else
    {
        out_color = preDecorationColor;
    }
}
";
            #endregion

            var vs = context.Create.VertexShader(vertexShaderText);
            var fs = context.Create.FragmentShader(fragmentShaderText);
            var program = context.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] {vs},
                FragmentShaders = new[] {fs},
                VertexAttributeNames = new[] {"in_position", "in_normal", "in_tex_coord", "in_tangent" },
                UniformBufferNames = new[] {"Transform", "Camera", "CameraExtra", "Light", "Material", "Global"},
                SamplerNames = new[] { "DiffuseMap", "NormalMap" }
            });
            return program;
        }
    }
}