Shader "Unlit/TargetTemp"
{
    Properties
    {
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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = fixed4(1,0,0,0);
                fixed dist = length(i.uv - fixed2(0.5, 0.5));
                if(dist > 0.5){
                    clip(-1);
                }
                else if(dist < 0.1) {
                    col.rgb = fixed3(0,0,1);
                }
                else if(dist < 0.3) {
                    col.rgb = fixed3(1,1,0);
                }
                return col;
            }
            ENDCG
        }
    }
}
