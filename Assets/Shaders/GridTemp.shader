Shader "Custom/GridTemp"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0)
        _GridColor ("Grid Color", Color) = (1,1,1,1)
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 _Color;
            fixed4 _GridColor;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = fixed4(1,0,0,0);
                fixed4 c = _Color;
                fixed gridThickness = 0.1;
                fixed3 fracCoords = frac(i.worldPos);
                if(fracCoords.x < gridThickness / 2 || fracCoords.x > 1 - gridThickness / 2) {
                    c = _GridColor;
                }
                if(fracCoords.y < gridThickness / 2 || fracCoords.y > 1 - gridThickness / 2) {
                    c = _GridColor;
                }
                return c;
            }
            ENDCG
        }
    }
}
