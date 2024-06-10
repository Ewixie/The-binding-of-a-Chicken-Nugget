using System;
using Entities;
using UnityEngine;

namespace Player.Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerHealth playerHealth;

        public void Init()
        {
            playerHealth.Init();
        }
        
    }

    [Serializable]
    public class PlayerHealth : IDamageable
    {
        public void Init()
        {
            CurrentHealth = StartingHealth;
        }
        
        [SerializeField] private float startingHealth;
        public float StartingHealth => startingHealth;
        public bool IsDead => (CurrentHealth <= 0);
        public float CurrentHealth { get; protected set; }
        
        public event Action TookDamage;
        public event Action Died;
        
        public void TakeDamage(float damage)
        {
            if (damage <= 0) return;
            CurrentHealth -= damage;
            TookDamage?.Invoke();
            if (CurrentHealth <= 0)
            {
                Died?.Invoke();
            }
        }
    }
}