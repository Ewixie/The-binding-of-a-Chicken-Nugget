
using System;
using UnityEngine;

namespace Enemies
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

        private void Update()
        {
            EnemyMovement.UpdateMovement();
        }
    }
}
