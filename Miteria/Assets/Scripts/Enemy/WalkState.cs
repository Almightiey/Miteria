using UnityEngine;

[CreateAssetMenu]
public class WalkState : State
{
    public override void Run()
    {
        if (isFinished)
        {
            Person.startPosition = Person.transform.position;
            return;
        }
        var rayIsGround = new Ray(Person.Eyes.position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGround.origin, rayIsGround.direction, 2.0f, Person.mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGround.origin, -rayIsGround.direction, 0.5f, Person.mask[2]);
        if (Mathf.Abs(Person.transform.position.x - Person.startPosition.x) > 4.0f || !hitIsGroundDown || hitIsGroundUp)
        {
            Person.Flip(Person.startPosition.x - Person.transform.position.x);
        }
        Person.rb.velocity = new Vector2(Person.transform.localScale.x * Person.speedEnemy / 2, Person.rb.velocity.y);

        Person.animation.SetInteger("State", 1);

        isFinished = Person.isMoving || Person.nextSpawn > Time.time || Person.isAlert || Person.isDialog || Person.isDeath;
    }
}
