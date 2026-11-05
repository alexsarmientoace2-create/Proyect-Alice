using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class NPCBehaviour1 : MonoBehaviour
{
    #region Atributes
    public float movementSpeed;
    Rigidbody2D rb;
    public float timeToChange;
    float timer;
    Vector2 direction = new Vector2(1,1);


    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = timeToChange;
            direction.y = direction.y * -1;
        }
        rb.linearVelocity = new Vector2(direction.x * movementSpeed * Time.deltaTime, direction.y * movementSpeed * Time.deltaTime);     
    }

    #endregion

    #region My Methods


}

#endregion

