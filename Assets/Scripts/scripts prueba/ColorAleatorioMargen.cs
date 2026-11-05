using System;
using UnityEngine;

public class ColorAleatorioMargen : MonoBehaviour
{
    float counter;
    public float timeToChange;
    private SpriteRenderer SpriteRenderer;
    Color color;
    [Range (0f, 1f)] public float minValue;
    [Range(0f, 1f)] public float maxValue;
    [Range(0f, 1f)] public float minSaturation;
    [Range(0f, 1f)] public float maxSaturation;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    private void Update()
    {
        counter += Time.deltaTime;

        if (counter >= timeToChange) 
        { 
        counter = 0;

            color = Color.HSVToRGB(UnityEngine.Random.value, UnityEngine.Random.Range(minSaturation, maxSaturation) , UnityEngine.Random.Range(minValue, maxValue));

            SpriteRenderer.color = color;
        }

    }
}
