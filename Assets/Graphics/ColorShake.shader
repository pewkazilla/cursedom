Shader "SHELL/Hidden/ColorShake"
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RedOffset("RedOffset", Vector) = (0.0, 0.0, 0.0, 0.0)
        _BlueOffset("BlueOffset", Vector) = (0.0, 0.0, 0.0, 0.0)
        _GreenOffset("GreenOffset", Vector) = (0.0, 0.0, 0.0, 0.0)
    }

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off
                
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            half4 _RedOffset;
            half4 _BlueOffset;
            half4 _GreenOffset;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed a = tex2D(_MainTex, i.uv).a;
                fixed r = tex2D(_MainTex, i.uv + _RedOffset.xy).r;
                fixed g = tex2D(_MainTex, i.uv + _GreenOffset.xy).g;
                fixed b = tex2D(_MainTex, i.uv + _BlueOffset.xy).b;

                return fixed4(r, g, b, a);
            }
            ENDCG

        }
    }

    Fallback off
}