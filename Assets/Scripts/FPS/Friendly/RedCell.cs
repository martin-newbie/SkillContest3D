using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class RedCell : Friendly
    {


        void Start()
        {

        }

        public override void OnDamage(float damage)
        {
            base.OnDamage(damage);
            GameManager.Instance.PainGauge += 5f;
        }
    }

}
