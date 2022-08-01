using UnityEngine;


[CreateAssetMenu(menuName = "Player/JumpState")]
public class JumpState : PlayerState
{
    public float jumpForce;
    public float jumpTime;
    public static bool isJumping;
    public static bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    internal bool isWallFront;
    private float jumpTimeCounter;
    public PlayerActor playerActor;
    private AudioSource audioMove;

    public override void Init()
    {
        base.Init();
        playerActor = Game.mainObjects.playerActor;
        audioMove = playerActor.gameObject.AddComponent<AudioSource>();
    }


    public override void Run()
    {
        audioMove.Stop();
        bool isAir = false;
        if (!player.isGrounded)
        {
            isAir = true;
            if (player.physicsObject.velocity.y < 0)
            {
                player.animator.SetFloat("velocityY", player.physicsObject.velocity.y);
            }
        }

        var hit = Physics2D.Raycast(player.transform.position, Vector2.down, checkRadius, whatIsGround);
        player.isGrounded = hit;
        if (player.isGrounded && isAir)
        {
            playerActor.effectsPlayer.DustPlay();
            if (hit.collider.TryGetComponent(out AudioTriggerMove audioTrigger))
            {
                audioMove.volume = audioTrigger.volume;
                audioMove.clip = audioTrigger.audioClip;
                if (!audioMove.isPlaying)
                    audioMove.Play();
            }
        }

        if (Game.isInvertoryOpen)
            return;

        if (player.isGrounded && Input.GetButton("Jump") && !player.hitDamage.isNoActionOtherAnimation)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            //rb.velocity = isWallFront ? new Vector2(direction, 1) * jumpForce : Vector2.up * jumpForce;
            //player.physicsObject.velocity = new Vector2(player.direction, 1) * jumpForce;
            player.physicsObject.velocity.y = jumpForce;
            playerActor.effectsPlayer.DustPlay();
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                //rb.velocity = Vector2.up * jumpForce;
                //player.rb.velocity = Vector2.up * jumpForce;
                player.physicsObject.velocity.y = jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        player.animator.SetBool(nameof(isGrounded), isGrounded);
        player.animator.SetBool(nameof(isJumping), !player.isGrounded);
    }
}
