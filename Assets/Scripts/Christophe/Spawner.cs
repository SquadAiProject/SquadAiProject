using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Transform spawnPoint;

    [FormerlySerializedAs("count")] public int counts = 99;

    public float spawnRate = 1.0f;
    private float originRate;

    private void Start()
    {
        originRate = spawnRate;
    }

    private void Update()
    {
        if (enemyPrefab && counts > 0)
        {
            spawnRate -= Time.deltaTime;

            if (spawnRate <= 0 && counts > 0)
            {
                counts--;
                GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                spawnRate = originRate;
            }
        }
    }
}
