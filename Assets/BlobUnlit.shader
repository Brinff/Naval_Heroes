Shader "Unlit/BlobUnlit"
{
    Properties
    {
        _SphA ("Sph A", Vector) = (0,0,0,0)
        _SphB ("Sph B", Vector) = (0,0,0,0)
        _SphC ("Sph C", Vector) = (0,0,0,0)
        _LineEnd ("Line End", Vector) = (0,0,0,0)
        _BlobFactorSph("Blob Factor Sph", Range(0,1)) = 0
        _BlobFactorLine("Blob Factor Line", Range(0,1)) = 0
        _Edge0("Edge 0", Float) = 0
        _Edge1("Edge 1", Float) = 0
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
            #pragma multi_compile_fog

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

            float4 _SphA;
            float4 _SphB;
            float4 _SphC;
            float4 _LineEnd;
            float _BlobFactorSph;
            float _BlobFactorLine;
            float _Edge0;
            float _Edge1;

            inline float smoothUnion(float d1, float d2, float k)
            {
                float h = max(k - abs(d1-d2), 0);
                float s =  min(d1, d2);
                return s - h * h * 0.25 / k;
            }

            inline float line_segment(float2 p, float2 a, float2 b) 
            {
	            float2 ba = b - a;
	            float2 pa = p - a;
	            float h = clamp(dot(pa, ba) / dot(ba, ba), 0., 1.);
	            return length(pa - h * ba);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = 0;
                float shpA = length(i.uv - _SphA.xy)  - _SphA.w;
                float shpB = length(i.uv - _SphB.xy)  - _SphB.w;
                float shpC = length(i.uv - _SphC.xy)  - _SphC.w;

                float l = line_segment(i.uv, _SphA.xy, _SphB.xy) - _LineEnd.w;
                float ab = smoothUnion(shpA, shpB, _BlobFactorSph);
                float lab = smoothUnion(l, ab, _BlobFactorLine);
                float labc = smoothUnion(lab, shpC, _BlobFactorSph);

                float result = smoothstep(_Edge0, _Edge1, labc);



                return result;
            }
            ENDCG
        }
    }
}
