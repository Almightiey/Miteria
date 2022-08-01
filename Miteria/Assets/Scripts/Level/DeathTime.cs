using UnityEngine;

public class DeathTime : MonoBehaviour
{
    public float timeLife = 1.5f;
    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }


    public void Delete()
    {
        Destroy(gameObject);
    }
}
