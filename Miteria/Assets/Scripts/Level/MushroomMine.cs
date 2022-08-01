using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMine : MonoBehaviour, ITickUpdate
{
    public float force;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isDeath;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateManager.ticks.Add(this);
        Action();
    }

    public void Action()
    {
        rb.AddForce(new Vector2(Game.player.direction, 1f) * force, ForceMode2D.Impulse);
    }



    public void OnUpdate()
    {
        if (isDeath)
        {
            UpdateManager.ticks.Remove(this);
            Destroy(gameObject, 1);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("isAction", true);
    }
}
