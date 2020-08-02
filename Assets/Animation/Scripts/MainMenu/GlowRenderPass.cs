using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowRenderPass : MonoBehaviour
{

    /*
     * Need to be rendered first before the main Scene.
     * To do so attach the script to camera where its z property is < to the main one
    */

    public Shader outlineShader;
    public Shader blurShader;
    public Shader combineGlowShader;

    public bool showPrepass = false;

    public int blurStrength = 3;
    public Color glowColor = new Color(1.0f, 0.8f, 0.3f);

    private static RenderTexture PrePass;
    private static RenderTexture Blurred;
    private static RenderTexture Final;

    private Material _GlowMat;
    private Material _blurMat;
    private Material _CombineGlowMat;
    private int m_blurDownScaleRatio;

    GlowRenderPass()
    {
        m_blurDownScaleRatio = 1;
    }

    void OnEnable()
    {
        PrePass = new RenderTexture(Screen.width, Screen.height, 24);
        Blurred = new RenderTexture(Screen.width / m_blurDownScaleRatio, Screen.height / m_blurDownScaleRatio, 0);

        var camera = GetComponent<Camera>();
        var glowShader = Shader.Find("Hidden/GlowReplace");
        //camera.targetTexture = PrePass;
        //camera.SetReplacementShader(outlineShader, "CanUseSpriteAtlas");

        Shader.SetGlobalTexture("_GlowPrePassTex", PrePass);
        Shader.SetGlobalTexture("_GlowBlurredTex", Blurred);

        //_blurMat = new Material(Shader.Find("Hidden/Blur"));
        //_blurMat.SetVector("_BlurSize", new Vector2(Blurred.texelSize.x * 1.5f, Blurred.texelSize.y * 1.5f));

        _GlowMat = new Material(outlineShader);
        _blurMat = new Material(blurShader);
        _CombineGlowMat = new Material(combineGlowShader);
        //_blurMat.SetFloat("Blur Strength", blurStrength);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //Graphics.Blit(src, dst);

        //Graphics.SetRenderTarget(Blurred);
        //GL.Clear(false, true, Color.clear);

        //Graphics.Blit(src, Blurred);
        //
        //for (int i = 0; i < 1; i++)
        //{
        //    var temp = RenderTexture.GetTemporary(Blurred.width, Blurred.height);
        //    Graphics.Blit(Blurred, temp, _blurMat);
        //    RenderTexture.ReleaseTemporary(temp);
        //}

        //Graphics.Blit(Blurred, dst);
        _blurMat.SetFloat("_Strength", blurStrength);
        _GlowMat.SetTexture("_MainTex", src);
        _GlowMat.SetColor("_Color", glowColor);


        //Graphics.SetRenderTarget(PrePass);
        //GL.Clear(false, true, Color.clear);

        Graphics.Blit(src, PrePass, _GlowMat);
        Graphics.Blit(PrePass, Blurred, _blurMat);

        _CombineGlowMat.SetTexture("_Mask", src);
        _CombineGlowMat.SetTexture("_Source", Blurred);

        Graphics.Blit(Blurred, dst, _CombineGlowMat);
        //if (showPrepass) {
        //    Graphics.Blit(PrePass, dst);
        //} else {
        //    Graphics.Blit(src, dst);
        //}
    }
}
