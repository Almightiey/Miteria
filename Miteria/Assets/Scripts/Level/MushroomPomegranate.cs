using System.Threading.Tasks;
using UnityEngine;

public class MushroomPomegranate: Body, ITickUpdate
{
    public float force;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isDeath;
    public bool isAction = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        UpdateManager.ticks.Add(this);
        if (isAction)
            Action();
    }

    public async void Action()
    {
        var collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        rb.gravityScale = 1;
        rb.AddForce(new Vector2(Game.player.direction, 1f) * force, ForceMode2D.Impulse);
        await Task.Delay(300);
        collider.enabled = true;
    }



    public void OnUpdate()
    {
        if (isDeath)
        {
            UpdateManager.ticks.Remove(this);
            Destroy(gameObject, 1);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Body _))
        {
            isAction = true;
        }
        if (collision.TryGetComponent(out Attack _))
        {
            isAction = true;
        }
        animator.SetBool(nameof(isAction), isAction);
    }
}
