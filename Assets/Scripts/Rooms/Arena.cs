using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Data;
using Entities.Enemies;
using Managers;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class Arena : MonoBehaviour
    {
        [Header("Spawning")]
        [SerializeField] private List<EnemyConfig> enemyConfigs;
        [SerializeField] private float waitBetweenSpawns;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField][MinMaxSlider(0, 20)] private Vector2 spawnValue;
        [Space(10)]        
        [SerializeField] private Room connectedRoom;


        private bool _spawningEnemies;
        private bool _isCompleted;
        
        private List<Enemy> _currentEnemies = new List<Enemy>();

        public void Start()
        {
            foreach (var door in connectedRoom.Doors)
            {
                door.EnteredRoom += StartArena;
            }
        }

        [Button]
        public void SetPointsToChildren()
        {
            spawnPoints.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                spawnPoints.Add(transform.GetChild(i));
            }
        }


        private void StartArena()
        {
            if (_isCompleted) return;
            connectedRoom.CloseDoors();
            StartCoroutine(SpawnEnemiesRoutine());
        }

        private IEnumerator SpawnEnemiesRoutine()
        {
            _spawningEnemies = true;
            float value = Random.Range(spawnValue.x, spawnValue.y);
            while (value > 0)
            {
                value -= SpawnEnemy();
                yield return TimeManager.WaitGameTime(waitBetweenSpawns);
            }

            _spawningEnemies = false;
            CheckCompleted();
        }

        private float SpawnEnemy()
        {
            EnemyConfig randomEnemy = enemyConfigs[Random.Range(0, enemyConfigs.Count)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            var enemy = Instantiate(randomEnemy.EnemyPrefab, randomPoint.position, Quaternion.identity);
            _currentEnemies.Add(enemy);
            enemy.Died += () =>
            {
                OnEnemyDied(enemy);
            };
            return randomEnemy.SpawnCost;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _currentEnemies.Remove(enemy);
            if (!_spawningEnemies) CheckCompleted();
        }

        private void CheckCompleted()
        {
            if (_isCompleted) return;
            if (_currentEnemies.Count == 0)
            {
                _isCompleted = true;
                connectedRoom.OpenDoors();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy is not null)
            {
                OnEnemyDied(enemy);
                Destroy(enemy.gameObject);
            }
        }
    }
}