using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCAndante : NPCFather
{
    [SerializeField] Vector2 throwDirection;
    [SerializeField] GameObject Rock;
    [SerializeField] Transform rockSpawner;
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

    void ThrowRock()
    {
        if (Rock == null) return;

       
        GameObject rockInstance = Instantiate(Rock, rockSpawner.position, Quaternion.identity);

        if (rockInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rockRb))
        {
            rockRb.AddForce(throwDirection, ForceMode2D.Impulse);
        }
    }

    protected override void ChangeDirection()
    {
        base.ChangeDirection();
        throwDirection = new Vector2(throwDirection.x * -1, throwDirection.y);

    }
}

