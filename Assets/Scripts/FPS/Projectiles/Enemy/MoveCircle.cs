using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCircle : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float amount = 12f;
    float move;
    Vector3 originPos;
    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        move += Time.deltaTime * moveSpeed;

        float x = Mathf.Cos(move) * amount + originPos.x;
        float y = Mathf.Sin(move) * amount + originPos.y;
        Vector3 pos = new Vector3(x, y, transform.position.z);

        transform.position = pos;

    }
}
