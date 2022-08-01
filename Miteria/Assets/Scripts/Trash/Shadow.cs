using UnityEngine;

public class Shadow : MonoBehaviour
{
    internal Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
}
