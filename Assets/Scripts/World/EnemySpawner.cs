using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemySpawner : MonoBehaviour {
    private static EnemySpawner _instance;
    public static EnemySpawner Instance => _instance;

    private int _count = 0;
    public int Count => _count;

    public void OnEnemyDeath()
    {
        _count--;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    public Enemy[] enemies;

    private Random rng;

    
    private void Start() {
        int seed = (int)DateTime.Now.Ticks;
        rng = new Random(seed);
    }

    public void SpawnEnemies(Transform[] spawnPoints, int level) {
        ShuffleSpawnPoints(spawnPoints);

        float percentage = (level <= 15) ? ((float) level / 15.0f) : 1.0f;
        int spawnCount = (int)Mathf.Lerp(1.0f, spawnPoints.Length, percentage);
        
        for (int i = 0; i < spawnCount; i++) {
            _count++;
            var enemy = Instantiate(RandEnemy()).transform;
            enemy.position = spawnPoints[i].position;
        }
    }
    
    private void ShuffleSpawnPoints(Transform[] list)  
    {  
        int n = list.Length;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            Transform value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    private Enemy RandEnemy() {
        return enemies[rng.Next(0, enemies.Length)];
    }
}
