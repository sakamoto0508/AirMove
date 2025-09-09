using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private EnemyFactoryManager _factoryManager;
    [Header("Spawn Position Settings")]
    [SerializeField] private Vector3 _minSpawnPositionGround = new Vector3(-5, 0, -10);
    [SerializeField] private Vector3 _maxSpawnPositionGround = new Vector3(5, 0, -10);
    [SerializeField] private Vector3 _minSpawnPositionAir = new Vector3(-5, 5, -10);
    [SerializeField] private Vector3 _maxSpawnPositionAir = new Vector3(5, 10, 10);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnRandomEnemy()
    {
        List<string> types = _factoryManager.GetAvailableTypes();
        if (types.Count == 0) return;

    }
}
