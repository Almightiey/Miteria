using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour, ITickUpdate, ITickFixedUpdate, ITickLateUpdate, IGameObject
{
    private Player player;
    public Rigidbody2D body;
    public LayerMask mask;
    public EffectsPlayer effectsPlayer;
    public HitPlayer hitDamage;
    public List<PlayerState> playerUpdateMethod;
    public List<PlayerState> playerFixedUpdateMethod;
    public List<PlayerState> playerLateUpdateMethod;

    public MainScipt mainScipt { get; set; }

    public void OnAwake()
    {
        player = new Player();
        player.health = hitDamage.health;
        player.name = gameObject.name;
        player.hitDamage = hitDamage;
        player.isInvulnerable = false;

        player.rb = body;
        player.animator = GetComponent<Animator>();
        player.areaAtack = GetComponentInChildren<AreaAtack>();
        player.atackPlayer = GetComponentInChildren<AttackPlayer>();
        player.rapierController = GetComponentInChildren<RapierController>();
        player.transform = transform;
        player.ScaleValue();
        Game.player = player;
        mainScipt = new MainScipt();
        mainScipt.playerActor = this;
        Game.endGame += EndGame;

        player.physicsObject = new PhysicsObject();
        player.physicsObject.Init(player, gameObject, mask);
    }

    void Start()
    {
        hitDamage.playerActor = this;
        hitDamage.player = player;
        hitDamage.GameOver += EndGame;

        effectsPlayer.player = player;
        UpdateManager.ticks.Add(this);
        UpdateManager.fixedTicks.Add(this);
        UpdateManager.lateTicks.Add(this);


        for (int i = 0; i < playerUpdateMethod.Count; i++)
        {
            playerUpdateMethod[i].player = player;
            playerUpdateMethod[i].Init();
        }

        for (int i = 0; i < playerFixedUpdateMethod.Count; i++)
        {
            playerFixedUpdateMethod[i].player = player;
            playerFixedUpdateMethod[i].Init();
        }

        for (int i = 0; i < playerLateUpdateMethod.Count; i++)
        {
            playerLateUpdateMethod[i].player = player;
            playerLateUpdateMethod[i].Init();
        }
    }

    public void OnUpdate()
    {
        player.animator.SetBool(nameof(player.isStop), player.isStop && player.isGrounded);
        if (player.isStop)
        {
            player.OnStop();
            return;
        }
        for (int i = 0; i < playerUpdateMethod.Count; i++)
        {
            playerUpdateMethod[i].Run();
        }
    }


    public void OnFixedUpdate()
    {
        player.animator.SetBool(nameof(player.isStop), player.isStop && player.isGrounded);
        if (player.isStop)
        {
            player.OnStop();
            return;
        }


        for (int i = 0; i < playerFixedUpdateMethod.Count; i++)
        {
            playerFixedUpdateMethod[i].Run();
        }
    }


    public void OnLateUpdate()
    {
        player.animator.SetBool(nameof(player.isStop), player.isStop && player.isGrounded);
        if (player.isStop)
        {
            player.OnStop();
            return;
        }

        for (int i = 0; i < playerLateUpdateMethod.Count; i++)
        {
            playerLateUpdateMethod[i].Run();
        }
    }


    private void EndGame()
    {
        Game.endGame -= EndGame;
        hitDamage.GameOver -= GameOver;
        UpdateManager.ticks.Remove(this);
        UpdateManager.fixedTicks.Remove(this);
        UpdateManager.lateTicks.Remove(this);
    }

    private void GameOver()
    {
        Game.endGame -= EndGame;
        hitDamage.GameOver -= GameOver;
        UpdateManager.ticks.Remove(this);
        UpdateManager.fixedTicks.Remove(this);
        UpdateManager.lateTicks.Remove(this);
    }
}
