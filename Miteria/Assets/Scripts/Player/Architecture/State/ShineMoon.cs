using UnityEngine;

[CreateAssetMenu(menuName = "Player/ShineMoon")]
public class ShineMoon : PlayerState
{
    private RapierController rapierController;
    public float speed;
    public float offset;
    private PlayerActor playerActor;
    private float rotZ;
    private AttackPlayer atackPlayerOfRapier;
    private bool isShineMoon;


    public override void Init()
    {
        base.Init();
        rapierController = player.rapierController;
        playerActor = Game.mainObjects.playerActor;
        isShineMoon = false;
    }

    [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
    public override void Run()
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
    {
        if (rapierController.dropRapier && Input.GetButtonDown("Fire1") && player.isGrounded)
        {
            var isBack = player.direction > 0 ? rapierController.rapier.transform.position.x < player.transform.position.x :
                rapierController.rapier.transform.position.x > player.transform.position.x;
            if (isBack)
            {
                atackPlayerOfRapier = rapierController.airController.rapierAtackPlayer;
                atackPlayerOfRapier.attack = 20.0f;
                atackPlayerOfRapier.recoil = 1.5f;
                rapierController.airController.isStop = true;
                isShineMoon = true;
            }
        }
        if (isShineMoon)
        {
            Vector3 difference = rapierController.rapier.transform.position - player.transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rapierController.rapier.transform.RotateAround(player.transform.position, new Vector3(0.0f, 0.0f, -player.direction), speed * Time.deltaTime);
            rapierController.rapier.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
            player.rb.velocity = Vector2.zero;
        }
        if (isShineMoon && rapierController.dropRapier && rapierController.airController.hitInfo.collider != null)
        {
            playerActor.effectsPlayer.stakes.transform.position = rapierController.rapier.transform.position;
            playerActor.effectsPlayer.stakes.Play();
            rapierController.airController.isStop = false;
            rapierController.dropRapier = false;
            rapierController.playerRapier.gameObject.SetActive(true);
            Destroy(rapierController.rapier);
            isShineMoon = false;
        }

        player.animator.SetBool(nameof(isShineMoon), isShineMoon);
    }
}
