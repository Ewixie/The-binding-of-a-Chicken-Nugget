using UnityEngine;
using UnityEngine.Pool;

namespace Player.Shooting
{
    public class BulletFactory
    {
        private readonly Bullet _bulletPrefab;
        private readonly ObjectPool<Bullet> _bulletsPool;

        public BulletFactory(Bullet bulletPrefab)
        {
            _bulletsPool = new ObjectPool<Bullet>(CreateNewBullet, OnBulletGet, OnBulletRelease, OnDestroyBullet, true,
                20, 1000);

            _bulletPrefab = bulletPrefab;
        }
        
        public Bullet CreateBullet(Vector3 position)
        {
            var bullet = _bulletsPool.Get();
            bullet.transform.position = position;
            return bullet;
        }

        private Bullet CreateNewBullet()
        {
            var bullet = Object.Instantiate(_bulletPrefab, Vector2.zero, Quaternion.identity);
            bullet.BulletDestroyed += () =>
            {
                _bulletsPool.Release(bullet);
            };
            return bullet;
        }
        
        private void OnDestroyBullet(Bullet obj)
        {
            Object.Destroy(obj.gameObject);
        }
        
        private void OnBulletRelease(Bullet obj)
        {
            obj.gameObject.SetActive(false);
            obj.ResetBullet();
        }

        private void OnBulletGet(Bullet obj)
        {
            obj.gameObject.SetActive(true);
        }
    }
}