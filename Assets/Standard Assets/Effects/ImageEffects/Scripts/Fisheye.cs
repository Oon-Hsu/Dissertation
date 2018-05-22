using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Displacement/Fisheye")]
    public class Fisheye : PostEffectsBase
	{
        public float strengthX = 0.05f; // up/down
        public float strengthY = 0.05f; // left/right
        public float strengthXVariable;
        public float strengthYVariable;
        public bool oscillate;

        public Shader fishEyeShader = null;
        private Material fisheyeMaterial = null;


        public override bool CheckResources ()
		{
            CheckSupport (false);
            fisheyeMaterial = CheckShaderAndCreateMaterial(fishEyeShader,fisheyeMaterial);

            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            if (CheckResources()==false)
			{
                Graphics.Blit (source, destination);
                return;
            }

            float oneOverBaseSize = 80.0f / 512.0f; // to keep values more like in the old version of fisheye

            float ar = (source.width * 1.0f) / (source.height * 1.0f);

            if (oscillate)
            {
                fisheyeMaterial.SetVector("intensity", new Vector4(strengthXVariable * ar * oneOverBaseSize, strengthYVariable * oneOverBaseSize, strengthXVariable * ar * oneOverBaseSize, strengthYVariable * oneOverBaseSize));
            }
            else
            {
                fisheyeMaterial.SetVector("intensity", new Vector4(strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize, strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize));
            }
            Graphics.Blit (source, destination, fisheyeMaterial);
        }

        private void Awake()
        {
            strengthXVariable = strengthX;
            strengthYVariable = strengthY;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                strengthY += 0.02f;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                strengthY -= 0.02f;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                strengthX -= 0.02f;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                strengthX += 0.02f;
            }

            if (oscillate)
            {
                strengthXVariable = Mathf.PingPong(Time.time * 0.1f, strengthX);
                strengthYVariable = Mathf.PingPong(Time.time * 0.2f, strengthY);
            }
        }
    }
}
