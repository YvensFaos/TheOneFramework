Shader "Example/FaceMappingURP"
{
    Properties
    { 
        _Top("Top", 2D) = "white"
        _Bottom("Bottom", 2D) = "white"
        _Left("Left", 2D) = "white"
        _Right("Right", 2D) = "white"
        _Front("Front", 2D) = "white"
        _Back("Back", 2D) = "white"
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 dominantAxis : TEXCOORD1;
            };

            TEXTURE2D(_Top);
            SAMPLER(sampler_Top);

            TEXTURE2D(_Bottom);
            SAMPLER(sampler_Bottom);

            TEXTURE2D(_Front);
            SAMPLER(sampler_Front);

            TEXTURE2D(_Back);
            SAMPLER(sampler_Back);

            TEXTURE2D(_Left);
            SAMPLER(sampler_Left);

            TEXTURE2D(_Right);
            SAMPLER(sampler_Right);

            CBUFFER_START(UnityPerMaterial)
                float4 _Top_ST;
                float4 _Bottom_ST;
                float4 _Left_ST;
                float4 _Right_ST;
                float4 _Front_ST;
                float4 _Back_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                
                // Determine the dominant axis and perform the UV transformation
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                if (abs(normalWS.x) > abs(normalWS.y) && abs(normalWS.x) > abs(normalWS.z))
                {
                    OUT.dominantAxis = float3(sign(normalWS.x), 0.0, 0.0); // x-axis dominant
                    OUT.uv = (sign(normalWS.x) > 0) ? TRANSFORM_TEX(IN.uv, _Right) : TRANSFORM_TEX(IN.uv, _Left);
                }
                else if (abs(normalWS.y) > abs(normalWS.x) && abs(normalWS.y) > abs(normalWS.z))
                {
                    OUT.dominantAxis = float3(0.0, sign(normalWS.y), 0.0); // y-axis dominant
                    OUT.uv = (sign(normalWS.y) > 0) ? TRANSFORM_TEX(IN.uv, _Top) : TRANSFORM_TEX(IN.uv, _Bottom);
                }
                else
                {
                    OUT.dominantAxis = float3(0.0, 0.0, sign(normalWS.z)); // z-axis dominant
                    OUT.uv = (sign(normalWS.z) > 0) ? TRANSFORM_TEX(IN.uv, _Front) : TRANSFORM_TEX(IN.uv, _Back);
                }
            
                return OUT;
            }
            

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color;
                float2 transformedUV;

                // Use the dominant axis to select the appropriate texture and UV transformation
                if (IN.dominantAxis.x != 0)
                {
                    if (IN.dominantAxis.x > 0)
                        transformedUV = TRANSFORM_TEX(IN.uv, _Right);
                    else
                        transformedUV = TRANSFORM_TEX(IN.uv, _Left);
                }
                else if (IN.dominantAxis.y != 0)
                {
                    if (IN.dominantAxis.y > 0)
                        transformedUV = TRANSFORM_TEX(IN.uv, _Top);
                    else
                        transformedUV = TRANSFORM_TEX(IN.uv, _Bottom);
                }
                else
                {
                    if (IN.dominantAxis.z > 0)
                        transformedUV = TRANSFORM_TEX(IN.uv, _Front);
                    else
                        transformedUV = TRANSFORM_TEX(IN.uv, _Back);
                }

                // Sample the correct texture using the transformed UVs
                if (IN.dominantAxis.x != 0)
                {
                    if (IN.dominantAxis.x > 0)
                        color = SAMPLE_TEXTURE2D(_Right, sampler_Right, transformedUV);
                    else
                        color = SAMPLE_TEXTURE2D(_Left, sampler_Left, transformedUV);
                }
                else if (IN.dominantAxis.y != 0)
                {
                    if (IN.dominantAxis.y > 0)
                        color = SAMPLE_TEXTURE2D(_Top, sampler_Top, transformedUV);
                    else
                        color = SAMPLE_TEXTURE2D(_Bottom, sampler_Bottom, transformedUV);
                }
                else
                {
                    if (IN.dominantAxis.z > 0)
                        color = SAMPLE_TEXTURE2D(_Front, sampler_Front, transformedUV);
                    else
                        color = SAMPLE_TEXTURE2D(_Back, sampler_Back, transformedUV);
                }

                return color;
            }
            ENDHLSL
        }
    }
}
