// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/HSV Unlit Transparent" {

    Properties {
   		_Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _HueShift("HueShift", Float ) = 0
        _Sat("Saturation", Float) = 1
        _Val("Value", Float) = 1
    }

    SubShader {
 		Tags {Queue=Transparent}
        //Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType" = "Transparent" }
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        Lighting Off
        Pass
        {
            CGPROGRAM
			// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
			#pragma exclude_renderers gles
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
			//#pragma surface surf Lambert
			
            #include "UnityCG.cginc"

            float3 shift_col(float3 RGB, float3 shift)
            {
	            float3 RESULT = float3(RGB);
	            float VSU = shift.z*shift.y*cos(shift.x*3.14159265/180);
	                float VSW = shift.z*shift.y*sin(shift.x*3.14159265/180);
	
	                RESULT.x = (.299*shift.z+.701*VSU+.168*VSW)*RGB.x
	                        + (.587*shift.z-.587*VSU+.330*VSW)*RGB.y
	                        + (.114*shift.z-.114*VSU-.497*VSW)*RGB.z;
	                
	                RESULT.y = (.299*shift.z-.299*VSU-.328*VSW)*RGB.x
	                        + (.587*shift.z+.413*VSU+.035*VSW)*RGB.y
	                        + (.114*shift.z-.114*VSU+.292*VSW)*RGB.z;
	                
	                RESULT.z = (.299*shift.z-.3*VSU+1.25*VSW)*RGB.x
	                        + (.587*shift.z-.588*VSU-1.05*VSW)*RGB.y
	                        + (.114*shift.z+.886*VSU-.203*VSW)*RGB.z;
	                
	            return (RESULT);
            }

            struct v2f {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
 
            float4 _MainTex_ST;
            fixed4 _Color;// by lancelot

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }
 
            sampler2D _MainTex;
            float _HueShift;
            float _Sat;
            float _Val;
 
            half4 frag(v2f i) : COLOR
            {
                half4 col = tex2D(_MainTex, i.uv);
                float3 shift = float3(_HueShift, _Sat, _Val);
                
                //half4( half3(shift_col(col, shift)), col.a);
                return half4( half3(shift_col(col, shift)), col.a) * _Color;// by lancelot
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}