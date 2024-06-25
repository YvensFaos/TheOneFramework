Shader "Unlit/ProximityTriggerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 100)) = 0
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }        
        LOD 100

        Pass
        {
            Cull Off
            Lighting Off
            ZTest Off  
            Blend One OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_ST;
            float _Progress;

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

            #if ETC1_EXTERNAL_ALPHA
                fixed4 alpha = tex2D (_AlphaTex, uv);
                color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
            #endif

                return color;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = SampleSpriteTexture (i.uv);
                col.rgb *= col.a;

                if (col.r > col.g + col.b) {

                    // Calculate distance from the center of the texture
                    float2 center = float2(0.5, 0.5);
                    float distance = length(i.uv - center);

                    // Calculate progress angle based on _Progress parameter
                    float progressAngle = _Progress * 3.6; // Convert _Progress to angle (0 to 360)

                    // Calculate angle between the current pixel and the top center of the circle
                    float angle = atan2(i.uv.x - center.x, i.uv.y - center.y);
                    angle = degrees(angle);
                    if (angle < 0) angle += 360;

                    // Fill the circle based on the progress
                    if (angle < progressAngle)
                    {
                        // Fill the pixel with w
                        fixed i = col.r;
                        col.rgb = float3(i, i, i);
                    }
                    else
                    {
                        // Leave the pixel as is
                        col.rgb = float3(0, 0, 0);
                    }
                }                
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
// {
//     Properties
//     {
//         _Progress ("Progress", Range(0, 100)) = 0
//         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
//         _Color ("Tint", Color) = (1,1,1,1)
//         [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
//         [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
//         [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
//         [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
//         [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
//     }

//     SubShader
//     {
//         Tags
//         {
//             "Queue"="Transparent"
//             "IgnoreProjector"="True"
//             "RenderType"="Transparent"
//             "PreviewType"="Plane"
//             "CanUseSpritAtlas"="True"
//         }

//         Cull Off
//         Lighting Off
//         ZWrite Off
//         Blend One OneMinusSrcAlpha

//         Pass
//         {
//         CGPROGRAM
//             float _Progress;

//             #pragma vertex SpriteVert2
//             #pragma fragment SpriteFrag2
//             #pragma target 2.0
//             #pragma multi_compile_instancing
//             #pragma multi_compile_local _ PIXELSNAP_ON
//             #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
//             #include "UnitySprites.cginc"

//             v2f SpriteVert2(appdata_t IN)
//             {
//                 v2f OUT;
            
//                 UNITY_SETUP_INSTANCE_ID (IN);
//                 UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
            
//                 OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
//                 OUT.vertex = UnityObjectToClipPos(OUT.vertex);
//                 OUT.texcoord = IN.texcoord;
//                 OUT.color = IN.color * _Color * _RendererColor;
            
//                 #ifdef PIXELSNAP_ON
//                 OUT.vertex = UnityPixelSnap (OUT.vertex);
//                 #endif

//                 OUT.vertex.z = 0.1;
            
//                 return OUT;
//             }
//             fixed4 SpriteFrag2(v2f IN) : SV_Target
//             {
//                 fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

//                 if (c.r > 0.01 && c.r > c.g + c.b)
//                 {
//                     if (_Progress > c.r * 100.0) {
//                         c.rgb = float3(1, 1, 1);
//                     } else {
//                         c.rgb = float3(0, 0, 0);
//                     }
//                 }

//                 c.rgb *= c.a;
//                 return c;
//             }            
//         ENDCG
//         }
//     }
// }

