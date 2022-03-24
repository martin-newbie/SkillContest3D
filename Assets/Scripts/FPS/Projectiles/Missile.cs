using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Missile : Projectile
    {
        void Start()
        {

        }

        void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall"))
            {
                Destroy(this.gameObject);
            }
        }
    }

}
