using UnityEngine;

public class NPCVolador : NPCFather
{

    [Header("Flight Settings")]
    public float SpeedY;
    public float WaveHeight;
    float Ydirection;

    protected override void UpdatePosition()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 || IsInFrontOfWall())
        {
            ChangeDirection();
        }

        Ydirection = Mathf.Sin(Time.time * SpeedY) * WaveHeight;
        rb.linearVelocity = new Vector2(movementSpeed * Time.deltaTime, Ydirection);

    }
}
