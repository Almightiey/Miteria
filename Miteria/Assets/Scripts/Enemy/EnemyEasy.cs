using UnityEngine;

public class EnemyEasy : Enemy, ITickUpdate
{
    protected EnemyEasy friends;


    [Header("Mage")]
    public OrbScripts attackEffets;
    public Transform[] atackSpawns;
    public float speedMagic;
    private OrbScripts[] orbs;


    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Init();
        Past();
        Game.endGame += DeathEnemy;
        deathEnemy += DeathEnemy;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnUpdate()
    {
        if (isDeath)
        {
            return;
        }
        if (!currentState.isFinished && currentState.Person != null)
        {
            currentState.Run();
        }
        else
        {
            if (isMoving && !isDialog && !Game.player.isDeath)
                SetState(moveState);
            else
                Past();
        }
        var playerHit = Physics2D.OverlapCircle(Eyes.position, distance, mask[0]);
        isDanger = nextDelay > Time.time;
        isAlert = playerHit && Game.player.popularity > 25;

        if (playerHit || isDanger)
        {
            if ((Game.player.popularity >= Random.Range(0, aggressionController) && Random.value >= chanse) || isDanger)
            {
                Flip(Game.player.transform.position.x - transform.position.x);
                if (Game.player.popularity >= aggressionController || isDanger)
                {
                    isMoving = true;
                    HelpFriend();
                }
            }
        }
        if (!playerHit && !isDanger && (friends == null || !friends.isDanger))
        {
            isMoving = false;
            startPosition = transform.position;
            silenceEnemy?.Invoke();
        }
    }


    protected void Past()
    {
        if (Time.time > nextSpawn)
        {
            motion = Random.Range(0, 100);
            nextSpawn += Random.Range(minDelay, maxDelay);
        }
        if (motion > 70 && !isAlert && !isDialog)
            SetState(walkState);
        else
            SetState(pastState);
    }

    private void HelpFriend()
    {
        if (Physics2D.OverlapCircle(Eyes.position, distance, mask[1]).TryGetComponent(out friends))
        {
            if (friends.isDialogStoped)
            {
                friends.isDialog = isDialog;
            }
            else
            {
                isDialog = false;
            }
            friends.nextDelay = nextDelay;
        }
        else
            friends = null;
    }

    protected void SetState(State state)
    {
        currentState = Instantiate(state);
        currentState.Init(this);
    }


    public void AtackSpawn()
    {
        orbs = new OrbScripts[atackSpawns.Length];
        for (int i = 0; i < atackSpawns.Length; i++)
        {
            orbs[i] = Instantiate(attackEffets, atackSpawns[i].position, Quaternion.identity, atackSpawns[i]);
        }
    }


    public void AtackShot()
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i] == null)
                continue;
            orbs[i].transform.parent = null;
            Vector3 difference = transform.position - orbs[i].transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            //orbs[i].transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);
            orbs[i].speed = speedMagic;
            orbs[i].isShoot = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out AttackEnemy _))
        {
            return;
        }
        if (collision.TryGetComponent(out RespawnArea _))
        {
            Death();
        }

        if (collision.TryGetComponent(out Attack attack) && !isDeath)
        {
            GetDamaged(attack);
        }
    }


    public void OnDelayAtackMove()
    {
        if (maxDelayMove > 0)
        {
            nextDelayMove = Time.time + Random.Range(minDelayMove, maxDelayMove);
        }
    }

    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
    }

    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
    }

    private void DeathEnemy()
    {
        deathEnemy -= DeathEnemy;
        Game.endGame -= DeathEnemy;
        UpdateManager.ticks.Remove(this);
        Game.mainObjects.dialogueManager.dialogPlay -= DialogueStart;
        Game.mainObjects.dialogueManager.dialogEnd -= DialogueStop;
        if (effectDeath != null && health <= 0)
        {
            Instantiate(effectDeath, transform.position, transform.rotation);
        }
    }
}