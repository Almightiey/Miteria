using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Light lightPoint;

    private void Start()
    {
        lightPoint = GetComponentInChildren<Light>();
        lightPoint.enabled = false;
    }

    private void OnBecameVisible()
    {
        lightPoint.enabled = true;
    }

    private void OnBecameInvisible()
    {
        lightPoint.enabled = false;
    }
}
