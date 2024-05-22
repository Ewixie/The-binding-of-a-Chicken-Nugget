using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float velocity;
    private PlayerInput _playerInput;      
    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Update()
    {
        _playerInput.UpdateInput();
        rb.velocity = _playerInput.GetMovement() * velocity;
    }
}