using UnityEngine;

namespace Player.Shooting
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootingDelay = 0.1f;
        [SerializeField] private Bullet bulletPrefab;

        private BulletFactory _bulletFactory;
        private CustomPlayerInput _playerInput;

        private float _shootingTimer;
        
        private bool _isInitialized;
    
        public void Init(CustomPlayerInput input)
        {
            if (_isInitialized) return;
            _bulletFactory = new BulletFactory(bulletPrefab);
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
                var bullet = _bulletFactory.CreateBullet(transform.position);
                bullet.SetVelocity(_playerInput.ShootingDirection * projectileSpeed);
                _shootingTimer = shootingDelay;
            }
        }
    }
}