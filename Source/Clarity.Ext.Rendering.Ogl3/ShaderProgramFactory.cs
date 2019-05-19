using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Shaders;

namespace Clarity.Ext.Rendering.Ogl3
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

out vec3 v_world_position;
out vec3 v_world_normal;
out vec2 v_tex_coord;

void main()
{
    vec4 worldPosition = vec4(in_position, 1.0f) * World;

    gl_Position = worldPosition * ViewProjection;
    gl_Position.y = -gl_Position.y;
    gl_Position.z = 2.0 * gl_Position.z - gl_Position.w * (1.0 - ZOffset);

    v_world_position = worldPosition.xyz;
    v_world_normal = (vec4(in_normal, 0.0f) * WorldInverseTranspose).xyz;
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
    bool UseTexture;
    bool IgnoreLighting;
    bool IsEdited;
    bool IsSelected;
    bool BlackIsTransparent;
    bool WhiteIsTransparent;
    bool NoSpecular;
    bool ScrollingEnabled;
    float ScrollingAmount;
};

layout(std140) uniform Global
{
    int ScreenWidth;
    int ScreenHeight;
    float Time;
};

uniform sampler2D DiffuseMap;

in vec3 v_world_position;
in vec3 v_world_normal;
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
    vec3 toLight2 = normalize(vec3(0, 0, 50) - v_world_position);
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
    }
    else
    {
        materialColor = Color;
    }

    vec4 preDecorationColor;
    if (IgnoreLighting)
    {
        preDecorationColor = materialColor;
    }
    else
    {
        float diffuseFactor = clamp(1.0 * abs(dot(toLight, normal)) + 0.5 * abs(dot(toLight2, normal)), 0.0f, 1.0f);
        float specularFactor = 0.0;
        if (!NoSpecular)
            specularFactor = pow(clamp(dot(toEye, reflect(-toLight, normal)), 0.0f, 1.0f), 72.0f);
        preDecorationColor = vec4(
                    materialColor.xyz * clamp(diffuseFactor, 0.5f, 1.0f) + 
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

    if (IsEdited || IsSelected)
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
        
        if (IsEdited)
        {
            float decorationMono = sin((v_tex_coord.x + v_tex_coord.y) * 300);
            vec4 decoration = vec4(decorationMono, decorationMono, decorationMono, 1.0);
            out_color = mix(preDecorationColor, decoration, amount);
        }
        else if (IsSelected)
        {
            vec4 decoration = vec4(poscos(t - 0.0 * 6.28), poscos(t - 0.33 * 6.28), poscos(t - 0.66 * 6.28), 1.0);
            out_color = mix(preDecorationColor, decoration, amount);
        }
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
                VertexAttributeNames = new[] {"in_position", "in_normal", "in_tex_coord" },
                UniformBufferNames = new[] {"Transform", "Camera", "CameraExtra", "Light", "Material", "Global"},
                SamplerNames = new[] { "DiffuseMap" }
            });
            return program;
        }
    }
}