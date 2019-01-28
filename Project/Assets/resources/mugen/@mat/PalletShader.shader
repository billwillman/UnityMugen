Shader "Mugen/PalletShader"
{
	Properties
	{
		_MainTex ("Index Texture", 2D) = "black" {}
		_PalletTex("Pallet Texture", 2D) = "black" {}
		_Color("Mix Color", Color) = (1,1,1,1)
	}
	SubShader
	{
	
		LOD 200
		
		Tags
		{
			"Queue"="Transparent" 
        	"IgnoreProjector"="True" 
        	"RenderType"="Transparent" 
        	//"RenderType"="Opaque"
        	"PreviewType"="Plane"
        	"CanUseSpriteAtlas"="True"
		}
		

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
		//	Blend One Zero
				
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _PalletTex;
			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				
				//half2 uv = float2(round(col.a * 255)/255, 0);
				half2 uv = float2(col.a, 0);
				col = tex2D(_PalletTex, uv);
				/*
				float d = col.r + col.g + col.b;
				if (d == 0)
					//col.a = 0;
					col.a = 1.0f;
				*/
				col *= i.color;
				
				return col;
			}
			ENDCG
		}
	}
}
