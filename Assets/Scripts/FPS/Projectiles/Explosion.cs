using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Explosion : MonoBehaviour
    {
        public float damage;
        [SerializeField]
        ParticleSystem explosionParticle;

        private void Start()
        {
            explosionParticle.Play();
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
