using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    public enum WaveForm { sin, tri, sqr, saw, inverseSaw, noise };
    public WaveForm waveform = WaveForm.sin;

    public float baseStart = 0.0f;
    public float amplitude = 1.0f;
    public float phase = 0.0f;
    public float frequency = 0.5f;

    private Color originalColor;
    private new Light light;

	// Use this for initialization
	private void Start ()
    {
        light = GetComponent<Light>();
        originalColor = light.color;	
	}
	
	// Update is called once per frame
	private void Update ()
    {
        light.color = originalColor * (EvalWave());	
	}

    private float EvalWave ()
    {
        float x = (Time.time + phase) * frequency;
        float y;
        x = x - Mathf.Floor(x);

        switch (waveform)
        {
            case WaveForm.sin:
                y = Mathf.Sin(x * 2 * Mathf.PI);
                break;

            case WaveForm.tri:
                if (x < 0.5f) y = 4.0f * x - 1.0f;
                else y = -4.0f * x + 3.0f;
                break;

            case WaveForm.sqr:
                if (x < 0.5f) y = 1.0f;
                else y = -1.0f;
                break;

            case WaveForm.saw:
                y = x;
                break;

            case WaveForm.inverseSaw:
                y = 1.0f - x;
                break;

            case WaveForm.noise:
                y = 1.0f - (Random.value * 2);
                break;

            default:
                y = 1.0f;
                break;
        }
        return (y * amplitude) + baseStart; 
    }
}
