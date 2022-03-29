using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class FirstBoss : EnemyBase
    {
        [HideInInspector] public Canvas canvas;
        Player player;
        Vector3 eyePos;
        [Header("Objects")]
        [SerializeField] GameObject Eye;
        [SerializeField] Light madLight;

        [Header("UI Objects")]
        [SerializeField] GameObject hpBarContainer;
        [SerializeField] Image hpBar;
        [SerializeField] Image hpBar2;
        float fillAmount;

        [Header("Monsters")]
        [SerializeField] GameObject Germ;

        [Header("Bullet")]
        [SerializeField] GameObject straightBullet;
        [SerializeField] GameObject SinVBullet;
        [SerializeField] GameObject SinHBullet;
        [SerializeField] GameObject SinHVBullet;

        bool madness;
        float maxHp;
        public bool finished;
        float eyeDelay = 2f;

        void Start()
        {
            canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
            player = FindObjectOfType(typeof(Player)) as Player;
            StartCoroutine(EyeMoveCoroutine());
            maxHp = Hp;

        }

        void Update()
        {
            EyeMove();
            HpBarControll();
        }

        void HpBarControll()
        {
            fillAmount = Mathf.Lerp(fillAmount, Hp / maxHp, Time.deltaTime * 10f);
            hpBar.fillAmount = fillAmount;
            hpBar2.fillAmount = Hp / maxHp;
        }

        void FollowAttack()
        {
            Vector3 dir = player.transform.position - Eye.transform.position;
            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            GameObject temp = Instantiate(straightBullet, Eye.transform.position, rot);
            temp.GetComponent<MoveForward>().moveSpeed = 15f;
        }

        void MonsterSpawn()
        {
            GameObject temp = Instantiate(Germ, Eye.transform.position, Quaternion.identity);
        }

        void EyeMove()
        {
            Eye.transform.localPosition = Vector3.Lerp(Eye.transform.localPosition, eyePos, Time.deltaTime * 10f);
        }

        protected override void DestroyEnemy()
        {
            if (!madness)
            {
                madness = true;
                eyeDelay = 0.5f;
                maxHp *= 1.5f;
                Hp = maxHp;
                madLight.gameObject.SetActive(true);
                hpBar.color = new Color(1, 0.69f, 0.7f, 1);
                hpBar2.color = new Color(1, 0.2f, 0.2f, 1);
            }
            else
            {
                finished = true;
            }
        }
        IEnumerator EyeMoveCoroutine()
        {
            yield return null;

            while (!finished)
            {
                Vector2 randPos = Random.insideUnitCircle * 0.25f;
                eyePos.x = randPos.x;
                eyePos.y = -1f;
                eyePos.z = randPos.y;
                int rand = Random.Range(0, 2);
                if (rand == 0) FollowAttack();
                else MonsterSpawn();

                yield return new WaitForSeconds(eyeDelay);
            }
        }
    }
}
