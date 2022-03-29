using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS 
{
    public enum MoveType
    {
        Horizontal,
        Vertical
    }

    public class MoveSin : MonoBehaviour
    {
        public float amount = 12f;
        public float moveSpeed = 5f;
        public MoveType moveType;

        float move;
        Vector3 originPos;

        void Start()
        {
            originPos = transform.position;
        }

        void Update()
        {

            move += Time.deltaTime * moveSpeed;
            Vector3 position = Vector3.zero;

            switch (moveType)
            {
                case MoveType.Horizontal:
                    position.x = Mathf.Sin(move) * amount + originPos.x;
                    position.y = transform.position.y;
                    break;
                case MoveType.Vertical:
                    position.y = Mathf.Sin(move) * amount + originPos.y;
                    position.x = transform.position.x;
                    break;
            }
            position.z = transform.position.z;

            transform.position = position;
        }
    }

}
