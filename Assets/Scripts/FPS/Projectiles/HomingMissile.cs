using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class HomingMissile : Projectile
    {
        Vector3[] m_points = new Vector3[4];

        float timerMax = 0f;
        float timerCur = 0f;
        float speed;

        public void Init(Transform startPos, Transform endPos, float _speed, float newPointDistanceFromStart, float newPointDistanceFromEnd)
        {
            speed = _speed;

            timerMax = Random.Range(0.8f, 1f);

            m_points[0] = startPos.position;

            m_points[1] = startPos.position +
                (newPointDistanceFromStart * Random.Range(-1f, 1f) * startPos.right) +
                (newPointDistanceFromStart * Random.Range(-0.15f, 1f) * startPos.up) +
                (newPointDistanceFromStart * Random.Range(-1f, 0.8f) * startPos.forward);

            m_points[2] = endPos.position +
                (newPointDistanceFromEnd * Random.Range(-1f, 1f) * endPos.right) +
                (newPointDistanceFromEnd * Random.Range(-1f, 1f) * endPos.up) +
                (newPointDistanceFromEnd * Random.Range(0.8f, 1f) * endPos.forward);

            m_points[3] = endPos.position;

            transform.position = startPos.position;
        }

        void Update()
        {
            if (timerCur > timerMax) Destroy(gameObject);

            timerCur += Time.deltaTime * speed;

            transform.position = new Vector3(
                CubicBezierCurve(m_points[0].x, m_points[1].x, m_points[2].x, m_points[3].x),
                CubicBezierCurve(m_points[0].y, m_points[1].y, m_points[2].y, m_points[3].y),
                CubicBezierCurve(m_points[0].z, m_points[1].z, m_points[2].z, m_points[3].z)
                );
        }

        float CubicBezierCurve(float a, float b, float c, float d)
        {
            float t = timerCur / timerMax;

            float ab = Mathf.Lerp(a, b, t);
            float bc = Mathf.Lerp(b, c, t);
            float cd = Mathf.Lerp(c, d, t);

            float abbc = Mathf.Lerp(ab, bc, t);
            float bccd = Mathf.Lerp(bc, cd, t);

            return Mathf.Lerp(abbc, bccd, t);
        }
    }

}