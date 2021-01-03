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

    public void SpawnEnemies(Transform[] spawnPoints, float probability) {
        foreach (var point in spawnPoints) {
            bool spawn = UnityEngine.Random.Range(0.0f, 1.0f) <= probability;
            if (spawn)
            {
                _count++;
                var enemy = Instantiate(RandEnemy()).transform;
                enemy.position = point.position;   
            }
        }
    }

    private Enemy RandEnemy() {
        return enemies[rng.Next(0, enemies.Length)];
    }
}
