 //Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
 //Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Range" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

//Shader "Custom/Circle" {
//	Properties{
//		_Color("Color", Color) = (1, 1, 1, 1)
//		_Width("RoundWidth", float) = 0.03
//	}
//
//	SubShader{
//			Pass{
//				ZTest Off
//				ZWrite Off
//				ColorMask 0
//			}
//
//			Pass{
//					Blend SrcAlpha OneMinusSrcAlpha
//					CGPROGRAM
//
//#pragma vertex vert
//#pragma fragment frag
//#include "UnityCG.cginc"
//
//					struct v2f {
//						float4 pos : SV_POSITION;
//						float4 oPos : TEXCOORD1;
//					};
//					fixed4 _Color;
//					int _Width;
//
//
//
//					float4 _MainTex_ST;
//					v2f vert(appdata_base v){
//						v2f o;
//						o.pos = UnityObjectToClipPos(v.vertex);
//						o.oPos = v.vertex;
//						return o;
//					}
//
//					fixed4 frag(v2f i) : COLOR{
//						float dis = sqrt(i.oPos.x * i.oPos.x + i.oPos.y * i.oPos.y);
//						float maxDistance = 0.05;
//						if (dis > 0.5){
//							discard;
//						}
//						else{
//							float ringWorldRange = unity_ObjectToWorld[0][0];
//							float minDistance = (ringWorldRange * 0.43 - _Width) / ringWorldRange * 0.9;
//							if (dis < minDistance){
//								discard;
//							}
//							_Color.a = (dis - minDistance) / (1 - minDistance) * 0.9;
//						}
//						return _Color;
//					}
//
//						ENDCG
//				}
//		}
//
//		FallBack "Diffuse"
//}
//
