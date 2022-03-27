using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class EnemyBase : MonoBehaviour
    {

        public float Hp;
        public float moveSpeed;
        float defaultSpeed;
        public float damage;

        void Start()
        {
            defaultSpeed = moveSpeed;
        }

        public virtual void OnDamage(float damage)
        {
            Hp -= damage;
            StartCoroutine(Stiffen());
            if (Hp <= 0)
            {
                DestroyEnemy();
            }
        }

        protected virtual void DestroyEnemy()
        {
            Destroy(gameObject, 5f);
            gameObject.SetActive(false);
        }

        protected virtual IEnumerator Stiffen()
        {
            float temp = defaultSpeed;

            moveSpeed = temp / 2;
            yield return new WaitForSeconds(0.2f);
            moveSpeed = temp;
        }
    }

}

