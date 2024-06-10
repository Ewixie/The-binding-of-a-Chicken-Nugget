using System.Collections;
using Entities;
using Player.Data;
using UnityEngine;

namespace Player
{
    public class PlayerDamageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private float immunityDuration = 2f;
        
        private PlayerData _data;
        private bool _isImmune;
        
        public void Init(PlayerData data)
        {
            _data = data;
            _data.playerHealth.Died += (() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void TakeDamage(float damage)
        {
            if (_isImmune) return;
            _data.playerHealth.TakeDamage(damage);
            if (!_data.playerHealth.IsDead) StartCoroutine(ActivateImmunity());
        }
        
        private IEnumerator ActivateImmunity()
        {
            _isImmune = true;
            float elapsedTime = 0;
            while (elapsedTime < immunityDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _isImmune = false;
        }
    }
}