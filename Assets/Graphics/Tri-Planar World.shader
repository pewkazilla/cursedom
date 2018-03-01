Shader "Tri-Planar World" {
  Properties {
  		_SideColor ("Side Color", Color) = (1,1,1,1)
  		_TopColor ("Top Color", Color) = (1,1,1,1)
  		_BottomColor ("Bottom Color", Color) = (1,1,1,1)
		_Side("Side", 2D) = "white" {}
		_Top("Top", 2D) = "white" {}
		_Bottom("Bottom", 2D) = "white" {}
		_SideScale("Side Scale", Float) = 2
		_TopScale("Top Scale", Float) = 2
		_BottomScale ("Bottom Scale", Float) = 2
	}
	
	SubShader {
		Tags {
			"Queue"="Geometry"
			"IgnoreProjector"="False"
			"RenderType"="Opaque"
		}

		Cull Back
		ZWrite On
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma exclude_renderers flash

		sampler2D _Side, _Top, _Bottom;
		float _SideScale, _TopScale, _BottomScale;
		fixed4 _SideColor, _TopColor, _BottomColor;

		struct Input {
			float3 worldPos;
			float3 worldNormal;
		};



		void surf (Input IN, inout SurfaceOutput o) {
			float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));
			
			// SIDE X
			float3 x = (tex2D(_Side, frac(IN.worldPos.zy * _SideScale)) * _SideColor) * abs(IN.worldNormal.x);
			
			// TOP / BOTTOM
			float3 y = 0;
			if (IN.worldNormal.y > 0) {
				y = (tex2D(_Top, frac(IN.worldPos.zx * _TopScale)) * _TopColor) * abs(IN.worldNormal.y);
			} else {
				y = (tex2D(_Bottom, frac(IN.worldPos.zx * _BottomScale)) * _BottomColor) * abs(IN.worldNormal.y);
			}
			
			// SIDE Z	
			float3 z = (tex2D(_Side, frac(IN.worldPos.xy * _SideScale)) * _SideColor) * abs(IN.worldNormal.z);
			
			o.Albedo = z;
			o.Albedo = lerp(o.Albedo, x, projNormal.x);
			o.Albedo = lerp(o.Albedo, y, projNormal.y);
		} 
		ENDCG
	}
	Fallback "Diffuse"
}