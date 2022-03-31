using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class Projectile : MonoBehaviour
    {
        public float damage = 3f;
        [SerializeField] protected float moveSpeed = 15f;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<EnemyBase>().OnDamage(damage);
                DestroyBullet();
            }

            if(other.CompareTag("Friendly"))
            {
                other.GetComponentInParent<Friendly>().OnDamage(damage);
                DestroyBullet();
            }

            if (other.CompareTag("Wall"))
            {
                DestroyBullet();
            }
        }

        protected virtual void DestroyBullet()
        {
            Destroy(this.gameObject);
        }
    }
}
