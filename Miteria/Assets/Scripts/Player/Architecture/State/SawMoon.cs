using UnityEngine;

[CreateAssetMenu(menuName = "Player/SawMoon")]
public class SawMoon : PlayerState
{
    public float speed;
    public float leadTime;
    public float shotDelay;
    private RapierController rapierController;
    private float sawMoonTime;
    private float nextDelay;
    private AttackPlayer atackPlayerOfRapier;
    private bool isSawMoon;


    public override void Init()
    {
        base.Init();
        rapierController = player.rapierController;
        isSawMoon = false;
        sawMoonTime = 0.0f;
        nextDelay = 0.0f;
    }

    public override void Run()
    {
        if (rapierController.dropRapier && Input.GetButtonDown("Fire1") && nextDelay < Time.time)
        {
            var isFront = player.direction > 0 ? rapierController.rapier.transform.position.x > player.transform.position.x :
                rapierController.rapier.transform.position.x < player.transform.position.x;
            if (isFront)
            {
                sawMoonTime = Time.time + leadTime;
                atackPlayerOfRapier = rapierController.airController.rapierAtackPlayer;
                atackPlayerOfRapier.attack = 10.0f;
                atackPlayerOfRapier.recoil = 0.1f;
                rapierController.airController.isStop = true;
                isSawMoon = true;
            }
        }
        if (sawMoonTime > Time.time && rapierController.rapier != null)
        {
            player.rb.velocity = Vector2.zero;
            rapierController.rapier.transform.RotateAround(rapierController.rapier.transform.position, new Vector3(0.0f, 0.0f, -player.direction), speed);
        }
        else if (isSawMoon)
        {
            rapierController.airController.isStop = false;
            isSawMoon = false;
            nextDelay = Time.time + shotDelay;
            if (rapierController.dropRapier)
                rapierController.airController.CameBack(rapierController.rapier);
        }

        player.animator.SetBool(nameof(isSawMoon), isSawMoon);
    }
}
