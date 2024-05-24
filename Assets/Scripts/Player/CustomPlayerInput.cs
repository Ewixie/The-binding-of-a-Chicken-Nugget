using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [Serializable]
    public class CustomPlayerInput
    {
        public Vector2 Movement => _movement;
        public Vector2 ShootingDirection => _shootingDirection;
        public bool IsShooting => _shootingDirection.magnitude > 0;
    
        private Vector2 _movement;
        private Vector2 _shootingDirection;
    
        [SerializeField] private InputAction movementAction;
        [SerializeField] private InputAction shootAction;

        public void Init()
        {
            movementAction.Enable();
            shootAction.Enable();
        }

        public void UpdateInput()
        {
            _movement = movementAction.ReadValue<Vector2>();
            _shootingDirection = shootAction.ReadValue<Vector2>();
        }
    }
}