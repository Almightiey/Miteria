using UnityEngine;

public class Player
{
    public HitPlayer hitDamage;
    public Rigidbody2D rb;
    public PhysicsObject physicsObject;
    public Animator animator;
    public Transform transform;
    public AreaAtack areaAtack;
    public AttackPlayer atackPlayer;
    public RapierController rapierController;
    public bool isGrounded;
    public bool isStop;
    public delegate void PlayerEvent();
    public bool isInvulnerable;

    public event PlayerEvent FlipIsGround;

    public float speed { get; set; }
    public float health { get; set; }
    public float popularity { get; set; }
    public bool isDeath { get; set; }
    internal float scaleX { get; private set; }

    internal int direction;
    internal string name;

    public float Time;

    public Player() => isDeath = false;

    public void ScaleValue()
    {
        scaleX = transform.localScale.x;
    }

    public void Flip(float value)
    {
        var oldScaleX = transform.localScale.x;
        direction = value != 0 ? (int)(value / Mathf.Abs(value)) : (int)(transform.localScale.x / Mathf.Abs(transform.localScale.x));
        transform.localScale = new Vector3(scaleX * direction, transform.localScale.y, transform.localScale.z);
        if (isGrounded && oldScaleX != transform.localScale.x)
            FlipIsGround?.Invoke();
    }


    public void OnStop()
    {
        physicsObject.noAirResistance = false;
        physicsObject.velocity = new Vector2(0, rb.velocity.y);
        physicsObject.gravityModifier = 1;
    }
}
