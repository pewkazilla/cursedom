Shader "HueShift/Tri-Planar World" {
  Properties {
		_Side("Side", 2D) = "white" {}
		_Top("Top", 2D) = "white" {}
		_Bottom("Bottom", 2D) = "white" {}
		_SideScale("Side Scale", Float) = 2
		_TopScale("Top Scale", Float) = 2
		_BottomScale ("Bottom Scale", Float) = 2
		_SideHueShift("Side Hue Shift", Float) = 0
      	_SideSaturationShift("Side Saturation Shift", Float) = 1.0
        _TopHueShift("Top Hue Shift", Float) = 0
      	_TopSaturationShift("Top Saturation Shift", Float) = 1.0
        _BottomHueShift("Bottom Hue Shift", Float) = 0
      	_BottomSaturationShift("Bottom Saturation Shift", Float) = 1.0
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
		float _SideHueShift,_TopHueShift,_BottomHueShift;
      	float _SideSaturationShift,_TopSaturationShift,_BottomSaturationShift;
     
		 float3 rgb_to_hsv_no_clip(float3 RGB)
        {
                float3 HSV;
           
         float minChannel, maxChannel;
         if (RGB.x > RGB.y) {
          maxChannel = RGB.x;
          minChannel = RGB.y;
         }
         else {
          maxChannel = RGB.y;
          minChannel = RGB.x;
         }
         
         if (RGB.z > maxChannel) maxChannel = RGB.z;
         if (RGB.z < minChannel) minChannel = RGB.z;
           
                HSV.xy = 0;
                HSV.z = maxChannel;
                float delta = maxChannel - minChannel;             //Delta RGB value
                if (delta != 0) {                    // If gray, leave H  S at zero
                   HSV.y = delta / HSV.z;
                   float3 delRGB;
                   delRGB = (HSV.zzz - RGB + 3*delta) / (6.0*delta);
                   if      ( RGB.x == HSV.z ) HSV.x = delRGB.z - delRGB.y;
                   else if ( RGB.y == HSV.z ) HSV.x = ( 1.0/3.0) + delRGB.x - delRGB.z;
                   else if ( RGB.z == HSV.z ) HSV.x = ( 2.0/3.0) + delRGB.y - delRGB.x;
                }
                return (HSV);
        }
 
        float3 hsv_to_rgb(float3 HSV)
        {
                float3 RGB = HSV.z;
           
                   float var_h = HSV.x * 6;
                   float var_i = floor(var_h);  
                   float var_1 = HSV.z * (1.0 - HSV.y);
                   float var_2 = HSV.z * (1.0 - HSV.y * (var_h-var_i));
                   float var_3 = HSV.z * (1.0 - HSV.y * (1-(var_h-var_i)));
                   if      (var_i == 0) { RGB = float3(HSV.z, var_3, var_1); }
                   else if (var_i == 1) { RGB = float3(var_2, HSV.z, var_1); }
                   else if (var_i == 2) { RGB = float3(var_1, HSV.z, var_3); }
                   else if (var_i == 3) { RGB = float3(var_1, var_2, HSV.z); }
                   else if (var_i == 4) { RGB = float3(var_3, var_1, HSV.z); }
                   else                 { RGB = float3(HSV.z, var_1, var_2); }
           
           return (RGB);
        }


		struct Input {
			float3 worldPos;
			float3 worldNormal;
		};
			
		void surf (Input IN, inout SurfaceOutput o) {
			float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));
			
			// SIDE X
			float3 x = tex2D(_Side, frac(IN.worldPos.zy * _SideScale)) * abs(IN.worldNormal.x);
			float3 hsv = rgb_to_hsv_no_clip(x);
          	hsv.x+=_SideHueShift;
          	hsv.y = _SideSaturationShift;
         
          	if ( hsv.x > 1.0 ) { hsv.x -= 1.0; }
          	x = half3(hsv_to_rgb(hsv))* ((x.r + x.g + x.b)/3);

			// TOP / BOTTOM
			float3 y = 0;
			if (IN.worldNormal.y > 0) {
				y = tex2D(_Top, frac(IN.worldPos.zx * _TopScale)) * abs(IN.worldNormal.y);
				hsv = rgb_to_hsv_no_clip(y);
	          	hsv.x+=_TopHueShift;
	          	hsv.y = _TopSaturationShift;
	         
	          	if ( hsv.x > 1.0 ) { hsv.x -= 1.0; }
	          	y = half3(hsv_to_rgb(hsv))* ((y.r + y.g + y.b)/3);
			} else {
				y = tex2D(_Bottom, frac(IN.worldPos.zx * _BottomScale)) * abs(IN.worldNormal.y);
				hsv = rgb_to_hsv_no_clip(y);
	          	hsv.x+=_BottomHueShift;
	          	hsv.y = _BottomSaturationShift;
	         
	          	if ( hsv.x > 1.0 ) { hsv.x -= 1.0; }
	          	y = half3(hsv_to_rgb(hsv))* ((y.r + y.g + y.b)/3);
			}
			
			// SIDE Z	
			float3 z = tex2D(_Side, frac(IN.worldPos.xy * _SideScale)) * abs(IN.worldNormal.z);

          	hsv = rgb_to_hsv_no_clip(z);
          	hsv.x+=_SideHueShift;
          	hsv.y = _SideSaturationShift;
         
          	if ( hsv.x > 1.0 ) { hsv.x -= 1.0; }
          	z = half3(hsv_to_rgb(hsv))* ((z.r + z.g + z.b)/3);


			o.Albedo = z;
			o.Albedo = lerp(o.Albedo, x, projNormal.x);
			o.Albedo = lerp(o.Albedo, y, projNormal.y);
		} 
		ENDCG
	}
	Fallback "Diffuse"
}