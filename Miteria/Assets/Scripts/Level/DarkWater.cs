using UnityEngine;

public class DarkWater : MonoBehaviour
{
    //public ParticleSystem waterEffect;
    public AudioSource audioSource;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();
        //Instantiate(waterEffect, new Vector3(collision.transform.position.x, collision.transform.position.y - (collision.transform.localScale.y / 2), 0), Quaternion.identity);
    }

}
