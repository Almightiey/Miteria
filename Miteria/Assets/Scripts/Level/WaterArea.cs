using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour
{
    public int pointPerUnits = 5;
    public Vector2 offset;
    public Vector2 size = new Vector2(6f, 2f);

    public int sortingLayerID;
    public int sortingLayerOrder = 3;

    public float dampening = 0.93f;
    public float tension = 0.025f;
    public float neighbourTransfer = 0.03f;

    public GameObject splashPlayerPrefab;
}
