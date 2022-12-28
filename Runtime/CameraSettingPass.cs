using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Naiwen.TAA
{
    internal class CameraSettingPass : ScriptableRenderPass
    {
        TAAData m_TaaData;
        internal CameraSettingPass()
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
            base.profilingSampler = new ProfilingSampler("TAASetCamera");
        }

        internal void Setup(TAAData data)
        {
            m_TaaData = data;
        }

        /// <inheritdoc/>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, profilingSampler))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                ref CameraData cameraData = ref renderingData.cameraData;
                cmd.SetViewProjectionMatrices(cameraData.camera.worldToCameraMatrix, m_TaaData.projOverride);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
