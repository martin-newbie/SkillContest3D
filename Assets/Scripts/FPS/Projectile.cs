using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 15f;
    }
}
