using Unity.VisualScripting;
using UnityEngine;

public class RockProjectile : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}



