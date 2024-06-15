using System;
using Entities.Enemies.Movement;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float startingHealth;
        public event Action Died;

        protected IEnemyMovement EnemyMovement;
        public float CurrentHealth { get; protected set; }
        
        private void Start()
        {
            CurrentHealth = startingHealth;
            Init();
        }

        protected abstract void Init();


        public virtual void TakeDamage(float damage)
        {
            if (damage <= 0) return;
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                Died?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}