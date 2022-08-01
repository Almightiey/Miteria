using System.Threading.Tasks;
using UnityEngine;

public class EffectRune
{
    public int delay;
    public float next;
    public float damage;


    public async void HitEffect(Enemy enemy)
    {
        while (next > Time.time && enemy.health > 0)
        {
            enemy.health -= damage;
            enemy.ChangeHealth();
            await Task.Delay(delay * 1000);
        }
    }

}
