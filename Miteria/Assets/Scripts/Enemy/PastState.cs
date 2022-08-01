using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Past State")]
public class PastState: State
{
    public override void Run()
    {
        if (isFinished)
        {
            return;
        }
        Person.rb.velocity = new Vector2(0, Person.rb.velocity.y);
        Person.animation.SetInteger("State", 0);
        isFinished = Person.isMoving || Person.nextSpawn < Time.time || !Person.isDialog || Person.isDeath;

    }
}
