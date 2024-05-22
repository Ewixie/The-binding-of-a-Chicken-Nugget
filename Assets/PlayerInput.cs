using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput
{
    private Vector2 movementXY;

    public Vector2 GetMovement()
    {
        return movementXY;
    }
    
    public void UpdateInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movementXY = new Vector2(inputX, inputY).normalized;
    }
}