using UnityEngine;

public class AttackPlayer : Attack
{
    public delegate void HitEvent(Enemy enemy);
    public event HitEvent hitOfTheEnemy;

    protected override void Init()
    {
        strikerPosition = Game.player.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            hitOfTheEnemy?.Invoke(enemy);
        }
    }
}
