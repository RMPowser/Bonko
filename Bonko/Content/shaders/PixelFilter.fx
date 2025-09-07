#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 SourceSize; // input texture resolution (width, height)
float2 OutputSize; // render target resolution (width, height)

Texture2D Source;
sampler2D SourceSampler = sampler_state
{
	Texture = <Source>;
};

float4 MainPS(float2 texCoord : TEXCOORD) : COLOR
{
	float2 texelSize = 1 / SourceSize.xy;

	float2 range = float2(
		abs(SourceSize.x / (OutputSize.x * SourceSize.x)),
		abs(SourceSize.y / (OutputSize.y * SourceSize.y))
	);
	
	range = range / 2.0 * 0.999;

	float left   = texCoord.x - range.x;
	float top    = texCoord.y + range.y;
	float right  = texCoord.x + range.x;
	float bottom = texCoord.y - range.y;
	
	float3 topLeftColor     = tex2D(SourceSampler, (floor(float2(left, top)     / texelSize) + 0.5 ) * texelSize).rgb;
	float3 bottomRightColor = tex2D(SourceSampler, (floor(float2(right, bottom) / texelSize) + 0.5 ) * texelSize).rgb;
	float3 bottomLeftColor  = tex2D(SourceSampler, (floor(float2(left, bottom)  / texelSize) + 0.5 ) * texelSize).rgb;
	float3 topRightColor	= tex2D(SourceSampler, (floor(float2(right, top)	/ texelSize) + 0.5 ) * texelSize).rgb;

	float2 border = clamp(
		round(texCoord / texelSize) * texelSize,
		float2(left, bottom),
		float2(right, top)
	);

	float totalArea = 4.0 * range.x * range.y;

	float3 averageColor;
	averageColor = ((border.x - left)  * (top - border.y)    / totalArea) * topLeftColor;
	averageColor += ((right - border.x) * (border.y - bottom) / totalArea) * bottomRightColor;
	averageColor += ((border.x - left)  * (border.y - bottom) / totalArea) * bottomLeftColor;
	averageColor += ((right - border.x) * (top - border.y)    / totalArea) * topRightColor;
	
	return float4(averageColor, 1.0);
}

technique Pixellate
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
}