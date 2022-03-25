using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class EnemyBase : MonoBehaviour
    {

        public float Hp;
        public float moveSpeed;
        public float damage;

        public virtual void OnDamage(float damage)
        {
            Hp -= damage;
            StartCoroutine(Stiffen());
            if (Hp <= 0)
            {
                gameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }


        IEnumerator Stiffen()
        {
            float temp = moveSpeed;

            moveSpeed = temp / 2;
            yield return new WaitForSeconds(0.2f);
            moveSpeed = temp;
        }
    }

}

