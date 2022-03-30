using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace FPS
{
    public class GameManager : Singleton<GameManager>
    {
        bool scoreCount = false;
        float curScoreDelay = 0f;
        float maxScoreDelay = 2f;
        bool isBoss;

        [Header("Status")]
        public float PainGauge;
        public float PlayerHp;
        public bool IsGameOver;
        public float Score 
        {
            get
            {
                return score;
            }
            set
            {
                curScoreDelay = maxScoreDelay;
                scoreCount = true;
                tempScore += value;
            }
        }
        private float score;
        float tempScore;
        [SerializeField] Text TempScore;
        [SerializeField] Text RealScore;
        [SerializeField] Player player;

        [Header("Map Move")]
        public List<GameObject> terrain = new List<GameObject>();
        public float terrainMoveSpeed;

        [Header("Monster")]
        [SerializeField] GameObject Bacteria;
        [SerializeField] GameObject Virus;

        [Header("Boss")]
        public int Stage = 1;
        [SerializeField] GameObject Boss_1;
        [SerializeField] GameObject Boss_2;

        void Start()
        {

            if (Stage == 1)
            {
                StartCoroutine(Stage1MonsterSpawnCoroutine());
                PainGauge = 10f;
            }
            else
            {
                StartCoroutine(Stage2MonsterSpawnCoroutine());
                PainGauge = 30f;
            }

            RealScore.text = score.ToString();
        }

        void Update()
        {
            CheckGameOver();
            TerrainMove();
            ScoreLogic();

            if (Input.GetKeyDown(KeyCode.Space)) Score = 100f;
        }

        void ScoreLogic()
        {
            if (scoreCount)
            {
                TempScore.gameObject.SetActive(true);
                TempScore.text = tempScore.ToString();
                curScoreDelay -= Time.deltaTime;
                if (curScoreDelay <= 0f)
                {
                    score += tempScore;
                    scoreCount = false;
                    tempScore = 0f;
                    RealScore.text = score.ToString();
                }
            }
            else TempScore.gameObject.SetActive(false); 

            if(score >= 1000 * Stage && !isBoss)
            {
                BossAppear();
            }
        }

        void BossAppear()
        {
            isBoss = true;

            if (Stage == 1)
                Boss_1.SetActive(true);
            else Boss_2.SetActive(true);
        }

        void TerrainMove()
        {
            terrain.ForEach(item =>
            {
                if (item.transform.position.z <= -450 - 10f) item.transform.position = new Vector3(-200f, 0f, 1150f - 10f);
                item.transform.Translate(Vector3.back * terrainMoveSpeed * Time.deltaTime);
            });
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

        IEnumerator Stage1MonsterSpawnCoroutine()
        {
            while (!isBoss)
            {
                MonsterSpawn(Bacteria);
                yield return new WaitForSeconds(2f);
            }
        }

        IEnumerator Stage2MonsterSpawnCoroutine()
        {
            while (!isBoss)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                    MonsterSpawn(Bacteria);
                else if (rand == 1)
                    MonsterSpawn(Virus);

                yield return new WaitForSeconds(2f);
            }
        }

        void MonsterSpawn(GameObject monster)
        {
            Vector3 pos = player.transform.position + new Vector3(Random.Range(-30f, 30f), Random.Range(-10f, 25f), 0f);
            GameObject temp = Instantiate(monster, pos, Quaternion.identity);
        }

        public void GameOver()
        {

        }

        public void GameClear()
        {
            UIManager.Instance.GameClearUI(score, PlayerHp, 100 - PainGauge);
        }
    }

}
