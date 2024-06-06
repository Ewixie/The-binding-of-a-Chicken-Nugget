using UnityEngine;

namespace Enemies
{
    public class EnemyStraightFollowMovement : IEnemyMovement
    {
        private Rigidbody2D _rb;
        private Transform _target;
        private MovementConfig _config;
        
        public void Init(Rigidbody2D body,Transform target, MovementConfig config)
        {
            _rb = body;
            _target = target;
            _config = config;
        }

        public void UpdateMovement()
        {
            Vector2 directionToPlayer = (_target.position - _rb.transform.position).normalized;
            _rb.AddForce(directionToPlayer * _config.Acceleration);
            if (_rb.velocity.magnitude >= _config.MaxSpeed)
            {
                _rb.velocity = _rb.velocity.normalized * _config.MaxSpeed;
            }
        }
    }
}