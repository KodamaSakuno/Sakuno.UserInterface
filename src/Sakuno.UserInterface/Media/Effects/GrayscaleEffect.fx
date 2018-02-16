sampler2D implicitInput : register(S0);

float factor : register(C0);

float4 main(float2 coordinate : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInput, coordinate);
    float gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
    float3 luminance = lerp(color.rgb, gray, factor);

    return float4(luminance, color.a);
}
