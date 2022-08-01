using UnityEngine;

[CreateAssetMenu(menuName = "Player/MovePlayer")]
public class MovePlayer : PlayerState
{
    public AudioClip attackClip;
    public AudioClip attackClipMove;
    public float speed;
    public LayerMask mask;
    private AudioSource audioAttack;
    private AudioSource audioMove;

    public override void Init()
    {
        player.speed = speed;
        var playerActor = Game.mainObjects.playerActor;
        audioAttack = player.atackPlayer.GetComponent<AudioSource>();
        audioMove = playerActor.gameObject.AddComponent<AudioSource>();
    }

    public override void Run()
    {
        //Бег 
        Vector2 move = Vector2.zero;
        move.x = !player.hitDamage.isNoActionOtherAnimation ? Input.GetAxis("Horizontal") : 0;
        player.Flip(move.x);
        player.animator.SetFloat(nameof(speed), Mathf.Abs(player.physicsObject.velocity.x));
        //nextFlip = isWallFront ? Time.time + flipDelay : nextFlip;
        if (player.physicsObject.velocity.x != 0 && player.isGrounded)
        {
            var hit = Physics2D.Raycast(player.transform.position, Vector2.down, 1.0f, mask);
            if (hit)
            {
                if (hit.collider.TryGetComponent(out AudioTriggerMove audioTrigger))
                {
                    audioMove.clip = audioTrigger.audioClip;
                    audioMove.volume = audioTrigger.volume;
                    if (!audioMove.isPlaying)
                        audioMove.Play();
                }
            }
        }
        else
            audioMove.Stop();

        audioAttack.clip = Input.GetButton("Horizontal") ? attackClipMove : attackClip;

        //Удар
        bool isAttack = Input.GetButtonDown("Fire1") && !player.rapierController.dropRapier
            && !player.isStop;
        if (isAttack)
        {
            player.Flip(player.areaAtack.pointAttack.position.x - player.transform.position.x);
            player.animator.SetTrigger("Attack");
        }

        player.physicsObject.targetVelocity = speed * move;
    }
}
