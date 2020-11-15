Shader "Hidden/Sprite"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		[HideInInspector]_MainTex_TexelSize("TexelSize", Vector) = (0,0,0,0)
		_SpriteRect("SpriteRect", Vector) = (0,0,0,0)
	}
		SubShader
		{
			Cull Off ZWrite Off ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				float4 _SpriteRect;

				fixed4 frag(v2f i) : SV_Target
				{
					float2 c = frac(i.uv);
					i.uv.x = (c.x * _SpriteRect.z + _SpriteRect.x) * _MainTex_TexelSize.x;
					i.uv.y = (c.y * _SpriteRect.w + _SpriteRect.y) * _MainTex_TexelSize.y;
					return tex2D(_MainTex, i.uv);
				}
				ENDCG
			}
		}
}
