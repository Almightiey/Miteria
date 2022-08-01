using UnityEngine;

public class OrbScripts : DistantionAtack
{
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("isAction", true);
    }
}
