using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    private bool _spawn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    public void StopSpawning()
    {
        _spawn = false;
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_spawn)
        {
            var spawnPoint = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var newEnemy = Instantiate(_enemyPrefab, spawnPoint, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_spawn)
        {
            var spawnPoint = new Vector3(Random.Range(-8f, 8f), 7f, 0);

            var index = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[index], spawnPoint, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }
}
