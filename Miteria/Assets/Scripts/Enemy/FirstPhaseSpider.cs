using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/FirstPhaseSpider")]
public class FirstPhaseSpider : State
{
    public float jumpForce;
    public float delay;
    private float next;
    public override void Run()
    {
        if (isFinished)
        {
            return;
        }
        if (next < Time.time)
        {
            MiddlePerson.randomIndexOrbs = (int)System.Math.Round((decimal)Random.Range(0, (MiddlePerson.orbs.Length - 1) * 100) / 100);
            next = Time.time + delay;
        }
        var rayIsGroundFront = new Ray(Person.Eyes.position, Vector2.down);
        var rayIsGroundBack = new Ray(Person.BackEyes.position, Vector2.down);
        var hitIsGroundDown = Physics2D.Raycast(rayIsGroundFront.origin, rayIsGroundFront.direction, 3, Person.mask[2])
         || Physics2D.Raycast(rayIsGroundBack.origin, rayIsGroundBack.direction, 3, Person.mask[2]);
        var hitIsGroundUp = Physics2D.Raycast(rayIsGroundFront.origin, -rayIsGroundFront.direction, 1, Person.mask[2])
         || Physics2D.Raycast(rayIsGroundBack.origin, -rayIsGroundBack.direction, 1, Person.mask[2]);

        var distantion = Game.player.transform.position.x - Person.transform.position.x;
        var direction = distantion / Mathf.Abs(distantion);
        if (Person.nextDelayMove > Time.time)
        {
            Person.animation.SetInteger("State", 1);
            if (hitIsGroundUp && Person.rb.velocity.y == 0)
            {
                var hitIsGroundUpFront = Physics2D.Raycast(rayIsGroundFront.origin, -rayIsGroundFront.direction, 1, Person.mask[2]);
                var hitIsGroundUpBack = Physics2D.Raycast(rayIsGroundBack.origin, -rayIsGroundBack.direction, 1, Person.mask[2]);
                var speed = hitIsGroundUpFront ? -1 : hitIsGroundUpBack ? 1 : 0;
                Person.rb.AddForce(new Vector2(Person.transform.localScale.x * speed, 1) * jumpForce, ForceMode2D.Impulse);
            }
            else if (hitIsGroundDown)
            {
                if (Person.speedEnemy != 0 && Mathf.Abs(distantion) < 5.4f)
                {
                    Person.rb.velocity = new Vector2(direction * -Person.speedEnemy, Person.rb.velocity.y);
                }
                else if (Person.speedEnemy != 0 && Mathf.Abs(distantion) > 7.0f)
                {
                    Person.rb.velocity = new Vector2(direction * Person.speedEnemy, Person.rb.velocity.y);
                }
            }
            else
            {
                Person.rb.velocity = new Vector2(0, Person.rb.velocity.y);
                Person.animation.SetInteger("State", 0);
            }
        }
        else
        {
            Person.Flip(Game.player.transform.position.x - Person.transform.position.x);
            Person.rb.velocity = new Vector2(0, Person.rb.velocity.y);
            Person.animation.SetInteger("State", 2);
        }

        isFinished = !Person.isMoving || Person.health < 150 || Game.player.isDeath;
    }
}
