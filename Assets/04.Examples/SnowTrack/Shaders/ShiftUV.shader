Shader "SnowTrack/ShiftUV"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Name "Clear With Normal Color"
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            uniform float4 _BlitTexture_TexelSize;

            uniform float4 _CameraParams;
            uniform float2 _DeltaPos;
           
            half4 frag (Varyings i) : SV_Target
            {
                return half4(0.5, 0.5, 1.0, 1.0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "Combine Snow Track"
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            uniform float4 _BlitTexture_TexelSize;

            uniform float2 _DeltaPos;
            TEXTURE2D_X(_OldTrack); 
            SAMPLER(sampler_OldTrack);

            TEXTURE2D_X(_NewTrack); 
            SAMPLER(sampler_NewTrack);

            TEXTURE2D_X(_Clear); 
            SAMPLER(sampler_Clear);

            float3 BlendNormals(float3 A, float3 B)
            {
                float3 result = float3(A.xy / A.z + B.xy / B.z, 1.0);

                return result;
            }

            

            half4 frag (Varyings i) : SV_Target
            {
                float2 uv = i.texcoord.xy + _DeltaPos;

                half4 oldColor = SAMPLE_TEXTURE2D(_OldTrack, sampler_OldTrack, uv);
                half4 newColor = SAMPLE_TEXTURE2D(_NewTrack, sampler_NewTrack, i.texcoord.xy);
                
                oldColor.rgb = oldColor.rgb * 2.0 - 1.0;
                newColor.rgb = newColor.rgb * 2.0 - 1.0;
                
                half3 defaultNormal = half3(0.5, 0.5, 1.0) * 2.0 - 1.0;

                half3 color = (oldColor.r * oldColor.g * oldColor.b) > 0.9 ? defaultNormal : oldColor.rgb;
                
                //half4 clear = SAMPLE_TEXTURE2D(_Clear, sampler_Clear, i.texcoord.xy);

                half4 result = half4(normalize(BlendNormals(color, defaultNormal)), 1);
                result.rgb = normalize(BlendNormals(result.rgb, newColor.rgb));

                //result.rgb = track.rgb + color.rgb * (1 - track.a);
                //result.rgb = color.rgb * color.a + track.rgb * (1 - color.a);



                float2 uvEdge = smoothstep(0.9, 1.0, abs(i.texcoord.xy - float2(0.5, 0.5)) * 2);
                float edge = saturate( uvEdge.x + uvEdge.y);
                
                //result.rgb = lerp(result.rgb, half3(0.5, 0.5, 1.0), edge);
                

                result.rgb = result.rgb * 0.5 + 0.5;

                //result.a = color.a * track.a;
                return result;
                //return half4(result.rgb, 1.0);
            }
            ENDHLSL
        }


    }
}
