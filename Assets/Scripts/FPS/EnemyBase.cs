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
        public float score = 10f;

        void Start()
        {
            defaultSpeed = moveSpeed;
        }

        public virtual void OnDamage(float damage)
        {
            Hp -= damage;
            if (Hp <= 0)
            {
                DestroyEnemy();
            }
        }

        protected virtual void DestroyEnemy()
        {
            GameManager.Instance.Score = score;
            Destroy(gameObject, 5f);
            gameObject.SetActive(false);
        }
    }

}

