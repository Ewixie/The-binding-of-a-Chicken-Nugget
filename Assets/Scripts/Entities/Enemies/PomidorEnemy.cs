using System;
using Entities.Data;
using Entities.Enemies.Movement;
using Player;
using UnityEngine;

namespace Entities.Enemies
{
    public class PomidorEnemy : Enemy
    {
        [SerializeField] private MovementConfig movementConfig;
        [SerializeField] private Rigidbody2D rb;
        
        protected override void Init()
        {
            EnemyMovement = new EnemyStraightFollowMovement();
            EnemyMovement.Init(rb,Player.Player.Instance.PlayerTransform, movementConfig);
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out PlayerDamageable damageable))
            {
                damageable.TakeDamage(1);
            }
        }

        private void Update()
        {
            EnemyMovement.UpdateMovement(Time.deltaTime);
        }
        
    }
}
