using UnityEngine;

[CreateAssetMenu(menuName = "Player/Hook")]
public class Hook : PlayerState
{
    public Vector2 frontCheckPosition;
    public float distance;
    public Vector2 upCheckPosition;
    private bool isWallFront = false;
    public LayerMask whatIsGround;

    private GameObject frontCheck;
    private GameObject upCheck;

    public override void Init()
    {
        base.Init();
        frontCheck = new GameObject("FRONTCHEK");
        upCheck = new GameObject("UPCHECK");
        frontCheck.transform.parent = player.transform;
        upCheck.transform.parent = player.transform;
        frontCheck.transform.position = frontCheckPosition;
        upCheck.transform.position = upCheckPosition;
    }

    public override void Run()
    {
        HookPraetermisissent();
        player.animator.SetBool("isWallFront", isWallFront);
    }

    private void HookPraetermisissent()
    {
        var wallHit = Physics2D.Raycast(frontCheck.transform.position, Vector2.right * Game.player.direction, 2.0f, whatIsGround);
        var upHit = Physics2D.Raycast(upCheck.transform.position, Vector2.up, distance, whatIsGround);
        isWallFront = wallHit.collider != null && !Game.player.isGrounded && !upHit;
        if (isWallFront)
        {
            player.Flip(player.transform.position.x - wallHit.point.x);
            player.rb.drag = 15.0f;
        }
        else
        {
            player.rb.drag = 0;
        }
        player.rb.gravityScale = 0;
        player.animator.SetBool("isHook", upHit.collider != null && !Game.player.isGrounded);
    }
}
