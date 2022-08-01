using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Move State")]
public class MoveState : State
{
    public float distanceForAttack;
    public override void Run()
    {
        if (isFinished)
        {
            return;
        }
        var rayIsGround = new Ray(Person.Eyes.position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, 3,Person.mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, 1, Person.mask[2]);


        if (Person.nextDelayMove > Time.time)
        {
            Person.rb.velocity = new Vector2(0, Person.rb.velocity.y);
            Person.animation.SetInteger("State", 0);
        }
        else if (Mathf.Abs(Game.player.transform.position.x - Person.transform.position.x) > distanceForAttack && hitIsGroundDown && !hitIsGroundUp && Person.speedEnemy != 0)
        {
            Person.rb.velocity = new Vector2(Person.transform.localScale.x * Person.speedEnemy, Person.rb.velocity.y);
            Person.animation.SetInteger("State", 1);

        }
        else
        {
            Person.rb.velocity = new Vector2(0, Person.rb.velocity.y);
            Person.animation.SetInteger("State", 2);
        }

        isFinished = !Person.isMoving || Person.isDialog || Person.isDeath || Game.player.isDeath;
    }
}
