using UnityEngine;

public class NPCHorizMov : NPCMovement
{
    private void Start()
    {
        ChangeDirection();
    }
    private void Update()
    {
        
    }

    protected override void ChangeDirection()
    {
        Debug.Log("Soy el hijo de NPCMovement");

        base.ChangeDirection();
    }
}
