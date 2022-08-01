using UnityEngine;

public class Attack : MonoBehaviour
{
    public float attack;
    public float recoil;
    public Vector3 strikerPosition;
    public GameObject attackEffect;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        strikerPosition = GetComponentInParent<Body>().transform.position;
    }

    public float TakeDamage()
    {
        return attack;
    }
}
