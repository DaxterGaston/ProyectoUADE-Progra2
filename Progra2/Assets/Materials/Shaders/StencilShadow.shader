Shader "Custom/StencilShadow"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Pixels("Pixels", Float) = 512
        _Ratio("Ratio", Vector) = (9,16,0,0)
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 200
            Blend SrcAlpha OneMinusSrcAlpha
            Stencil{
                Ref 1
                /*Comp equal */
            }
            Pass{

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            /*#pragma surface surf Standard fullforwardshadows*/

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                /*UNITY_FOG_COORDS(1)*/
                    float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Pixels;
            float4 _Ratio;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                /*o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);*/
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dx = _Ratio.x * (1 / _Pixels);
                float dy = _Ratio.y * (1 / _Pixels);
                float2 Coord = float2(dx * floor(i.uv.x / dx), dy * floor(i.uv.y / dy));
                // sample the texture
                fixed4 col = tex2D(_MainTex, Coord);
                // apply fog
                /*UNITY_APPLY_FOG(i.fogCoord, col);*/
                return col;
            }
            ENDCG
        }
    }
}