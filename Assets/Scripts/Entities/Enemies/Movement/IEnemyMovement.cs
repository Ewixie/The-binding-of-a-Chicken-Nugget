using Entities.Data;
using UnityEngine;

namespace Entities.Enemies.Movement
{
    public interface IEnemyMovement
    {
        void Init(Rigidbody2D body, Transform target, MovementConfig config);
        void UpdateMovement();
    }
}