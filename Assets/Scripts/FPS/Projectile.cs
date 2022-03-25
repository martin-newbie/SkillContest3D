using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float damage = 3f;
        [SerializeField] protected float moveSpeed = 15f;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<EnemyBase>().OnDamage(damage);

                Destroy(this.gameObject);
            }
            if (other.CompareTag("Wall"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
