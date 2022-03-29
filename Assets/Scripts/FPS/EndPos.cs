using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FPS
{
    public class EndPos : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyBullet"))
            {
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                GameManager.Instance.PainGauge += other.GetComponent<EnemyBase>().damage / 2;
                other.GetComponent<EnemyBase>().OnDamage(other.GetComponent<EnemyBase>().Hp);
            }
        }
    }

}
