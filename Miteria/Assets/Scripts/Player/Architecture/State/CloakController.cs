using UnityEngine;

[CreateAssetMenu(menuName = "Player/CloakController")]
public class CloakController : PlayerState
{
    private bool bb = false;
    public float jumpForce;
    public PlayerActor playerActor;
    private Vector3 stakesPosition;

    public override void Init()
    {
        base.Init();
        playerActor = Game.mainObjects.playerActor;
        stakesPosition = playerActor.effectsPlayer.stakes.transform.position;
    }

    public override void Run()
    {

        if (player.isGrounded == true)
        {
            bb = false;
        }

        if (player.rb.velocity.y < 0 && Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") < 0 && !player.isStop)
        {
            bb = true;
            player.rb.drag = 0;
            if (!player.isGrounded && Input.GetAxisRaw("Vertical") == -1)
            {
                player.rb.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
                player.animator.SetBool("DashDown", true);
            }
            else
            {
                player.rb.velocity = Vector2.zero;
            }
        }
        else if (!player.isGrounded && player.rb.velocity.y < 0.5f && !bb)
        {
            player.rb.drag = 15;
        }
    

        if (player.rb.velocity.y > 0)
        {
            player.rb.drag = 0;
        }

        if (player.animator.GetBool("DashDown") && player.isGrounded)
        {
            playerActor.effectsPlayer.stakes.transform.position = stakesPosition;
            playerActor.effectsPlayer.StakesPlay();
            player.animator.SetBool("DashDown", false);
        }
    }
}
