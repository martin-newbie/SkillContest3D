using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class Bacteria : EnemyBase
    {
        bool isGoingFront = true;

        private void Update()
        {
            if (isGoingFront)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * 4f);
                if (transform.position.z >= 80f) isGoingFront = false;
            }
            else
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
    }
}

