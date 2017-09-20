// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Color Overlay"
{
    Properties
    {
    	_Color ("Overlay Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white"
    }  
 
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }        
             
        Pass
        {  
        	Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
 
            #include "UnityCG.cginc"
 			#include "../Include/PhotoshopBlendModeFunctions.cginc"
 			
            struct appdata_custom
            {
                float4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
            };
           
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
            fixed4 _Color;            
 
            v2f vert (appdata_custom v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }          
 
            fixed4 frag (v2f i) : COLOR
            {
                fixed4 diffuse = tex2D(_MainTex, i.uv);
                fixed oldAlpha = diffuse.a;
				diffuse = Overlay(diffuse, _Color);
                diffuse.a  = oldAlpha * _Color.a;
                return diffuse;
            }
            ENDCG
        }
    }  
    Fallback off
}