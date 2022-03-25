using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Virus : EnemyBase
    {
        [SerializeField] EnemyBullet bullet;
        [SerializeField] Transform virusMesh;
        [SerializeField] List<Transform> bulletPos = new List<Transform>();

        float rotX, rotY, rotZ;
        private void Start()
        {
            StartCoroutine(BulletSpawn());
        }

        IEnumerator BulletSpawn()
        {
            while (true)
            {
                Transform pos = bulletPos[Random.Range(0, bulletPos.Count)];

                EnemyBullet temp = Instantiate(bullet, pos.position, pos.rotation);
                temp.damage = damage;
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void Update()
        {
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);

            rotX += Time.deltaTime * 100f;
            rotY += Time.deltaTime * 100f;
            rotZ += Time.deltaTime * 100f;

            virusMesh.rotation = Quaternion.Euler(rotX, rotY, rotZ);
        }
    }

}
