using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float health = 100;
    public Slider sliderHealth;
    public GameObject personObject;

    private void Start()
    {
        sliderHealth.maxValue = health;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (personObject != null)
        {
            if (other.GetComponent<AttackPlayer>())
            {
                GetDamaged(other);
            }
        }
    }
    public void GetDamaged(Collider2D collision)
    {
        sliderHealth.gameObject.SetActive(true);
        var atackPlayer = collision.GetComponent<AttackPlayer>();
        health -= atackPlayer.TakeDamage();
        sliderHealth.value = health;
        if (health <= 0)
        {
            Destroy(personObject);
        }
    }
}
