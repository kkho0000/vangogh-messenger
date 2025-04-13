using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullscreenDissolveFeature : ScriptableRendererFeature
{
    class FullscreenDissolvePass : ScriptableRenderPass
    {
        private Material dissolveMaterial;
        private RenderTargetIdentifier cameraColorTarget;
        private float dissolveProgress = 0f;
        private bool isDissolving = false;
        private bool touchzero = false; // 是否触碰到零点

        public FullscreenDissolvePass(Material material)
        {
            dissolveMaterial = material;
        }

        public void StartDissolve()
        {
            dissolveProgress = 1f;
            isDissolving = true;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!isDissolving) return;

            CommandBuffer cmd = CommandBufferPool.Get("FullscreenDissolve");

            // 更新溶解进度
            if (touchzero)
            {
                dissolveProgress += Time.deltaTime / 2f;
            }
            else
            {
                dissolveProgress -= Time.deltaTime / 1f;
            }
            dissolveMaterial.SetFloat("_Dissolve", dissolveProgress);

            // 渲染全屏溶解效果
            cmd.Blit(null, cameraColorTarget, dissolveMaterial);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            if (dissolveProgress <= 0f && !touchzero)
            {
                touchzero = true; // 触碰到零点
                dissolveMaterial.SetFloat("_Dissolve", 0f); // 设置溶解值为 0
            }

            // 停止溶解效果
            if (dissolveProgress >= 1f)
            {
                isDissolving = false;
            }
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // 清理资源（如果需要）
        }
    }

    public Material dissolveMaterial; // 在 Inspector 中分配溶解材质
    private FullscreenDissolvePass dissolvePass;

    public override void Create()
    {
        dissolvePass = new FullscreenDissolvePass(dissolveMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(dissolvePass);
    }

    public void TriggerDissolve()
    {
        dissolvePass.StartDissolve();
    }
}