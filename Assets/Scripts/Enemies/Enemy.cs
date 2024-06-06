using Player.Shooting;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float startingHealth;

        protected IEnemyMovement EnemyMovement;
        
        private float _currentHealth;
        
        private void Start()
        {
            _currentHealth = startingHealth;
            Init();
        }

        protected abstract void Init();

        public virtual void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}