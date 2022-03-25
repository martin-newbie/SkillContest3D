using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Missile : Projectile
    {
        [SerializeField] Explosion explosion;

        void Start()
        {

        }

        void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
            {/*
                other.GetComponentInParent<EnemyBase>().OnDamage(damage);*/
                Explosion temp = Instantiate(explosion, transform.position, Quaternion.identity);
                temp.damage = damage;
                Destroy(this.gameObject);
            }
        }
    }

}
