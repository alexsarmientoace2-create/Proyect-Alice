using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class NPCBehaviour2 : MonoBehaviour
{
    #region Atributes
    public float movementSpeed;
    Rigidbody2D rb;
    public float timeToChange;
    float timer;
    Vector2 direction = new Vector2(1,1);
    bool yChangeIsNext = true;


    #endregion

    #region Unity Methods
    private void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && yChangeIsNext == true )
        {
            timer = timeToChange;
            direction.y = direction.y * -1;
            yChangeIsNext = false;
        }
        else if ( timer <= 0 && yChangeIsNext == false ) 
        {
            timer = timeToChange;
            direction.x = direction.x * -1;
            yChangeIsNext = true;
        }

            rb.linearVelocity = new Vector2(direction.x * movementSpeed * Time.deltaTime, direction.y * movementSpeed * Time.deltaTime);
       
    }

    #endregion

    #region My Methods

 
}

#endregion

