using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class NPCBehaviour : MonoBehaviour
{
    #region Atributes
    //SerializeField usado en una varibale privada hace que el color no pueda ser cambiada desde otras clases pero si desde el inspector de unity
    //NonSerialized es lo contrario, usarlo en una variable publica haceque pueda ser accedida desde otras clases pero no desde el inspector de unity
    [SerializeField] float rainbowSpeed;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ChangeColor(spriteRenderer.color);

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
}

#endregion

