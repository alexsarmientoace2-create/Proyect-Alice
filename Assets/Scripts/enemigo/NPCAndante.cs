using UnityEngine;

public class NPCAndante : NPCFather
{
     protected override void UpdatePosition()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
        }


        if (!IsGrounded() || IsInFrontOfWall())
        {
            ChangeDirection();
        }

        rb.linearVelocity = new Vector2(movementSpeed, rb.linearVelocityY);
    }
}
