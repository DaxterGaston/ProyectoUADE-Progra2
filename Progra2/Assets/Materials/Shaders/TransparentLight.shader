Shader "Custom/TransparentLight"
{
    Properties{
        _SpecColor("Specular Color", Color) = (0.25,0.25,0.25,0)
        _Color("Main Color", Color) = (0.5,0.5,0.5,1)
        _Shininess("Shininess", Range(0.01, 1)) = 0.5
        _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
    }
    SubShader{
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        LOD 300

        CGPROGRAM
        /*#pragma surface surf BlinnPhong decal:add nolightmap*/
        /*#pragma surface surf Lambert addlight*/
        /*#pragma surface surf Lambert addshadow*/
        #pragma surface surf Lambert alpha:blend /*addlight*/

        half _Shininess;
        float4 _Color;
        sampler2D _MainTex;

        struct Input {
            float4 color : COLOR;
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            /*fixed4 c = _Color;*/
            o.Albedo = c.rgb * _SpecColor.a;
            o.Gloss = c.rgb;
            o.Specular = c.rgb * _Shininess;
            /*o.Alpha = 0;*/
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Transparent/VertexLit"
}