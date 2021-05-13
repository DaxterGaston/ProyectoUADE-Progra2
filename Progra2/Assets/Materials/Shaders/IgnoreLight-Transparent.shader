Shader "Custom/IgnoreLight-Transparent" {
        Properties
        {
            _Color("Color", Color) = (1,1,1,1)
            _MainTex("Albedo (RGB)", 2D) = "white" {}
        }

        SubShader{
            Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
            LOD 300
            Pass {
                 ColorMask 0
             }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

        CGPROGRAM
        #pragma surface surf LambertCustom

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
        };

        half4 LightingLambertCustom(SurfaceOutput s, half3 lightDir, half atten) {
            s.Normal = normalize(s.Normal);

            half diff;
            if (_WorldSpaceLightPos0.w != 0.0)
                diff = max(0, dot(s.Normal, normalize(lightDir))) * _LightColor0;
            else
                diff = 1.0;

            half4 c;
            c.rgb = (s.Albedo * diff) * atten;
            c.a = s.Alpha;
            return c;
       }

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            /*o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));*/
        }
        ENDCG
    }

        FallBack "Standard"
}