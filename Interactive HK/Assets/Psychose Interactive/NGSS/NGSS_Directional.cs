using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Light))]
[ExecuteInEditMode()]
public class NGSS_Directional : MonoBehaviour
{
    [Tooltip("Optimize shadows performance by skipping fragments that are 100% lit or 100% shadowed. Some tiny noisy artefacts can be seen if shadows are too soft.")]
    public bool EARLY_BAILOUT_OPTIMIZATION = true;
    [Tooltip("Help with bias problems but can leads to some noisy artefacts if Early Bailout Optimization is enabled.\nRequires PCSS in order to work.")]
    public bool USE_BIAS_FADE = false;
    [Tooltip("Provides Area Light like soft-shadows. With shadows being harder at close ranges and softer at long ranges.\nDisable it if you are looking for uniformly simple soft-shadows.")]
    public bool PCSS_ENABLED = true;
    private bool PCSS_SWITCH = true;
    [Tooltip("Overall softness for both PCF and PCSS shadows.\nRecommended value: 0.01.")]
    [Range(0f, 0.02f)]
    public float PCSS_GLOBAL_SOFTNESS = 0.01f;
    [Tooltip("PCSS softness when shadows is close to caster.\nRecommended value: 0.05.")]
    [Range(0f, 1f)]
    public float PCSS_FILTER_DIR_MIN = 0.05f;
    [Tooltip("PCSS softness when shadows is far from caster.\nRecommended value: 0.25.\nIf too high can lead to visible artifacts when early bailout is enabled.")]
    [Range(0f, 0.5f)]
    public float PCSS_FILTER_DIR_MAX = 0.25f;

    [Tooltip("Amount of banding or noise. Example: 0.0 gives 100 % Banding and 10.0 gives 100 % Noise.")]
    [Range(0f, 10f)]
    public float BANDING_NOISE_AMOUNT = 1f;
    
    public enum SAMPLER_COUNT { SAMPLERS_16, SAMPLERS_25, SAMPLERS_32, SAMPLERS_64 }
    [Tooltip("Recommended values: Mobile = 16, Consoles = 25, Desktop Low = 32, Desktop High = 64")]
    public SAMPLER_COUNT SAMPLERS_COUNT = SAMPLER_COUNT.SAMPLERS_64;

    //public Texture noiseTexture;
    private bool isInitialized = false;
    private bool isGraphicSet = false;

    //#if UNITY_5_4 || UNITY_5_5 || UNITY_5_6
    private Light m_Light;
    private CommandBuffer rawShadowDepthCB;
    private RenderTexture m_ShadowmapCopy;

    void OnDestroy()
    {
        if (isGraphicSet)
        {
            isGraphicSet = false;
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
        }

        RemoveCommandBuffers();
    }
    void OnDisable()
    {
        if (isGraphicSet)
        {
            isGraphicSet = false;
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
        }

        RemoveCommandBuffers();
    }

    void OnApplicationQuit()
    {
        if (isGraphicSet)
        {
            isGraphicSet = false;
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/Internal-ScreenSpaceShadows"));
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseBuiltin);
        }

