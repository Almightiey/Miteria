using UnityEngine;

public class EnemyMiddle : Enemy, ITickUpdate
{
    public State secondPhaseMove;
    protected EnemyEasy friends;



    [Header("Distant Atack")]
    public DistantionAtack[] orbs;
    public Transform[] atackSpawns;
    public float speedPosion;
    protected DistantionAtack[] currentOrbs;
    public bool SpawnTargetRandom;
    public int randomIndexOrbs;

    private void Start()
    {
        Init();
        Past();
        Game.endGame += GameOver;
        deathEnemy += GameOver;
    }

    public void OnUpdate()
    {
        if (isDeath)
        {
            return;
        }
        if (!currentState.isFinished)
        {
            currentState.Run();
        }
        else
        {
            if (isMoving && !isDialog && !Game.player.isDeath)
            {
                if (health > 150)
                SetState(moveState);
                else
                    SetState(secondPhaseMove);
            }
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
        currentState.MiddlePerson = this;
        currentState.Person = this;
        state.Init(this);
    }


    public void AtackSpawn()
    {
        currentOrbs = new DistantionAtack[atackSpawns.Length];
        for (int i = 0; i < atackSpawns.Length; i++)
        {
            if (!atackSpawns[i].gameObject.activeSelf)
            {
                currentOrbs[i] = null;
                return;
            }

            currentOrbs[i] = Instantiate(orbs[randomIndexOrbs], atackSpawns[i].transform);
            if (SpawnTargetRandom)
                currentOrbs[i].isToRandom = true;
        }
    }


    public void AtackShot()
    {
        for (int i = 0; i < currentOrbs.Length; i++)
        {
            if (currentOrbs[i] == null)
                continue;
            currentOrbs[i].from = atackSpawns[i];
            currentOrbs[i].isShoot = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out AttackEnemy _))
        {
            return;
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
        if (isDeath)
            Destroy(animation);
    }

    private void GameOver()
    {
        deathEnemy -= GameOver;
        Game.endGame -= GameOver;
        UpdateManager.ticks.Remove(this);
        Game.mainObjects.dialogueManager.dialogPlay -= DialogueStart;
        Game.mainObjects.dialogueManager.dialogEnd -= DialogueStop;
        if (effectDeath != null && health <= 0)
        {
            Instantiate(effectDeath, transform.position, transform.rotation);
        }
    }
}
