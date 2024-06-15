using Entities.Enemies;
using UnityEngine;

namespace Entities.Data
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Entities/Enemies/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public Enemy EnemyPrefab => enemyPrefab;
        [SerializeField] private Enemy enemyPrefab;
        public float SpawnCost => spawnCost;
        [SerializeField] private float spawnCost;
        
    }
}