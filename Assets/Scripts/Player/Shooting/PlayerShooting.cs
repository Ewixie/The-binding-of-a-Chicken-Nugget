using UnityEngine;

namespace Player.Shooting
{
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Bullet settings")] 
        [SerializeField] private ShootingConfig config;

        private BulletFactory _bulletFactory;
        private CustomPlayerInput _playerInput;

        private float _shootingTimer;

        private bool _isInitialized;

        public void Init(CustomPlayerInput input)
        {
            if (_isInitialized) return;
            _bulletFactory = new BulletFactory(config.BulletPrefab);
            _playerInput = input;
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;
            if (_shootingTimer > 0) _shootingTimer -= Time.deltaTime;
            if (!_playerInput.IsShooting) return;
            if (_shootingTimer <= 0)
            {
                Bullet bullet = _bulletFactory.CreateBullet(transform.position);
                bullet.SetVelocity(_playerInput.ShootingDirection * config.ProjectileSpeed);
                _shootingTimer = config.ShootingDelay;
            }
        }
    }
}