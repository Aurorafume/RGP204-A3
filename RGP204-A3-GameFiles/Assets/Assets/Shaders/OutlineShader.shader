Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Float) = 1.0
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
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 posWorld : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 color = tex2D(_MainTex, uv);

                float outline = _OutlineThickness / i.vertex.w;
                float2 offsets[8] = {
                    float2(-outline, -outline), float2(0, -outline), float2(outline, -outline),
                    float2(-outline, 0), /* Center */ float2(outline, 0),
                    float2(-outline, outline), float2(0, outline), float2(outline, outline)
                };

                float4 outlineColor = _OutlineColor;
                for (int j = 0; j < 8; j++)
                {
                    color = max(color, tex2D(_MainTex, uv + offsets[j]));
                }

                return color;
            }
            ENDCG
        }
    }
}