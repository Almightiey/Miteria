using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABWave : MonoBehaviour
{
    public Transform A;
    public Transform B;

    [Range(0, 1)]
    public float time;

    public float freq = 30.0f; //частота волны
    public float waveScale = 1.0f;//размер волны


    private Vector2 WaveLerp(Vector2 a, Vector2 b, float time, float waveScale = 1.0f, float freq = 1.0f)
    {
        Vector2 result = Vector2.Lerp(a, b, time);
        Vector2 dir = (b - a).normalized;
        Vector2 leftnormal = result + new Vector2(-dir.y, dir.x) * waveScale;

        result = Vector2.LerpUnclamped(result, leftnormal, Mathf.Cos(time * freq));
        return result;
    }


    private void OnDrawGizmos()
    {
        if (A == null || B == null) return;

        Vector2 res = WaveLerp(A.position, B.position, time, waveScale, freq);
        Gizmos.DrawSphere(res, 1.0f);

        int points = 100;
        for (int i = 0; i < points; i++)
        {
            res = WaveLerp(A.position, B.position, (1 / points) * i, waveScale, freq);
            Gizmos.DrawSphere(res, 0.1f);
        }
    }
}
