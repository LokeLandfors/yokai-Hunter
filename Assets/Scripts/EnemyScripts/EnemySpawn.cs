using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;   
    public int maxEnemies = 10;       
    public float spawnInterval = 0.5f; 

    private int enemiesSpawned = 0;   

    void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (enemiesSpawned < maxEnemies)
        {
            SpawnEnemy();
            enemiesSpawned++;

            
            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Reached maximum enemy limit, stopping spawns.");
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("No Enemy prefab assigned to EnemySpawn!");
            return;
        }

        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Debug.Log($"Spawned Enemy #{enemiesSpawned + 1}");
    }
}
