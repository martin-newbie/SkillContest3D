using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Explosion : MonoBehaviour
    {
        public float damage;

        private void Start()
        {
            Destroy(gameObject, 1f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<EnemyBase>().OnDamage(damage);
            }
        }
    }
}
