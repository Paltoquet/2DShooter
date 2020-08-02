using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowRenderPrePass : MonoBehaviour
{

    private static RenderTexture PrePass;

    void OnEnable()
    {
        Shader.SetGlobalTexture("_GlowMainTex", PrePass);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, PrePass);
        Graphics.Blit(src, dst);
    }
}
