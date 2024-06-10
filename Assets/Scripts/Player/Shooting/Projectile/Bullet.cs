using System;
using Entities;
using UnityEngine;
using Utils;

namespace Player.Shooting.Projectile
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask destroyMask;
        [SerializeField] private float damage = 10;
        public event Action BulletDestroyed;
        
        public void SetVelocity(Vector2 velocity)
        {
            rb.velocity = velocity;
        }
        public void ResetBullet()
        {
            rb.velocity = Vector2.zero;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable is not null)
            {
                damageable.TakeDamage(damage);
                DestroyBullet();
            }
            
            
            if (LayerUtils.IsInLayerMask(other.gameObject.layer, destroyMask))
            {
                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            BulletDestroyed?.Invoke();
        }
    }
}