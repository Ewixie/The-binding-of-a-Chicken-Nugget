using Player.Data;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;

        private CustomPlayerInput _playerInput;
        private bool _isInitialized;

        private PlayerData _data;

        public Vector2 Velocity => rb.velocity;
        
        public void Init(PlayerData data, CustomPlayerInput input)
        {
            if (_isInitialized) return;
            _playerInput = input;
            _data = data;
            _isInitialized = true;
        }

        private void FixedUpdate()
        {
            rb.velocity = _playerInput.Movement * speed;
        }
    }
}