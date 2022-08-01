using UnityEngine;

[CreateAssetMenu(menuName = "Player/LungePlayer")]
public class LungePlayer : PlayerState
{

    public float somersaultForce;
    public float somersaultDelay;
    public float somersaultTimeDelay;
    private float somersaultTime;
    private float nextSomersault;
    private bool isSomersault;
    private HitPlayer hitDamage;


    public override void Init()
    {
        somersaultTime = 0.0f;
        nextSomersault = 0.0f;
        hitDamage = player.hitDamage;
    }


    public override void Run()
    {
        isSomersault = somersaultTime > Time.time;
        player.animator.SetBool(nameof(isSomersault), isSomersault);
        if (Input.GetButton("Down") && nextSomersault < Time.time && !Input.GetButtonDown("Jump"))
        {
            player.physicsObject.targetVelocity = Vector2.zero;
            //rb.velocity = Vector2.right * force * somersaultForce * direction;
            nextSomersault = somersaultDelay + Time.time;
            somersaultTime = somersaultTimeDelay + Time.time;
        }
        hitDamage.isItInvulnerable = somersaultTime > Time.time || hitDamage.isRecoil;
        if (somersaultTime > Time.time)
        {
            //player.rb.AddForce(Vector2.right * player.direction * somersaultForce);
            player.physicsObject.velocity.x = player.direction * somersaultForce;
            player.physicsObject.velocity.y = 0;
            player.physicsObject.noAirResistance = true;
            player.physicsObject.gravityModifier = 0;
        }
        else if (Time.time < somersaultTime + 0.1f)
        {
            player.physicsObject.gravityModifier = 1;
            player.physicsObject.noAirResistance = false;
        }
        //else if (!hitUp && !isSit)
        //{
        //    isItInvulnerable = false;
        //    playerActror.player.rb.gravityScale = 1;
        //}
    }
}
