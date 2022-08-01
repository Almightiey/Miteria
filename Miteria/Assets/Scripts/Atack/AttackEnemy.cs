using UnityEngine;

public class AttackEnemy : Attack, ITickUpdate
{
    private Enemy enemy;

    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
        Game.endGame += EndGame;
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        strikerPosition = enemy.transform.position;

    }

    public void OnUpdate()
    {
        if (enemy != null)
        {
            strikerPosition = enemy.transform.position;
        }
    }


    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
    }

    public void EndGame()
    {
        Game.endGame -= EndGame;
        UpdateManager.ticks.Remove(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent( out HitPlayer hitPlayer))
        {
            if (!hitPlayer.isItInvulnerable && attackEffect != null)
            {
                Instantiate(attackEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
