using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class DitheringEffect : MonoBehaviour
{
    class Cache
    {
        public Shader ditherEffectShader;
        public Material noiseMaterial;

        public int noiseTexID;
        public int noiseBaseID;
        public int noiseRangeID;

        public Cache(DitheringEffect context)
        {
            ditherEffectShader = Shader.Find("Hidden/Maya Dithering Shader");
            if (ditherEffectShader == null || !ditherEffectShader.isSupported)
            {
                return;
            }

            noiseMaterial = new Material(ditherEffectShader);

            noiseTexID = Shader.PropertyToID("_NoiseTex");
            noiseTexID = Shader.PropertyToID("_NoiseBase");
            noiseTexID = Shader.PropertyToID("_NoiseRange");
        }
    }

    Cache _cache;
    Cache cache
    {
        get
        {
            if (_cache == null)
            {
                _cache = new Cache(this);
            }
            return _cache;
        }
    }

	public int baseViewHeight = 240;
	public bool disableDithering = true;
	public int ditherColourLevels = 8;
	public Texture2D ditherMatrixTexture;
    [Range(0, 1)]
    public float effectStrength = 1;
	
	protected void Update ()
	{
		//Check non-negative size
		baseViewHeight = Mathf.Max (1, baseViewHeight);

		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects) {
			Debug.Log( "Image effects are not supported! Disabling noise effect." );
			enabled = false;
			return;
		}

		if (!disableDithering)
		{
            if(cache.noiseMaterial == null)
			{
				disableDithering = true;
                _cache = null;
				return;
			}
		}
	}

	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
        float aspect = Screen.width / (float)Screen.height;
        int screenWidth = (int)(aspect * baseViewHeight);

        var midRT = RenderTexture.GetTemporary(screenWidth, baseViewHeight, 0);
        midRT.filterMode = FilterMode.Point;

		if (disableDithering)
		{
			Graphics.Blit(source, midRT);
			Graphics.Blit(midRT, destination);
		}
		else
		{
            Material mat = cache.noiseMaterial;
            mat.SetTexture("_NoiseTex", ditherMatrixTexture);

			float stepSize = 1f / ditherColourLevels;
			mat.SetInt("_Steps", ditherColourLevels);

			float noiseRange = stepSize * 0.5f;
			float noiseBase = (stepSize - noiseRange) * 0.5f;
            mat.SetFloat("_NoiseBase", noiseBase);
            mat.SetFloat("_NoiseRange", stepSize);
            mat.SetFloat("_EffectStrength", effectStrength);

            var ditherRT = RenderTexture.GetTemporary(screenWidth, baseViewHeight, 0);
            ditherRT.filterMode = FilterMode.Point;

            Graphics.Blit(source, midRT);
            Graphics.Blit(midRT, ditherRT, mat);
            Graphics.Blit(ditherRT, destination);

            ditherRT.Release();
		}

        midRT.Release();
	}

	void OnPostRender()
	{

	}
}
