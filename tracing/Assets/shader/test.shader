Shader "sza/test"
{
    Properties
    {
        _MainTex("Texture",2D) = "white"{}
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Lit"
            "IgnoreProjector" = "True"
        }
        LOD 300

        Pass
        {
           Tags
            {
                "LightMode" = "UniversalForward"
            }
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 positionOS : POSITION;
                float4 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normalWS : TEXCOORD1;                                       //�������ռ��·�����Ϣ

            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
            CBUFFER_END


            TEXTURE2D (_MainTex);
            SAMPLER(sampler_MainTex);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS.xyz,true);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                Light mylight = GetMainLight();                                //��ȡ��������Դ
                real4 LightColor = real4(mylight.color,1);                     //��ȡ����Դ����ɫ
                float3 LightDir = normalize(mylight.direction);                //��ȡ���շ���
                float LdotN = dot(LightDir,i.normalWS) * 0.5 + 0.5;                        //LdotN

                half4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv);     //��ͼ�������3������

                return col * LdotN * LightColor;
            }
            ENDHLSL
        }
    }
}
