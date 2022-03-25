using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    bool isBoss;

    [Header("Map Move")]
    public float terrainMoveSpeed;
    [SerializeField] List<GameObject> TerrainList = new List<GameObject>();
    [SerializeField] Transform spawnPos;
    [SerializeField] Transform endPos;

    [Header("Enemy")]
    [SerializeField] float enemySpawnDelay;
    [SerializeField] GameObject Bacteria;
    [SerializeField] GameObject Germ;
    [SerializeField] GameObject Virus;

    void Start()
    {
        StartCoroutine(Stage1MonsterSpawn());
    }

    void Update()
    {
        TerrainMove();
    }

    void Stage1EnemySpawn()
    {
        if (!isBoss)
        {
            
        }
    }

    IEnumerator Stage1MonsterSpawn()
    {
        while (true)
        {
            if (!isBoss)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-15f, 15f), Random.Range(0f, 25f), 150f);
                GameObject temp = Instantiate(Bacteria, spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    void TerrainMove()
    {
        TerrainList.ForEach(item =>
        {
            item.transform.Translate(Vector3.back * terrainMoveSpeed * Time.deltaTime);

            if(item.transform.position.z <= endPos.position.z)
            {
                item.transform.position = spawnPos.position;
            }
        });
    }
}
