using System;
using UnityEngine;
using Utils;

namespace Player.Shooting
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask destroyMask;

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