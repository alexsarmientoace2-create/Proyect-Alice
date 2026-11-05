using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class NPCBehaviour : MonoBehaviour
{
    #region Atributes
    //SerializeField usado en una varibale privada hace que el color no pueda ser cambiada desde otras clases pero si desde el inspector de unity
    //NonSerialized es lo contrario, usarlo en una variable publica haceque pueda ser accedida desde otras clases pero no desde el inspector de unity
    [SerializeField] float rainbowSpeed;
    private SpriteRenderer spriteRenderer;
    InputAction move;
    public float movementSpeed;
    Rigidbody2D rb;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        move = InputSystem.actions.FindAction("move");
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        ChangeColor(spriteRenderer.color);
        Vector2 direccionMovimiento = move.ReadValue<Vector2>();
        Movement(direccionMovimiento);
    }

    #endregion

    #region My Methods

    private void ChangeColor(UnityEngine.Color color)
    {

        //Usamos Time.time en lugar de Time.deltatime porque lo que queremos es usar el tiempo total transcurrido en lugar del tiempo entre frame y frame
        //usamos %1f para que el resultado siempre se quede entre 0 y 1 ya que usamos el resto de la division con 1. asi, da igual es numero que sea o cuanto tiempo transcurra,
        //siempre va a ir de 0 a 1 y de 1 a 0 con todos los decimales entremedias. cuando usamos esto en el hue nos da una transicion continua ya que estamos usando hsv y no rgb.
        float hue = (Time.time * rainbowSpeed) % 1f;
        //HSVToRGB nos permite trabajar con el color como un vector3 y traducir este vector3 a color. Aqui solo estmos afectando el tono (hue)
        color = Color.HSVToRGB(hue, 1f, 1f);

        spriteRenderer.color = color;

        Debug.Log(color);
    }

    private void Movement(Vector2 direccionMovimiento)
    {
        //El codigo no comentado y el comentado son lo mismo, operamos un vector con un numero (ya sea int o float), estamos operando todos los valores de dicho vector a la vez 
        // rb.linearVelocity = new Vector2(direccionMovimiento.x * movementSpeed * Time.deltaTime, direccionMovimiento.y * movementSpeed * Time.deltaTime);
        //uso .normalized para que a la hora de ir en diagonal la velocidad no aumente de mas por la suma de los dos vectores de movimiento
        //transform.postion = (direccionMovimiento * movementSpeed * Time.deltaTime).normalized;
        rb.linearVelocity = new Vector2(direccionMovimiento.x * movementSpeed * Time.deltaTime, direccionMovimiento.y * movementSpeed * Time.deltaTime);
    }
}

#endregion

