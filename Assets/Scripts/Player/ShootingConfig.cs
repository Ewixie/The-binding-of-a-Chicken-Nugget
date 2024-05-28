using Player.Shooting;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "New Shooting Config", menuName = "Player")]
    public class ShootingConfig : ScriptableObject
    {
        public float ProjectileSpeed => projectileSpeed;
        public float ShootingDelay => shootingDelay;
        public Bullet BulletPrefab => bulletPrefab;
        
        [SerializeField] private Bullet bulletPrefab;
        [Space(10)]
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootingDelay = 0.1f;

       
    }
}