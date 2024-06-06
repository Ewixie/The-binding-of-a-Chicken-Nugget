using UnityEngine;

namespace Enemies
{
    public interface IEnemyMovement
    {
        void Init(Rigidbody2D body, Transform target, MovementConfig config);
        void UpdateMovement();
    }
}