        RemoveCommandBuffers();
    }

    void RemoveCommandBuffers()
    {
        if (isInitialized == false) { return; }
        
        m_Light.RemoveCommandBuffer(LightEvent.AfterShadowMap, rawShadowDepthCB);
        m_ShadowmapCopy = null;

        isInitialized = false;
    }

    void OnEnable()
    {
        Init();
    }

    //[MenuItem("NGSS/Setup Directional")]
    void Init()
    {
        //if (noiseTexture == null) { enabled = false; return; }
        if (isInitialized) { return; }

        if (isGraphicSet == false)
        {
            isGraphicSet = true;
            //If CloseFit is forced cascades must be disabled or it wil produce unwanted behavior with Cascade blending
            //QualitySettings.shadowProjection = ShadowProjection.StableFit;
            //QualitySettings.shadowCascades = 4;
            //QualitySettings.shadowCascade4Split = new Vector3(0.1f, 0.275f, 0.5f);
            GraphicsSettings.SetShaderMode(BuiltinShaderType.ScreenSpaceShadows, BuiltinShaderMode.UseCustom);
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, Shader.Find("Hidden/NGSS_Directional"));//Shader.Find can sometimes return null in Player builds (careful).
        }
        
        if(PCSS_ENABLED == false) { return; }

        m_Light = GetComponent<Light>();
        int shadowResolution = QualitySettings.shadowResolution == ShadowResolution.VeryHigh ? 4096 : QualitySettings.shadowResolution == ShadowResolution.High ? 2048 : QualitySettings.shadowResolution == ShadowResolution.Medium ? 1024 : 512;
        //RenderTargetIdentifier shadowmap = BuiltinRenderTextureType.CurrentActive;
        m_ShadowmapCopy = null;
        m_ShadowmapCopy = new RenderTexture(shadowResolution, shadowResolution, 0, RenderTextureFormat.RFloat);
        m_ShadowmapCopy.filterMode = FilterMode.Bilinear;
        //m_ShadowmapCopy.anisoLevel = 10;
        m_ShadowmapCopy.useMipMap = false;

        rawShadowDepthCB = new CommandBuffer { name = "NGSS Directional PCSS buffer" };
        rawShadowDepthCB.Clear();

        // Change shadow sampling mode for m_Light's shadowmap.
        rawShadowDepthCB.SetShadowSamplingMode(BuiltinRenderTextureType.CurrentActive, ShadowSamplingMode.RawDepth);

        // The shadowmap values can now be sampled normally - copy it to a different render texture.
        rawShadowDepthCB.Blit(BuiltinRenderTextureType.CurrentActive, m_ShadowmapCopy);

        rawShadowDepthCB.SetGlobalTexture("NGSS_DirectionalRawDepth", m_ShadowmapCopy);
        //rawShadowDepthCB.SetGlobalTexture("NGSS_NoiseTexture", noiseTexture);

        // Execute after the shadowmap has been filled.
        foreach (CommandBuffer cb in m_Light.GetCommandBuffers(LightEvent.AfterShadowMap))
        { if (cb.name == rawShadowDepthCB.name) { isInitialized = true; return; } }
        
        m_Light.AddCommandBuffer(LightEvent.AfterShadowMap, rawShadowDepthCB);

        isInitialized = true;

        // Sampling mode is restored automatically after this command buffer completes, so shadows will render normally.
    }

    void Update()
    {
        if(PCSS_ENABLED != PCSS_SWITCH) { PCSS_SWITCH = !PCSS_SWITCH; if (PCSS_ENABLED) { isInitialized = false; Init(); } else { RemoveCommandBuffers(); } }
        SetGlobalSettings();
    }

    void SetGlobalSettings()
    {
        Shader.SetGlobalFloat("NGSS_PCSS_GLOBAL_SOFTNESS", PCSS_GLOBAL_SOFTNESS);
        Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MIN", PCSS_FILTER_DIR_MIN > PCSS_FILTER_DIR_MAX? PCSS_FILTER_DIR_MAX : PCSS_FILTER_DIR_MIN);
        Shader.SetGlobalFloat("NGSS_PCSS_FILTER_DIR_MAX", PCSS_FILTER_DIR_MAX < PCSS_FILTER_DIR_MIN ? PCSS_FILTER_DIR_MIN : PCSS_FILTER_DIR_MAX);
        Shader.SetGlobalFloat("NGSS_POISSON_SAMPLING_NOISE_DIR", BANDING_NOISE_AMOUNT);
        if (PCSS_ENABLED) { Shader.EnableKeyword("NGSS_PCSS_FILTER_DIR"); } else { Shader.DisableKeyword("NGSS_PCSS_FILTER_DIR"); }
        if (EARLY_BAILOUT_OPTIMIZATION) { Shader.EnableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION_DIR"); } else { Shader.DisableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION_DIR"); }         
        if (USE_BIAS_FADE) { Shader.EnableKeyword("NGSS_USE_BIAS_FADE_DIR"); } else { Shader.DisableKeyword("NGSS_USE_BIAS_FADE_DIR"); }

        Shader.DisableKeyword("DIR_POISSON_64"); Shader.DisableKeyword("DIR_POISSON_32"); Shader.DisableKeyword("DIR_POISSON_25"); Shader.DisableKeyword("DIR_POISSON_16");
        Shader.EnableKeyword(SAMPLERS_COUNT == SAMPLER_COUNT.SAMPLERS_64? "DIR_POISSON_64" : SAMPLERS_COUNT == SAMPLER_COUNT.SAMPLERS_32 ? "DIR_POISSON_32" : SAMPLERS_COUNT == SAMPLER_COUNT.SAMPLERS_25 ? "DIR_POISSON_25" : "DIR_POISSON_16");
        /*
        //LOCAL LIGHTS TEST
        if (PCSS_ENABLED) { Shader.EnableKeyword("NGSS_PCSS_FILTER"); } else { Shader.DisableKeyword("NGSS_PCSS_FILTER"); }
        if (EARLY_BAILOUT_OPTIMIZATION) { Shader.EnableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION"); } else { Shader.DisableKeyword("NGSS_USE_EARLY_BAILOUT_OPTIMIZATION"); }

        Shader.DisableKeyword("POISSON_64"); Shader.DisableKeyword("POISSON_32"); Shader.DisableKeyword("POISSON_25"); Shader.DisableKeyword("POISSON_16");
        Shader.EnableKeyword(SAMPLERS_COUNT == SAMPLER_COUNT.SAMPLERS_64 ? "POISSON_64" : SAMPLERS_COUNT == SAMPLER_COUNT.SAMPLERS_32 ? "POISSON_32" : SAMPLERS_COUNT == SAMPLER_COUNT.SAMPLERS_32 ? "POISSON_25" : "POISSON_16");
        */
    }
}