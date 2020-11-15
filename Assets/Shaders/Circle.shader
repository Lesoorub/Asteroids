Shader "Hidden/Circle"
{
	Properties
	{
		_MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		_Tickness("Tickness", Float) = 0.05
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		// No culling or depth
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
			float _Tickness;

			fixed4 frag(v2f i) : SV_Target
			{
			fixed4 col = tex2D(_MainTex, i.uv);
			float x = i.uv.x - 0.5;
			float y = i.uv.y - 0.5;
			float r = 0.5;
			if (x * x + y * y > r * r) {
				col.rgba = 0;
			}
			else  if (x * x + y * y < (r - _Tickness) * (r - _Tickness)){
					col.rgba = 0;
			}
			return col;
			}
			ENDCG
		}
	}
}
