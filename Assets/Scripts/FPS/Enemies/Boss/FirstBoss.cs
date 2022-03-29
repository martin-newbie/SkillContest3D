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

        [Header("Bullet")]
        [SerializeField] GameObject straightBullet;

        bool madness;
        float maxHp;
        public bool finished;
        float eyeDelay = 2f;

        void Start()
        {
            canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
            player = FindObjectOfType(typeof(Player)) as Player;
            StartCoroutine(EyeMoveCoroutine());
            StartCoroutine(BossAttack());
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

        void DefaultAttack()
        {
            Vector3 spawnPos = Eye.transform.position;

            Vector3 dir = player.transform.position - Eye.transform.position;
            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            int offset = 120 / 5;
            for (int i = offset * -2; i < 120 - 2 * offset; i += offset)
            {
                GameObject temp = Instantiate(straightBullet, spawnPos, rot * Quaternion.Euler(0, i, 0));
                temp.GetComponent<MoveForward>().moveSpeed = 30f;
            }

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

        IEnumerator BossAttack()
        {
            while (!finished)
            {
                DefaultAttack();
                yield return new WaitForSeconds(1f);
            }

            GameManager.Instance.GameClear();
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

                yield return new WaitForSeconds(eyeDelay);
            }
        }
    }
}
