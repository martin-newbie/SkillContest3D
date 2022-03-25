using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class EnemyBullet : MonoBehaviour
    {
        public float damage;
        [SerializeField] float moveSpeed = 15f;

        void Start()
        {
            DestroyBullet();
        }

        void Update()
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {

                Destroy(gameObject);
            }
        }

        void DestroyBullet()
        {
            Destroy(gameObject, 15f);
        }
    }

}
