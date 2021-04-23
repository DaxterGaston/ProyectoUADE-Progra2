Shader "Custom/IgnoreLight" {
    Properties{
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _BumpMap("Normalmap", 2D) = "bump" {}
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 300

        CGPROGRAM
        #pragma surface surf LambertCustom

        sampler2D _MainTex;
        sampler2D _BumpMap;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
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
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        }
        ENDCG
    }

        FallBack "Diffuse"
}