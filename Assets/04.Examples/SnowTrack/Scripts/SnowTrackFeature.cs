using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SnowTrack
{
    public class SnowTrackFeature : ScriptableRendererFeature
    {
        [SerializeField] Settings renderObjectSettings = new Settings();
        [SerializeField] SnowTrackSetting snowTrackSetting = new SnowTrackSetting();


        SnowTrackPass snowTrackPass;
        RenderObjectsToTexturePass renderObjectsToTexturePass;

        RTHandle trackRTHandle;
        RTHandle fullTrackRTHandle;
        static string fullTrackRTName = "FullTrackRT";
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(renderObjectsToTexturePass);
            renderer.EnqueuePass(snowTrackPass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = renderObjectSettings.depthBufferBits;
            descriptor.colorFormat = renderObjectSettings.colorFormat;
            descriptor.width = snowTrackSetting.trailRTSize;
            descriptor.height = snowTrackSetting.trailRTSize;
            
            RenderingUtils.ReAllocateIfNeeded(ref trackRTHandle, descriptor, wrapMode : TextureWrapMode.Clamp, name : snowTrackSetting.trackRTName);
            RenderingUtils.ReAllocateIfNeeded(ref fullTrackRTHandle, descriptor, wrapMode: TextureWrapMode.Clamp, name: fullTrackRTName);
            
            renderObjectsToTexturePass.Setup(trackRTHandle);
            snowTrackPass.Setup(trackRTHandle, fullTrackRTHandle);
        }


        public override void Create()
        {
            renderObjectsToTexturePass = new RenderObjectsToTexturePass(renderObjectSettings);
            snowTrackPass = new SnowTrackPass(snowTrackSetting);
        }

     
    }

    [System.Serializable]
    public class SnowTrackSetting
    {
        public int trailRTSize = 1024;
        public string trackRTName = "_TrackTexture";
        public string profilingTag = "Snow Track";

        public Material shiftRTMaterial;


    }


}
