using UnityEngine;

public abstract class NPCMovement : MonoBehaviour
{
    public virtual void UpdateMovement()
    {

    }

    protected virtual void ChangeDirection()
    {
        Debug.Log("Soy NPCMovement");
    }

    public virtual void BePushed()
    {

    }
}
