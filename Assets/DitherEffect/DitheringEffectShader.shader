// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Maya Dithering Shader" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_NoiseTex ("Base (RGB)", 2D) = "gray" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off Fog { Mode off }

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct v2f { 
	float4 pos	: SV_POSITION;
	float2 uv	: TEXCOORD0;
	float2 uvn	: TEXCOORD1;
};

uniform sampler2D _MainTex;
uniform float4 _MainTex_TexelSize;
uniform sampler2D _NoiseTex;
uniform float4 _NoiseTex_TexelSize;

uniform fixed _NoiseRange;
uniform int _Steps;
uniform half _EffectStrength;

half3 rgb2hsv(half3 c)
{
    half4 K = half4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    half4 p = lerp(half4(c.bg, K.wz), half4(c.gb, K.xy), step(c.b, c.g));
    half4 q = lerp(half4(p.xyw, c.r), half4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return half3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

half3 hsv2rgb(half3 c)
{
    half4 K = half4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    half3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
}

v2f vert (appdata_full v)
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv = MultiplyUV (UNITY_MATRIX_TEXTURE0, v.texcoord);
	o.uvn = MultiplyUV (UNITY_MATRIX_TEXTURE1, v.texcoord);
    o.uvn *= _MainTex_TexelSize.zw * _NoiseTex_TexelSize.xy;
	return o;
}

fixed4 frag (v2f i) : SV_Target
{
	fixed3 original = tex2D(_MainTex, i.uv);
    fixed3 col = rgb2hsv(original);
	
	// sample noise texture and do a signed add
	fixed3 noise = tex2D(_NoiseTex, i.uvn).rrr;
	col += noise * _NoiseRange;
	col = floor(col * _Steps) / _Steps;

    col = hsv2rgb(col);

    // lerp based on effect strength
    col = lerp(original, col, _EffectStrength);

	return fixed4(col, 1);
}
ENDCG
	}
}

Fallback off

}