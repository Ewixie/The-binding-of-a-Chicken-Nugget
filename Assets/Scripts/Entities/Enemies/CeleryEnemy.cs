using System.Numerics;
using Entities.Data;
using Player.Shooting.Projectile;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Entities.Enemies
{
    public class CeleryEnemy : Enemy
    {
        [SerializeField] private ShootingConfig shootingConfig;

        private float _shootingTimer;

        private BulletFactory _bulletFactory;
        
        
        protected override void Init()
        {
            _bulletFactory = new BulletFactory(shootingConfig.BulletPrefab);
        }
        private void Update()
        {
            if (_shootingTimer > 0) _shootingTimer -= Time.deltaTime;
            if (_shootingTimer <= 0)
            {

                Vector2 directionToPlayer = (( (Vector2)Player.Player.Instance.PlayerTransform.position + Player.Player.Instance.PlayerVelocity) - (Vector2)transform.position).normalized;
                
                Bullet bullet = _bulletFactory.CreateBullet(transform.position);
                bullet.SetVelocity(directionToPlayer * shootingConfig.ProjectileSpeed);
                _shootingTimer = shootingConfig.ShootingDelay;
            }
        }
    }
}