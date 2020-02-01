Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_TransparentColor("Transparent Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work

			#include "UnityCG.cginc"

			struct v2f {
				float4 v: SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			v2f vert(float4 v: POSITION, float2 uv : TEXCOORD0)
			{
				v2f result;
				result.v = UnityObjectToClipPos(v);
				result.uv = uv;
				return result;
			}

			fixed4 _Color;
			sampler2D _MainTex;
			fixed4 _TransparentColor;

			fixed4 frag(v2f v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);	

				// Output colour will be the texture color * the vertex colour
				half4 output_col = col * _Color;

				//calculate the difference between the texture color and the transparent color
				//note: we use 'dot' instead of length(transparent_diff) as its faster, and
				//although it'll really give the length squared, its good enough for our purposes!
				half3 transparent_diff = col.xyz - _TransparentColor.xyz;
				half transparent_diff_squared = dot(transparent_diff, transparent_diff);

				//if colour is too close to the transparent one, discard it.
				//note: you could do cleverer things like fade out the alpha
				if (transparent_diff_squared < 0.1)
					discard;

				return output_col;
			}
			ENDCG
		}
    }
}
