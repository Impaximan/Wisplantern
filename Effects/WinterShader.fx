sampler uImage0 : register(s0); // The contents of the screen
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float4 uShaderSpecificData;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageSize4;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float newColor = (color.r - 0.12 + color.g - 0.12 + color.b - 0.12) / 3;
    return lerp(color, float4(newColor, newColor * 1.1, newColor * 1.1, newColor), uOpacity * 0.3);
}

technique Technique1
{
    pass Winter
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}