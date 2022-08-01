using UnityEngine;

[CreateAssetMenu(menuName = "Player/DushPlayer")]
public class DushPlayer : PlayerState
{
    public float dashForce;
    private float dashTime;
    public float dashTimeDelay;
    public float dashDelay;
    private float dashNextDelay;
    internal bool isDash;
    private HitPlayer hitDamage;
    public PlayerActor playerActor;

    public override void Init()
    {
        dashNextDelay = 0;
        dashTime = 0;
        isDash = false;
        playerActor = Game.mainObjects.playerActor;
        hitDamage = player.hitDamage;
    }

    public override void Run()
    {
        Vector2 move = Vector2.zero;
        isDash = dashTime > Time.time;
        player.animator.SetBool(nameof(isDash), isDash);
        player.isInvulnerable = isDash;
        if (Input.GetButton("Back") && dashNextDelay < Time.time && !Input.GetButtonDown("Jump"))
        {
            //player.rb.velocity = Vector2.zero;
            //rb.velocity = Vector2.right * force * somersaultForce * direction;
            if (player.isGrounded)
                playerActor.effectsPlayer.DustPlay();
            dashNextDelay = dashDelay + Time.time;
            dashTime = dashTimeDelay + Time.time;
        }
        hitDamage.isItInvulnerable = dashTime > Time.time || hitDamage.isRecoil;
        if (dashTime > Time.time)
        {
            player.animator.SetTrigger("Dash");
            player.Flip(player.areaAtack.pointAttack.position.x - player.transform.position.x);
            player.physicsObject.velocity.x = -player.direction * dashForce;
            player.physicsObject.noAirResistance = true;
            //if (player.isGrounded)
            //{
            //    player.Flip(player.areaAtack.pointAtatck.position.x - player.transform.position.x);
            //    //player.rb.AddForce(new Vector2(1 * -player.direction, 0.03f) * dashForce);
            //    player.physicsObject.velocity = new Vector2(1 * -player.direction, 0.03f) * dashForce;
            //}
            //else
            //{
            //    //playerActror.player.Flip(areaAtack.pointAtatck.position.x - playerActror.player.transform.position.x);
            //    //player.rb.AddForce(Vector2.right * -player.direction * dashForce);
            //    player.physicsObject.velocity = Vector2.right * -player.direction * dashForce;
            //}
        }
        else if (Time.time < dashTime + 0.1f)
        {
            player.physicsObject.noAirResistance = false;
        }
    }
}
