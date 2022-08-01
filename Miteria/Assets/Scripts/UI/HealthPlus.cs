using UnityEngine;

public class HealthPlus : MonoBehaviour
{
    public float hpPlus;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitPlayer>())
        {
            Destroy(gameObject);
        }
    }
}
