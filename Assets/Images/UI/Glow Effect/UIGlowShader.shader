Shader "Custom/UIGlowShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _TexScale("TexScale", Float) = 1

        _NoiseTex("Noise Texture", 2D) = "white" {}
        _CornerMask("Corner Mask", 2D) = "black" {}
        _CornerMaskExponent("Corner Mask Exponent", Float) = 1
        _CornerMaskScale("Corner Mask Scale", Float) = 1

        _DistortionDamper("Distortion Damper", Float) = 10
        _DistortionSpread("Distortion Spread", Float) = 100
        _TimeSpeed("Time Speed", Float) = 1

        _NoiseLoop("Noise Loop", Float) = 300

        // for unity UI mask
        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }



        LOD 100
        //ZTest[unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        //ColorMask[_ColorMask]


        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

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
                fixed4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TexScale;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            sampler2D _CornerMask;
            float _CornerMaskExponent;
            float _CornerMaskScale;

            float _DistortionDamper;
            float _DistortionSpread;
            float _TimeSpeed;

            float _NoiseLoop;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 newuv = i.uv;

                float2 pos = float2(abs(i.worldPosition.x), abs(i.worldPosition.y)) / _DistortionSpread;
                float timeAxis = _Time[1] / _TimeSpeed;

                float2 distortion = float2(
                    tex2D(_NoiseTex, float2(pos.y, timeAxis) % _NoiseTex_ST.xy).r,
                    tex2D(_NoiseTex, float2(timeAxis, pos.x) % _NoiseTex_ST.xy).r
                );
                distortion -= 0.5;

                // sample _CornerMask, invert and multiply with the final color
                fixed4 cornerMaskColor = tex2D(_CornerMask, i.uv * _TexScale * _CornerMaskScale + ((1 - (_TexScale * _CornerMaskScale)) / 2));
                float cornerMask = pow(1 - cornerMaskColor.r, _CornerMaskExponent);


                // sample the texture
                fixed4 color = tex2D(_MainTex, ((i.uv + (distortion / _DistortionDamper)) * _TexScale) + ((1 - _TexScale) / 2));

                color.a *= cornerMask;

                return color * i.color;
            }
            ENDCG
        }
    }
}
