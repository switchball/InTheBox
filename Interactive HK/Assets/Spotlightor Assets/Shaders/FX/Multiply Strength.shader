// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "FX/Multiply Strength" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_Strength ("Strength", Range(0,1)) = 1
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float _Strength;
						
		struct v2f {
			half4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
		};

		v2f vert(appdata_full v) {
			v2f o;
			
			o.pos = UnityObjectToClipPos (v.vertex);	
			o.uv.xy = v.texcoord.xy;
					
			return o; 
		}
		
		fixed4 frag( v2f i ) : COLOR {	
			fixed4 result = tex2D (_MainTex, i.uv.xy);
			result = lerp(fixed4(1,1,1,0), result, _Strength);
			return result;
		}
	
	ENDCG
	
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Cull Off
		Lighting Off
		ZWrite Off
		Blend Zero SrcColor
		Fog { Color (1,1,1,1) }
	Pass {
	
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		 
		}
				
	} 
	FallBack Off
}
