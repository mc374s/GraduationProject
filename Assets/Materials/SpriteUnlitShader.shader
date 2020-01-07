Shader "Unlit/SpriteUnlitShader"
{
    Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
        _SubTex("Sub Texture", 2D) = "white" {}

        [Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("Blend Src", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("Blend Dst", Float) = 10
            
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)]_ZTestMode("ZTest Mode", Float) = 3
        [Toggle]_ZWriteParam("ZWrite", Float) = 0
        [KeywordEnum(None, Additive, Multiply, Replace)]_Overlay("Overlay mode", Float) = 2
        //_TintColor("Tint Color", Color) = (1,1,1,1)
        _InvertThreshold("Invert Threshold", Range(0.0, 1.0)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend [_BlendSrc] [_BlendDst]
        Cull [_CullMode]
        ZTest [_ZTestMode]
        ZWrite [_ZWriteParam]
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uvSub : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SubTex;
            float4 _SubTex_ST;
            //float4 _TintColor;
            float _InvertThreshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uvSub = TRANSFORM_TEX(v.uv, _SubTex);
                o.uvSub = v.uv * _SubTex_ST.xy + _SubTex_ST.zw;
                o.color = v.color;
                return o;
            }

            #pragma multi_compile _OVERLAY_NONE _OVERLAY_ADDITIVE _OVERLAY_MULTIPLY _OVERLAY_REPLACE

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * tex2D(_SubTex, i.uvSub);

                #ifdef _OVERLAY_ADDITIVE
                col.rgb += i.color.rgb;
                #elif _OVERLAY_MULTIPLY
                col.rgb *= i.color.rgb;
                #elif _OVERLAY_REPLACE
                col.rgb = i.color.rgb;
                #endif

                // float speed = 10;
                // _TintColor.a = (sin(_Time.w* speed) + 1.0) * 0.5;

                col.rgb = abs(_InvertThreshold - col.rgb);
                col.a *= i.color.a;

                return col;
            }
            ENDCG
        }
    }
}
