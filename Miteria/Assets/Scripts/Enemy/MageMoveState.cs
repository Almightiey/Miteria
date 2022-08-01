using UnityEngine;

[CreateAssetMenu]
public class MageMoveState : State
{
    public override void Run()
    {
        if (isFinished)
        {
            return;
        }
        Person.nextDelay = Person.canDelay + Time.time;
        var rayIsGround = new Ray(Person.Eyes.position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, 3, Person.mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, 1, Person.mask[2]);



        if (Mathf.Abs(Game.player.transform.position.x - Person.transform.position.x) < 4.5f && hitIsGroundDown && !hitIsGroundUp
            && Person.speedEnemy != 0 && Person.nextDelayMove > Time.time)
        {
            Person.rb.velocity = new Vector2(Person.transform.localScale.x * -Person.speedEnemy, Person.rb.velocity.y);
            Person.animation.SetInteger("State", 1);
        }
        else if (Person.nextDelayMove < Time.time)
        {
            Person.rb.velocity = Vector2.zero;
            Person.animation.SetInteger("State", 2);
        }
        else
        {
            Person.animation.SetInteger("State", 0);
        }

        isFinished = !Person.isMoving || Person.isDialog || Person.isDeath;
    }
}
