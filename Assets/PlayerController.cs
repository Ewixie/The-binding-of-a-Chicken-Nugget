using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    private PlayerInput _playerInput;
    private bool _isInitialized;
    
    public void Init(PlayerInput input)
    {
        if (_isInitialized) return;
        _playerInput = input;
        _isInitialized = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = _playerInput.GetMovement() * speed;
    }
}