using UnityEngine;

public class ColorAleatorio : MonoBehaviour
{
    float counter;
    public float timeToChange;
    private SpriteRenderer SpriteRenderer;
    Color color;

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

            color = Color.HSVToRGB(Random.value, 1, 1);

            SpriteRenderer.color = color;
        }

    }
}
