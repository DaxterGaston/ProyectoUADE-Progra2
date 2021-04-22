Shader "Custom/StencilMaskWithEmission"
{
    Properties{
         _SpecColor("Specular Color", Color) = (0.25,0.25,0.25,0)
         _Color("Main Color", Color) = (0.5,0.5,0.5,1)
         _Shininess("Shininess", Range(0.01, 1)) = 0.5
    }
    SubShader{
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        LOD 300

        CGPROGRAM
        #pragma surface surf BlinnPhong decal:add nolightmap

        half _Shininess;
        float4 _Color;

        struct Input {
            float4 color : COLOR;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = _Color;
            o.Albedo = c.rgb * _SpecColor.a;
            o.Gloss = c.rgb;
            o.Specular = c.rgb * _Shininess;
            o.Alpha = 0;
        }
        ENDCG
}
    FallBack "Transparent/VertexLit"
}
