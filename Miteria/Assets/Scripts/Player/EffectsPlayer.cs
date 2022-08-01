using UnityEngine;

public class EffectsPlayer : MonoBehaviour
{
    public ParticleSystem dust;
    public ParticleSystem stakes;
    public GameObject hit;
    public GameObject atackEffect;
    public Transform pointEffect;
    internal Player player;

    public void AtackEffect()
    {
        var atackObject = Instantiate(atackEffect, pointEffect.position, Quaternion.identity);
        atackObject.transform.localScale = new Vector3(player.direction, 0.5f, 1);
        var childAtackEffect = atackObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < childAtackEffect.Length; i++)
        {
            childAtackEffect[i].transform.localScale = new Vector3(player.direction, 0.5f, 1);
        }
        Destroy(atackObject, 0.5f);
    }


    public void StakesPlay()
    {
        stakes.Play();
    }


    public void DustPlay()
    {
        dust.Play();
    }
}
