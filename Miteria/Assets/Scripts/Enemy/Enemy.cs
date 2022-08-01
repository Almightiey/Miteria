using UnityEngine;
using UnityEngine.UI;

public class Enemy : Body
{
    public GameObject damageEffect;
    public float speedEnemy = 4;
    public float distance = 7;
    internal bool isDanger;
    public float canDelay = 10;
    public Transform Eyes;
    public Transform BackEyes;
    internal float nextSpawn;
    protected float motion;

    [Header("States")]
    public float minDelay = 7;
    public float maxDelay = 35;
    public State pastState;
    public State walkState;
    public State moveState;
    [Header("Actual state")]
    public State currentState;
    
    [Tooltip("Создайте три экземпляра слоя, где 1)Слой игрока 2)Слой друга объекта 3)Слой земли")]
    public LayerMask[] mask;

    internal float nextDelay;
    internal bool isDeath;
    internal Rigidbody2D rb;

    internal new Animator animation;
    internal Vector2 startPosition;
    internal bool isMoving;
    public float minDelayMove = 2;
    public float maxDelayMove = 4;
    internal float nextDelayMove;
    internal bool isAlert;
    internal bool isDialog;
    public bool isDialogStoped;

    private Text[] texts;

    public GameObject[] Loots;
    public GameObject effectDeath;
    public float aggressionController;
    public float chanse;

    [Header("Popularity")]
    public float minIncreaseInPlayerPopularity;
    public float maxIncreaseInPlayerPopularity;
    private float increaseInPlayerPopularity;

    public delegate void damageEnemyEvent(float value);
    public delegate void TriggerEnemyEvent();
    public damageEnemyEvent damageEnemy;
    public damageEnemyEvent killingAPersonByAPlayer;
    public TriggerEnemyEvent deathEnemy;
    public TriggerEnemyEvent silenceEnemy;

    internal bool isNoFlip;

    public void Init()
    {
        increaseInPlayerPopularity = Random.Range(minIncreaseInPlayerPopularity, maxIncreaseInPlayerPopularity);
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        motion = Random.Range(0, 70);
        nextSpawn = Random.Range(minDelay, maxDelay);
        isDeath = false;
        nextDelay = 0;
        startPosition = transform.position;
        texts = GetComponentsInChildren<Text>();
        Game.mainObjects.dialogueManager.dialogPlay += DialogueStart;
        Game.mainObjects.dialogueManager.dialogEnd += DialogueStop;
    }

    internal void Flip(float targetX)
    {
        if (isNoFlip)
            return;
        var direction = targetX != 0 ? targetX / Mathf.Abs(targetX) : transform.localScale.x;
        transform.localScale = new Vector3(direction, transform.localScale.y, 1);
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].transform.localScale = new Vector3(direction, transform.localScale.y, 1);
        }
    }

    protected void GetDamaged(Attack attack)
    {
        var xPosAtackPlayer = transform.worldToLocalMatrix.MultiplyPoint3x4(attack.transform.position);
        Instantiate(damageEffect, attack.transform.position, Quaternion.identity);

        float offsetDiscarding = attack.transform.position.x - transform.position.x > 0 ? -attack.recoil : attack.recoil;
        transform.position = new Vector2(transform.position.x + offsetDiscarding, transform.position.y);
        health -= xPosAtackPlayer.x < -0.7f ? attack.TakeDamage() + Random.Range(30, 50) : attack.TakeDamage();
        nextDelay = canDelay + Time.time;
        ChangeHealth();
    }


    protected void Death()
    {
        health -= health;
        increaseInPlayerPopularity = 0;
        ChangeHealth();
    }

    public void ChangeHealth()
    {
        damageEnemy?.Invoke(health);
        if (health <= 0)
        {
            deathEnemy?.Invoke();
            killingAPersonByAPlayer?.Invoke(increaseInPlayerPopularity);
            animation.SetInteger("State", 3);
            isDeath = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.gravityScale = 1;
            rb.freezeRotation = false;
            if (Loots.Length > 0)
            {
                foreach (var loot in Loots)
                    Instantiate(loot, transform.position, Quaternion.identity);
            }
            Game.mainObjects.dialogueManager.dialogPlay -= DialogueStart;
            Game.mainObjects.dialogueManager.dialogEnd -= DialogueStop;
            gameObject.layer = 24;
        }
    }

    public void DialogueStart()
    {
        if (isDialogStoped)
            isDialog = true;
    }

    public void DialogueStop()
    {
        isDialog = false;
    }
}
