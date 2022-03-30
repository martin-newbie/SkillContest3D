using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class SecondBoss : EnemyBase
    {
        [HideInInspector] public Canvas canvas;
        Player player;
        Vector3[] eyePos;
        Animator anim;
        bool divided = false;
        [Header("Objects")]
        [SerializeField] GameObject[] Eyes;
        [SerializeField] Light[] madLight;

        [Header("UI Objects")]
        [SerializeField] GameObject hpBarContainer;
        [SerializeField] Image hpBar;
        [SerializeField] Image hpBar2;
        float fillAmount;

        [Header("Attack")]
        [SerializeField] GameObject straightBullet;
        [SerializeField] GameObject Bacteria;
        [SerializeField] GameObject Virus;

        bool madness;
        float maxHp;
        public bool finished;
        float eyeDelay = 2f;
        int bulletCount = 3;
        bool attackAble = false;
        float patternDelay = 5f;

        void Start()
        {
            canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
            player = FindObjectOfType(typeof(Player)) as Player;
            anim = GetComponent<Animator>();

            StartCoroutine(EyeMoveCoroutine());
            StartCoroutine(AttackCoroutine());
        }

        void Update()
        {
            EyeMovement();
            HpBarControll();
        }

        protected override void DestroyEnemy()
        {
            if (!madness)
            {
                madness = true;
                eyeDelay = 0.5f;
                maxHp *= 1.5f;
                Hp = maxHp;
                madLight[0].gameObject.SetActive(true);
                madLight[1].gameObject.SetActive(true);
                hpBar.color = new Color(1, 0.69f, 0.7f, 1);
                hpBar2.color = new Color(1, 0.2f, 0.2f, 1);
                bulletCount = 5;
            }
            else
            {
                finished = true;
            }
        }

        public override void OnDamage(float damage)
        {
            if (attackAble)
                base.OnDamage(damage);
        }

        IEnumerator Appear()
        {
            transform.position = new Vector3(0f, -25f, 100f);
            while (transform.position.y <= 30f)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 10f);
                yield return null;
            }
            hpBarContainer.SetActive(true);
            attackAble = true;
        }

        void HpBarControll()
        {
            fillAmount = Mathf.Lerp(fillAmount, Hp / maxHp, Time.deltaTime * 10f);
            hpBar.fillAmount = fillAmount;
            hpBar2.fillAmount = Hp / maxHp;
        }

        void EyeMovement()
        {
            Eyes[0].transform.localPosition = Vector3.Lerp(Eyes[0].transform.localPosition, eyePos[0], Time.deltaTime * 10f);
            Eyes[1].transform.localPosition = Vector3.Lerp(Eyes[1].transform.localPosition, eyePos[1], Time.deltaTime * 10f);
        }

        IEnumerator AttackCoroutine()
        {
            yield return StartCoroutine(Appear());
            while (!finished)
            {
                int randFunc = Random.Range(0, 2);
                if (divided)
                {
                    if (randFunc == 0) yield return StartCoroutine(BacteriaSpawnAttack());
                    else
                    {
                        yield return StartCoroutine(PlayerAimAttack(Eyes[0].transform));
                        yield return StartCoroutine(PlayerAimAttack(Eyes[1].transform));
                    }
                }
                else
                {
                    if (randFunc == 0) yield return StartCoroutine(PlayerAimAttack(Eyes[0].transform));
                    else yield return StartCoroutine(VirusSpawnAttack());
                }
                yield return new WaitForSeconds(patternDelay);

                int rand = Random.Range(0, 2);

                if (rand == 0 && !divided) Divided();
                else if (divided) Merged();
            }
            yield return StartCoroutine(Disappear());
        }

        IEnumerator Disappear()
        {
            while (transform.position.y >= -25f)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 3f);
                yield return null;
            }
        }

        IEnumerator PlayerAimAttack(Transform spawnTrans)
        {
            Vector3 spawnPos = spawnTrans.transform.position;
            Vector3 dir = player.transform.position - spawnTrans.transform.position;
            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            for (int i = 0; i < 3; i++)
            {
                Instantiate(straightBullet, spawnPos, rot);
                yield return new WaitForSeconds(0.2f);
            }
        }

        IEnumerator VirusSpawnAttack()
        {
            Vector3 spawnPos = Eyes[0].transform.position;

            for (int i = 0; i < 2; i++)
            {
                Instantiate(Virus, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator BacteriaSpawnAttack()
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 2; i++)
                {
                    Instantiate(Bacteria, Eyes[0].transform.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        void Divided()
        {
            divided = true;
            anim.SetTrigger(0);
        }

        void Merged()
        {
            divided = false;
            anim.SetTrigger(1);
        }

        IEnumerator EyeMoveCoroutine()
        {
            yield return null;

            while (!finished)
            {
                for (int i = 0; i < Eyes.Length; i++)
                {
                    Vector2 randPos = Random.insideUnitCircle * 0.25f;
                    eyePos[i].x = randPos.x;
                    eyePos[i].y = -1f;
                    eyePos[i].z = randPos.y;
                }

                yield return new WaitForSeconds(eyeDelay);
            }
        }
    }

}
