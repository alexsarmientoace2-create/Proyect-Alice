using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Input Actions")]
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction attack;

    [Header("Movement Settings")]
    public float movementSpeed;
    public float sprintSpeedMultiplier;
    private float currentSpeed;
    public float maxStamina;
    [SerializeField] float minStaminaToRun;
    private float currentStamina;
    public float CurrentStamina => currentStamina;
    [SerializeField] float staminaRegainMultiplier = 1;
  
    bool isSprinting;
    PlayerUIManager playerUIManager;
    [SerializeField] GameObject ManagerGo;


    [Header("Jump Settings")]
    public float jumpForce;
    // Multiplicador para recortar la velocidad de ascenso si se suelta el botón antes de tiempo
    [SerializeField] private float jumpCutMultiplier;
    // Controla lo rápido o suave que frena al cortar el salto (a mayor número, más rápido frena)
    [SerializeField] private float jumpSmoothFactor; 
    [Header("State & Physics")]
    private bool isGrounded;
    private bool isJumping;
    private bool canMove = true;
    private bool canJump = true;

    [Header("Components")]
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer swordSpriteRenderer;
    private Animator animator;
    private LevelManager levelManager;

    [Header("Combat & Health")]
    private HealthSystem playerHealth;
    public float damageTaken;
    [SerializeField] private Vector2 damageRecoil;
    [SerializeField] private float recoilTime;
    [SerializeField] private Collider2D hurtboxCollider;

    [Header("Combat Visuals & Effects")]
    [SerializeField] private Vector3 hitboxStartingPosition;
    [SerializeField] private GameObject hitbox;
    private Color colorInicial;
    private bool isHealing = false;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayLenght = 1;
    [SerializeField] GameObject floorDetector;
    [SerializeField] float coyoteTimeDuration = 0.15f;
    float coyoteTimer;
    private Coroutine coyoteRoutine;

    #endregion

    #region Unity Methods

    private void Start()
    {
        levelManager = ManagerGo.GetComponent<LevelManager>();
        playerUIManager = ManagerGo.GetComponent<PlayerUIManager>();
        currentStamina = maxStamina;
        hitbox.transform.position = hitboxStartingPosition;
        currentSpeed = movementSpeed;

        move = InputSystem.actions.FindAction("move");
        jump = InputSystem.actions.FindAction("jump");
        sprint = InputSystem.actions.FindAction("Sprint");
        attack = InputSystem.actions.FindAction("Attack");

        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<HealthSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        swordSpriteRenderer = hitbox.GetComponent<SpriteRenderer>();
        colorInicial = spriteRenderer.color;
        animator = GetComponent<Animator>();
        playerUIManager.ChangeStaminaUI();
    }

    private void FixedUpdate()
    {
        CheckGroundStatus();
        Vector2 direccionMovimiento = move.ReadValue<Vector2>();
        Movement(direccionMovimiento);
        if (!isSprinting && isGrounded && currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * staminaRegainMultiplier;
            playerUIManager.ChangeStaminaUI();
        }
    }

    #endregion

    #region Ground Detection

    private bool IsGrounded()
    {
        return Physics2D.Raycast(floorDetector.transform.position, Vector2.down, groundRayLenght, groundLayer);
    }

    private void CheckGroundStatus()
    {
        bool previousGrounded = isGrounded;
        bool rayHit = Physics2D.Raycast(floorDetector.transform.position, Vector2.down, groundRayLenght, groundLayer);      

        // CORRECCIÓN CLAVE: Si estamos saltando y subiendo, ignoramos temporalmente el Raycast 
        // para evitar falsos positivos de aterrizaje prematuro en el primer milisegundo.
        if (isJumping && rb.linearVelocityY > 0f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = rayHit;
        }

        if (isGrounded && !previousGrounded)
        {
            canJump = true;
            animator.SetBool("isTouchingGround", true);
            isJumping = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            coyoteTimer = coyoteTimeDuration;
        }
        else if (!isGrounded && previousGrounded)
        {
            animator.SetBool("isTouchingGround", false);
            // Evitamos que se active la caída por error si estamos en plena corrutina/estado de salto
            if (!isJumping)
            {
                animator.SetBool("isFalling", true);
                //empleo la variable coyoteRoutine para asegurarme que solo hay una corutina coyote a la vez
                if (coyoteRoutine != null) StopCoroutine(coyoteRoutine);
                coyoteRoutine = StartCoroutine(CoyoteTimeCoroutine());
            }
        }

        if (isGrounded)
        {
            isJumping = false;
            canJump = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            animator.SetBool("isTouchingGround", true);
        }

        if (!IsGrounded())
        {
            isGrounded = false;
            animator.SetBool("isTouchingGround", false);

            if (!isJumping)
            {
                animator.SetBool("isFalling", true);
            }
        }
    }

    private IEnumerator CoyoteTimeCoroutine()
    {
        canJump = true;
        float timer = coyoteTimeDuration;

        while (timer > 0f)
        {
            // Si el jugador pulsa saltar antes de que acabe el tiempo, canJump cambiará a false y salimos
            if (!canJump) yield break;

            timer -= Time.deltaTime;
            yield return null; // Espera al siguiente frame obligatoriamente para no congelar Unity
        }

        canJump = false; // Se acabó el tiempo de coyote
    }

    #endregion

    #region Jump Mechanics

    public void OnJump()
    {
        // Solo saltar en el frame en que se pulsa, no al soltar
        if (!jump.WasPressedThisFrame()) return;

        if (canJump && canMove == true)
        {
            canJump = false; // Consumimos el salto

            // Si saltamos usando el coyote time, paramos la corrutina para que no interfiera
            if (coyoteRoutine != null)
            {
                StopCoroutine(coyoteRoutine);
                coyoteRoutine = null;
            }

            isGrounded = false;
            isJumping = true;

            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isTouchingGround", false);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            StartCoroutine(JumpLogic());
        }
    }

    IEnumerator JumpLogic()
    {
        // Damos un frame de margen para que el AddForce impulse al personaje hacia arriba con seguridad
        yield return null;

        // Mantendremos este bucle activo mientras el personaje siga en estado de salto
        while (isJumping)
        {
            // SALTO VARIABLE SUAVIZADO: Si soltamos el botón mientras subimos, 
            // reducimos la velocidad de forma progresiva.
            if (!jump.IsPressed() && rb.linearVelocityY > 0f)
            {
                float targetVelocityY = rb.linearVelocityY * jumpCutMultiplier;

                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    Mathf.MoveTowards(rb.linearVelocityY, targetVelocityY, jumpSmoothFactor * Time.deltaTime)
                );
            }

            // Transición de ascenso a caída cuando la velocidad en Y pasa a ser negativa o cero
            if (rb.linearVelocityY <= 0f)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
                break; // Rompemos el bucle porque el salto ha terminado
            }

            yield return null; // Espera al siguiente frame antes de volver a comprobar
        }
    }

    #endregion

    #region Movement & Sprint

    private void Movement(Vector2 direccionMovimiento)
    {
        if (canMove == true)
        {
            rb.linearVelocity = new Vector2(direccionMovimiento.x * currentSpeed * Time.deltaTime, rb.linearVelocityY);
        }

        if (direccionMovimiento.x != 0 && canMove == true && IsGrounded() == true)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (direccionMovimiento.x < 0)
        {
            spriteRenderer.flipX = true;
            swordSpriteRenderer.flipX= true;
            hitbox.transform.localPosition = new Vector3(-Mathf.Abs(hitboxStartingPosition.x), hitboxStartingPosition.y, hitboxStartingPosition.z);
        }
        else if (direccionMovimiento.x > 0)
        {
            spriteRenderer.flipX = false;
            swordSpriteRenderer.flipX = false;
            hitbox.transform.localPosition = new Vector3(Mathf.Abs(hitboxStartingPosition.x), hitboxStartingPosition.y, hitboxStartingPosition.z);
        }
    }

    public void OnSprint(InputValue value)
    {
        if (sprint.IsPressed() && currentStamina >= minStaminaToRun)
        {
            StartCoroutine(Sprint());
            
        }
    }

    private IEnumerator Sprint()
    {
        while (sprint.IsPressed() && currentStamina > 0)
        {
            if (canMove == true && isGrounded && rb.linearVelocityX != 0)
            {
                isSprinting = true;
                currentStamina -= Time.deltaTime;
                currentSpeed = movementSpeed * sprintSpeedMultiplier;
                animator.SetBool("isRunning", true);
                playerUIManager.ChangeStaminaUI();
            }
            yield return null;
        }
        currentSpeed = movementSpeed;
        animator.SetBool("isRunning", false);
        isSprinting = false;
    }

    #endregion

    #region Combat & Health Mechanics

    public void OnAttack()
    {
        if (isGrounded && canMove == true)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    public void StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") && hurtboxCollider.IsTouching(collision))
        {
            rb.linearVelocity = Vector2.zero;
            Vector2 collisionPosition = new Vector2(collision.transform.position.x, collision.transform.position.y);
            recoil(collisionPosition);
            playerHealth.TakeDamage(10);
        }

        if (collision.gameObject.CompareTag("health"))
        {
            if (playerHealth.CurrentHealth < playerHealth.MaxHealth)
            {
                playerHealth.IncreaseHealth(20);
                Destroy(collision.gameObject);
                isHealing = true;
                StartCoroutine(HealtColorEffect());
                StartCoroutine(WaitToStopHealing());
            }
        }

        if (collision.gameObject.CompareTag("death"))
        {
            playerHealth.OnDeath.Invoke();
        }

        if (collision.gameObject.CompareTag("win"))
        {
            levelManager.Win();
        }

        if (collision.gameObject.CompareTag("point"))
        {
            levelManager.totalPoints += 1;
            playerUIManager.ChangePoints();
            Destroy (collision.gameObject);
        }
    }

    private void recoil(Vector2 otherPosition)
    {
        canMove = false;
        StartCoroutine(RecoilColorEffect());
        StartCoroutine(WaitToMove());

        Vector2 recoilVector = rb.position - otherPosition;

        if ((recoilVector.x < 0 && damageRecoil.x > 0) || (recoilVector.x > 0 && damageRecoil.x < 0))
        {
            damageRecoil.x *= -1;
        }

        rb.AddForce(damageRecoil, ForceMode2D.Impulse);
    }

    private IEnumerator RecoilColorEffect()
    {
        while (canMove == false)
        {
            float saturation = (Time.time * 4) % 1f;
            Color color = Color.HSVToRGB(1f, saturation, 1f);
            spriteRenderer.color = color;
            yield return null;
        }
    }

    private IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(recoilTime);
        canMove = true;
        spriteRenderer.color = colorInicial;
    }

    private IEnumerator HealtColorEffect()
    {
        while (isHealing == true)
        {
            float saturation = (Time.time * 4) % 1f;
            Color color = Color.HSVToRGB(0.33f, saturation, 1f);
            spriteRenderer.color = color;
            yield return null;
        }
    }

    private IEnumerator WaitToStopHealing()
    {
        yield return new WaitForSeconds(recoilTime / 1.5f);
        isHealing = false;
        spriteRenderer.color = colorInicial;
    }

    #endregion

    #region Gizmos & Debug

    private void OnDrawGizmos()
    {
        if (floorDetector != null)
        {
            Debug.DrawRay(floorDetector.transform.position, Vector2.down * groundRayLenght, Color.red);
        }
    }

    #endregion
}