using System.Collections;
using UnityEngine;

public abstract class NPCFather : MonoBehaviour
{

    [Header("Movement")]
    public float movementSpeed;
    protected bool canMove = true;
    public float timeToChange;
    protected float timer;
    protected float changeDetectorX;

    [Header("Raycast settings")]
    [SerializeField] protected GameObject changeDetector;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float groundRayLenght = 1;
    [SerializeField] protected float wallRayLenght = 1;
    [SerializeField] protected Vector2 pushDirection;
    protected Vector2 currentWallRayDirection = Vector2.right;


    [Header("Damage System")]
    public float currentHealth = 2;
    public GameObject dropItem;

    [Header("Components")]
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

     public virtual void Awake()
    {
        timer = timeToChange;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeDetectorX = changeDetector.transform.localPosition.x;
    }



    protected virtual void FixedUpdate()
    {
        if (canMove)
        {
            UpdatePosition();
        }
    }

    protected abstract void UpdatePosition();

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("hitter"))
        {
            Hit(other.transform);
        }
    }

    protected virtual void Hit(Transform otherTransform)
    {
        currentHealth = currentHealth - 1;
        BePushed(otherTransform);
        if (currentHealth <= 0) animator.SetTrigger("Death");
    }


    protected virtual void BePushed(Transform otherTransform)
    {
        if (currentHealth > 0)
        {
            StartCoroutine(WaitToMove());

            Vector2 pushVector = rb.position - new Vector2(otherTransform.position.x, otherTransform.position.y);

            if ((pushVector.x < 0 && pushDirection.x > 0) || (pushVector.x > 0 && pushDirection.x < 0))
            {
                pushDirection.x *= -1;
            }

            rb.AddForce(pushDirection, ForceMode2D.Impulse);
        }
    }

    protected virtual IEnumerator WaitToMove()
    {
        canMove = false;
        while (rb.linearVelocity.magnitude > 0.2f)
        {
            yield return null;
        }
        canMove = true;
    }

    protected virtual void ChangeDirection()
    {
        timer = timeToChange;
        movementSpeed *= -1;
        spriteRenderer.flipX = movementSpeed < 0;


        currentWallRayDirection = movementSpeed > 0 ? Vector2.right : Vector2.left;


        changeDetectorX = -changeDetectorX;
        changeDetector.transform.localPosition = new Vector3(changeDetectorX, changeDetector.transform.localPosition.y, 0);
    }



    #region rays
    protected bool IsGrounded()
    {
        return Physics2D.Raycast(changeDetector.transform.position, Vector2.down, groundRayLenght, groundLayer);
    }

    protected bool IsInFrontOfWall()
    {
        return Physics2D.Raycast(changeDetector.transform.position, currentWallRayDirection, wallRayLenght, groundLayer);
    }
    protected void OnDrawGizmos()
    {
        Debug.DrawRay(changeDetector.transform.position, Vector2.down * groundRayLenght, Color.red);
        Debug.DrawRay(changeDetector.transform.position, currentWallRayDirection * wallRayLenght, Color.blue);
    }

    #endregion


    //lo llamo desde el animator
    public virtual void Destroy()
    {
        Instantiate(dropItem, this.transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
        Destroy(this.gameObject);

    }
}
