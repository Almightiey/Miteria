using UnityEngine;

public class SlashEnemy : OrbScripts, ITickUpdate
{
    public float timeDeath;
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        Destroy(gameObject, timeDeath);
        UpdateManager.ticks.Add(this);
    }


    public void OnUpdate()
    {
        transform.Translate(Vector2.right * transform.localScale.x * speed * Time.deltaTime);
        if (enemy.isDeath) Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitPlayer _))
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UpdateManager.ticks.Remove(this);
    }
}
