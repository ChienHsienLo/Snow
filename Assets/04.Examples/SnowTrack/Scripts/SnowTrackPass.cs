using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;


namespace SnowTrack
{
    public class SnowTrackPass : ScriptableRenderPass
    {
        ProfilingSampler sampler;
        SnowTrackSetting setting;

        RTHandle trackRT;
        RTHandle fullTrackRT;
        RTHandle tempRT;
        RTHandle oldTrackRT;
        Material shiftUVMaterial;

        static string tempRTName = "_TempRT";

        static int fullTrackId = Shader.PropertyToID("_FullTrack");
        static int oldTrackId = Shader.PropertyToID("_OldTrack");
        static int newTrackId = Shader.PropertyToID("_NewTrack");

        public SnowTrackPass(SnowTrackSetting setting)
        {
            this.setting = setting;
            shiftUVMaterial = setting.shiftRTMaterial;

            sampler = new ProfilingSampler(this.setting.profilingTag);
        }


        public void Setup(RTHandle trackRT, RTHandle fullTrackRT)
        {
            this.trackRT = trackRT;
            this.fullTrackRT = fullTrackRT;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor descripter = cameraTextureDescriptor;
            descripter.depthBufferBits = trackRT.rt.depth;
            descripter.colorFormat = trackRT.rt.format;
            descripter.width = trackRT.rt.width;
            descripter.height = trackRT.rt.height;
            
            RenderingUtils.ReAllocateIfNeeded(ref tempRT, descripter, wrapMode : TextureWrapMode.Clamp , name: tempRTName);
            RenderingUtils.ReAllocateIfNeeded(ref oldTrackRT, descripter, wrapMode: TextureWrapMode.Clamp, name: tempRTName);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            //CommandBuffer cmd = CommandBufferPool.Get();
            //cmd.Clear();

            //using (new ProfilingScope(cmd, sampler))
            //{
            ////    context.ExecuteCommandBuffer(cmd);
            ////    cmd.Clear();

            ////Blitter.BlitCameraTexture(cmd, trackRT, oldTrackRT);

            ////    //Blitter.BlitCameraTexture(cmd, tempRT, tempRT, shiftUVMaterial, 0);
            ////    Shader.SetGlobalTexture(oldTrackId, oldTrackRT);
            ////    //Shader.SetGlobalTexture(clearId, tempRT);
            ////    Shader.SetGlobalTexture(newTrackId, trackRT);

            ////    //combine pass
            ////    Blitter.BlitCameraTexture(cmd, tempRT, fullTrackRT, shiftUVMaterial, 1);

            ////Shader.SetGlobalTexture(fullTrackId, trackRT);

            //}
            //context.ExecuteCommandBuffer(cmd);

            //CommandBufferPool.Release(cmd);
        }

    }
}
