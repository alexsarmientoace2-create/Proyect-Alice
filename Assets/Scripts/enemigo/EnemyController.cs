using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class EnemyController: MonoBehaviour
{
    #region Atributes
    public float movementSpeed;
    Rigidbody2D rb;
    public float timeToChange;
    float timer;
    SpriteRenderer spriteRenderer;
    Collider2D col;
    #endregion
    Animator animator;
    [SerializeField] private GameObject changeDetector;
    float changeDetectorX;
    [Header("Raycast settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLenght = 1;
    [SerializeField] private float wallRayLenght = 1;
    [SerializeField] Vector2 damageRecoil;
    private Vector2 currentWallRayDirection = Vector2.right;
    #region Unity Methods
    public float currentHealth = 2;
    bool canMove = true;
    public GameObject coin;


    void Awake()
    {
        timer = timeToChange;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();   
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        changeDetectorX = changeDetector.transform.localPosition.x;

    }

    private void FixedUpdate()
    {
      if (canMove)
        {
            UpdatePosition();
        }
    }

    void UpdatePosition()
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
    private void OnDrawGizmos()
    {
        Debug.DrawRay(changeDetector.transform.position, Vector2.down * groundRayLenght, Color.red);
        Debug.DrawRay(changeDetector.transform.position, currentWallRayDirection * wallRayLenght, Color.blue);
    }

    #endregion

    #region My Methods

    void ChangeDirection()
    {
        timer = timeToChange; 
        movementSpeed *= -1;
        spriteRenderer.flipX = movementSpeed < 0;


        currentWallRayDirection = movementSpeed > 0 ? Vector2.right : Vector2.left;

  
        changeDetectorX = -changeDetectorX;
        changeDetector.transform.localPosition = new Vector3(changeDetectorX, changeDetector.transform.localPosition.y, 0);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(changeDetector.transform.position, Vector2.down, groundRayLenght, groundLayer);
    }

    private bool IsInFrontOfWall()
    {
        return Physics2D.Raycast(changeDetector.transform.position, currentWallRayDirection, wallRayLenght, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("hitter"))
        {
            currentHealth = currentHealth - 1;
            if (currentHealth > 0)
            {
                StartCoroutine(WaitToMove());
                Vector2 recoilVector = rb.position - new Vector2(other.transform.position.x, other.transform.position.y) ;

                if ((recoilVector.x < 0 && damageRecoil.x > 0) || (recoilVector.x > 0 && damageRecoil.x < 0))
                {
                    damageRecoil.x *= -1;
                }

                rb.AddForce(damageRecoil, ForceMode2D.Impulse);
            }
            if (currentHealth <= 0) animator.SetTrigger("Death");
        }
    }

    private IEnumerator WaitToMove()
    {
        canMove = false;
        while (rb.linearVelocity.magnitude > 0.2f)
        {
            yield return null;
        }
        canMove = true;
    }

    //lo llamo desde el animator
    public void Destroy()
    {
        Instantiate(coin, this.transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
        Destroy(this.gameObject);
        
    }


}



#endregion

