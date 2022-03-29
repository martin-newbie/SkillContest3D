using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class PlaneEnemy : EnemyBase
    {

        void Start()
        {

        }

        void Update()
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }

}
