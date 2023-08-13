using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public float TimeBetweenSpawn = 2f;
    public GameObject EnemyPrefab;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if(_timer > TimeBetweenSpawn)
        {
            _timer = 0;
            Instantiate(EnemyPrefab, SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity);
        }
    }
}
