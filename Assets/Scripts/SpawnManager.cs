﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(5);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int powerupLottery = Random.Range(0, 4);
            if (powerupLottery == 0)
            {
                int powerupID = 0;
                GameObject newPowerup = Instantiate(_powerups[powerupID], posToSpawn, Quaternion.identity);
            }
            else if (powerupLottery == 1)
            {
                int powerupID = 1;
                GameObject newPowerup = Instantiate(_powerups[powerupID], posToSpawn, Quaternion.identity);
            }
            else if (powerupLottery == 2)
            {
                int powerupID = 2;
                GameObject newPowerup = Instantiate(_powerups[powerupID], posToSpawn, Quaternion.identity);
            }
            else
            {
                int powerupID = 3;
                GameObject newPowerup = Instantiate(_powerups[powerupID], posToSpawn, Quaternion.identity);
            }
        }
    }

    public void StartSpawnRoutine()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
