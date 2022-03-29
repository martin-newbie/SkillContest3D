using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FPS
{
    public class GameManager : Singleton<GameManager>
    {
        bool isBoss;
        [Header("Status")]
        public float PainGauge;
        public float PlayerHp;
        public bool IsGameOver;

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

        [Header("Boss")]
        [SerializeField] GameObject firstBoss;

        void Start()
        {
            StartCoroutine(Stage1MonsterSpawn());
        }

        void Update()
        {
            TerrainMove();
            CheckGameOver();
        }

        void CheckGameOver()
        {
            if (PainGauge >= 100f && !IsGameOver)
            {
                IsGameOver = true;
                GameOver();
            }
            if (PlayerHp <= 0f && !IsGameOver)
            {
                IsGameOver = true;
                GameOver();
            }
        }

        IEnumerator Stage1MonsterSpawn()
        {
            PainGauge = 10f;
            Vector3 spawnPos = new Vector3(0, 15, 90);
            yield return DNA_H_monsterSpawn(spawnPos);
            yield return DNA_V_monsterSpawn(spawnPos);
            yield return Diagonal_monsterSpawn(spawnPos, 10, 1);
            yield return Diagonal_monsterSpawn(spawnPos, 10, -1);
            yield return CircleMove_monsterSpawn(spawnPos, 8);
            yield return Circle_monsterSpawn(spawnPos, 15);
            yield return BossAppear(spawnPos);
        }

        IEnumerator DNA_H_monsterSpawn(Vector3 spawnPos)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject temp = Instantiate(Bacteria, spawnPos, Quaternion.identity);
                MoveSin sin = temp.AddComponent<MoveSin>();
                sin.amount *= 1 + (i * -2);
            }

            yield return new WaitForSeconds(2f);
        }
        IEnumerator DNA_V_monsterSpawn(Vector3 spawnPos)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject temp = Instantiate(Bacteria, spawnPos, Quaternion.identity);
                MoveSin sin = temp.AddComponent<MoveSin>();
                sin.amount *= 1 + (i * -2);
                sin.moveType = MoveType.Vertical;
            }

            yield return new WaitForSeconds(2f);
        }
        IEnumerator Diagonal_monsterSpawn(Vector3 spawnPos, int count, int dir)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject temp = Instantiate(Bacteria, spawnPos, Quaternion.identity);
                MoveSin sin1 = temp.AddComponent<MoveSin>();
                MoveSin sin2 = temp.AddComponent<MoveSin>();

                sin2.moveType = MoveType.Vertical;

                sin2.amount *= dir;

                yield return new WaitForSeconds(0.5f);
            }
        }
        IEnumerator CircleMove_monsterSpawn(Vector3 spawnPos, int count)
        {
            for (int i = 0; i < count; i++)
            {

                GameObject temp = Instantiate(Bacteria, spawnPos, Quaternion.identity);
                MoveCircle circle = temp.AddComponent<MoveCircle>();
                yield return new WaitForSeconds(0.5f);
            }
        }
        IEnumerator Circle_monsterSpawn(Vector3 spawnPos, int count)
        {
            List<GameObject> monsterList = new List<GameObject>();
            for (int i = 0; i < 180; i += 180 / count * 2)
            {
                Vector3 posTemp = spawnPos + (new Vector3(Mathf.Cos(i), Mathf.Sin(i), 0) * 12f);
                Vector3 posTemp2 = spawnPos + (new Vector3(Mathf.Cos(i), Mathf.Sin(i), 0) * -12f);

                GameObject t1 = Instantiate(Bacteria, posTemp, Quaternion.identity);
                GameObject t2 = Instantiate(Bacteria, posTemp2, Quaternion.identity);
                monsterList.Add(t1);
                monsterList.Add(t2);
                yield return new WaitForSeconds(0.2f);
            }

            while (monsterList.Count > 0)
            {
                for (int i = 0; i < monsterList.Count; i++)
                {
                    if (!monsterList[i].activeSelf) monsterList.RemoveAt(i);
                }
                yield return null;
            }
        }
        IEnumerator BossAppear(Vector3 spawnPos)
        {
            firstBoss.SetActive(true);
            while (!firstBoss.GetComponent<FirstBoss>().finished)
            {
                Instantiate(Bacteria, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(5f);
            }
            StartCoroutine(BossDisappear());
        }

        IEnumerator BossDisappear()
        {
            Vector3 originPos = firstBoss.transform.position;
            while (firstBoss.transform.position.y >= -10f)
            {
                originPos += firstBoss.transform.up * -Time.deltaTime;
                firstBoss.transform.position = originPos + Random.insideUnitSphere * 1.2f;
                yield return null;
            }

            firstBoss.gameObject.SetActive(false);
        }

        void TerrainMove()
        {
            TerrainList.ForEach(item =>
            {
                item.transform.Translate(Vector3.back * terrainMoveSpeed * Time.deltaTime);
                if (item.transform.position.z <= endPos.position.z)
                {
                    item.transform.position = spawnPos.position;
                }
            });
        }

        public void GameOver()
        {

        }

        public void GameClear()
        {

        }
    }

}
