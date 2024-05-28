using Player.Shooting;
using UnityEngine;

public class PomidorEnemy : MonoBehaviour, IDamageable
{
    private float _currentHealth;
    [SerializeField] private float startingHealth;

    private void Start()
    {
        _currentHealth=startingHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}
