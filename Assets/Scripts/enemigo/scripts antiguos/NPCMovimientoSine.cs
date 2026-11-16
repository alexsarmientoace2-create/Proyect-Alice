using UnityEngine;
using UnityEngine.Rendering;

public class NPCMovimientoSine : MonoBehaviour
{
    public float SpeedX;
    public float SpeedY;
    public float WaveHeight;
    float Ydirection;
    Rigidbody2D rb;
    float counter;
    public float timeToFlip;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        counter =  counter + Time.deltaTime;
        Ydirection = Mathf.Sin(Time.time * SpeedY) * WaveHeight;
        rb.linearVelocity = new Vector2(SpeedX * Time.deltaTime, Ydirection);

        if (counter >= timeToFlip)
        {
            SpeedX = SpeedX * -1;
            counter = 0;
            spriteRenderer.flipX = SpeedX < 0;
        }
    }
}
