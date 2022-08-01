using UnityEngine;

[CreateAssetMenu(menuName = "Player/ShineSword")]
public class ShineSword : PlayerState
{
    public float leadTime;
    public float rapierThrowDealay;
    private float shimeSwordTime;
    private float rapierThrowingTime;
    private bool isShineSword;
    private RapierController rapierController;

    public override void Init()
    {
        base.Init();
        rapierController = player.rapierController;
        shimeSwordTime = 0.0f;
        rapierThrowingTime = 0.0f;
        isShineSword = false;
    }
    

    public override void Run()
    {
        isShineSword = shimeSwordTime > Time.time;
        player.animator.SetBool(nameof(isShineSword), isShineSword);
        if (Input.GetButtonUp("Fire2") && rapierController.dropRapier)
        {
            rapierThrowingTime = Time.time + rapierThrowDealay;
        }
        if (rapierThrowingTime > Time.time && Input.GetButton("Fire1"))
        {
            shimeSwordTime = leadTime + Time.time;
        }
        if (shimeSwordTime > Time.time)
        {
            player.rb.velocity = Vector2.zero;
        }
    }
}
