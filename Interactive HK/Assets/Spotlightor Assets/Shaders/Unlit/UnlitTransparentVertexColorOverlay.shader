// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Vertex Colored Overlay"
{
    Properties
    {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
    }
   
    SubShader
    {
    	LOD 100
    
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
                float4 color : COLOR;
            };
           
            struct v2f
            {
                float4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
           
            sampler2D _MainTex;
            fixed4 _MainTex_ST;
           
            v2f vert (appdata_custom v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv,_MainTex);
                o.color = v.color;
                return o;
            }          
           
            fixed4 frag (v2f i) : COLOR
            {
				fixed4 diffuse = tex2D(_MainTex, i.uv);
                fixed oldAlpha = diffuse.a;
				diffuse = Overlay(diffuse, i.color);
                diffuse.a  = oldAlpha * i.color.a;
                return diffuse;
            }
            ENDCG
        }
    }
    Fallback off
}