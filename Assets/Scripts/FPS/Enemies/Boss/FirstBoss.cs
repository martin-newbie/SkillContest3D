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
        [SerializeField] MeshRenderer mainMesh;
        [SerializeField] Material damagedMat;
        [SerializeField] Light madLight;

        [Header("UI Objects")]
        [SerializeField] GameObject hpBarContainer;
        [SerializeField] Image hpBar;
        [SerializeField] Image hpBar2;
        float fillAmount;

        [Header("Bullet")]
        [SerializeField] GameObject straightBullet;
        [SerializeField] GameObject SinVBullet;
        [SerializeField] GameObject SinHBullet;
        [SerializeField] GameObject SinHVBullet;

        bool madness;
        float maxHp;

        void Start()
        {
            canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
            player = FindObjectOfType(typeof(Player)) as Player;
            StartCoroutine(EyeMoveCoroutine(2f));
            maxHp = Hp;

            FollowAttack();
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
                maxHp *= 1.5f;
                Hp = maxHp;
                madLight.gameObject.SetActive(true);
                hpBar.color = new Color(1, 0.69f, 0.7f, 1);
                hpBar2.color = new Color(1, 0.2f, 0.2f, 1);
            }
            else
            {
                hpBarContainer.SetActive(false);
                Destroy(this.gameObject);
            }
        }

        IEnumerator EyeMoveCoroutine(float delay)
        {
            yield return null;

            while (true)
            {
                Vector2 randPos = Random.insideUnitCircle * 0.25f;
                eyePos.x = randPos.x;
                eyePos.y = -0.1f;
                eyePos.z = randPos.y;

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
