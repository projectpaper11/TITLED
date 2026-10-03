using System;
using System.Collections.Generic;
using Agents.Enemies;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using DevLib.TileAstar;
using GameSystem.GameServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystem.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRange = 5f;
        [SerializeField] private float minSpawnRate = 5f;
        [SerializeField] private float maxSpawnRate = 10f;
        [SerializeField] private List<GameObject> _enemies;
        [SerializeField] private PathBakeDataSO pathBakeData;
        [SerializeField] private SoundClipSO[] soundPlayers;

        private float _timer;
        private IMapService _mapService;

        private void Start()
        {
            _mapService = ServiceLocator.Get<IMapService>();
            _timer = Random.Range(minSpawnRate, maxSpawnRate);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                SpawnEnemy();
                _timer = Random.Range(minSpawnRate, maxSpawnRate);
            }
        }

        private void SpawnEnemy()
        {
            ServiceLocator.Get<IAudioService>().PlaySfx(soundPlayers[Random.Range(0, soundPlayers.Length)]);
            GameObject enemy = Instantiate(_enemies[Random.Range(0, _enemies.Count)]);
            Vector3 safePosition = new Vector3();
            NodeData node;
            do
            {
                float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                Vector2 randomDir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
                safePosition = transform.position + (Vector3)(randomDir.normalized * spawnRange);
            } while (!pathBakeData.GetNodeIfExist(_mapService.GetWorldToCell(safePosition),out node));
            enemy.transform.position = node.worldPosition;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRange);
        }
    }
}