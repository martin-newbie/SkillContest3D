using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Germ : EnemyBase
    {
        [SerializeField] Transform GermBody;
        [SerializeField] Transform ShootPos;
        [SerializeField] EnemyBullet bullet;
        Player player;

        void Start()
        {
            player = FindObjectOfType(typeof(Player)) as Player;
            StartCoroutine(AttackCoroutine());
        }

        void Update()
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            FollowShootPlayer();
        }

        void FollowShootPlayer()
        {
            GermBody.LookAt(player.transform);
        }

        IEnumerator AttackCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                EnemyBullet temp = Instantiate(bullet, ShootPos.position, ShootPos.rotation * Quaternion.Euler(Random.insideUnitSphere * 4f));
                temp.damage = damage;
            }
        }
    }

}
