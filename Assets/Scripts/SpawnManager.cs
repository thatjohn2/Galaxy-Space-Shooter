using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    /*
     * Create a Wave system wherebye enemies appear one after another in groups
     * each group should be bigger than the last
     * crate a pause between waves to allow time for the player to deal with them
     * spawn each enemy in a wave one second apart
     * allow 10 seconds between waves to give players space to breath
     */
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;

    int _waveSize = 1;

    int powerupID;

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
            for (int i = 0; i < _waveSize; i++)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(1f);
            }
            _waveSize++;
            yield return new WaitForSeconds(10f);
        }
        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int powerupLottery = Random.Range(0, 11);
            if (powerupLottery < 2)
            {
                powerupID = 0;
            }
            else if (powerupLottery < 4)
            {
                powerupID = 1;
            }
            else if (powerupLottery < 6)
            {
                powerupID = 2;
            }
            else if (powerupLottery < 8)
            {
                powerupID = 3;
            }
            else if (powerupLottery < 10)
            {
                powerupID = 4;
            }
            else
            {
                powerupID = 5;
            }
            GameObject newPowerup = Instantiate(_powerups[powerupID], posToSpawn, Quaternion.identity);
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
