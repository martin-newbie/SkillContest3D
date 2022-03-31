using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class Friendly : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] GameObject Mesh;
        float rotX, rotY, rotZ;
        public float Hp;

        private void Update()
        {
            rotX += Time.deltaTime * moveSpeed;
            rotY += Time.deltaTime * moveSpeed;
            rotZ += Time.deltaTime * moveSpeed;

            Mesh.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        public virtual void OnDamage(float damage)
        {
            Hp -= damage;

            if(Hp <= 0f)
            {
                DestroyObject();
            }
        }

        protected virtual void DestroyObject()
        {
            Destroy(gameObject, 5f);
            gameObject.SetActive(false);
        }
    }

}
