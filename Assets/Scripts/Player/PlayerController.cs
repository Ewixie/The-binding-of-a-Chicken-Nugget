using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;

        private CustomPlayerInput _playerInput;
        private bool _isInitialized;
    
        public void Init(CustomPlayerInput input)
        {
            if (_isInitialized) return;
            _playerInput = input;
            _isInitialized = true;
        }

        private void FixedUpdate()
        {
            rb.velocity = _playerInput.Movement * speed;
        }
    }
}