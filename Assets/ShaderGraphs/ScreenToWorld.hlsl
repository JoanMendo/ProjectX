#ifndef SCREEN_TO_WORLD_INCLUDED
#define SCREEN_TO_WORLD_INCLUDED

void ScreenToPoint_float(float4 screenPoint, float4x4 projectionMatrixInverted, float4x4 viewMatrixInverse, out float3 WorldPosition)
{
    // Convert x and y from normalized [0,1] to clip space [-1,1]
    float2 clipXY = screenPoint.xy * 2.0 - 1.0;

    // Reconstruct the clip space position
    float4 clipPos = float4(clipXY, screenPoint.z, screenPoint.w);

    // Transform from clip space to view space
    float4 viewPos = mul(projectionMatrixInverted, clipPos);
    viewPos /= viewPos.w;

    // Transform from view space to world space
    float4 worldPos = mul(viewMatrixInverse, viewPos);

    // Return the world position as float3
    WorldPosition = worldPos.xyz;
}

#endif // SCREEN_TO_WORLD_INCLUDED