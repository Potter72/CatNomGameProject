using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPass : ScriptableRenderPass
{
    public FilterMode filterMode;
    public Material material;
    public RenderTargetIdentifier source;
    public RenderTargetIdentifier destination;

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("CustomRenderPass");

        // Set the render target and clear it if necessary
        cmd.SetRenderTarget(destination);
        cmd.ClearRenderTarget(true, true, Color.clear);

        // Set your custom material
        cmd.SetGlobalFloat("_FilterMode", (float)filterMode);
        cmd.SetGlobalTexture("_MainTex", source);

        // Access the culling results
        CullingResults cullingResults = renderingData.cullResults;

        // Draw the objects you want to render with your custom shader
        FilteringSettings filterSettings = new FilteringSettings(RenderQueueRange.all, int.MaxValue);
        DrawingSettings drawSettings = CreateDrawingSettings(filterSettings, ref renderingData, ShaderPassName);

        context.DrawRenderers(cullingResults, ref drawSettings, ref filterSettings);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}