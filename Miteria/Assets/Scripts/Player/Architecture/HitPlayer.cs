using System.Threading.Tasks;
using UnityEngine;


public class HitPlayer : Body
{
    public Player player;
    public Material material;
    public float armor;
    public bool isNoActionOtherAnimation;
    internal PlayerActor playerActor;
    internal bool isItInvulnerable;
    internal bool isRecoil;

    public delegate void HealthAction(float value);
    public delegate void SomeAction();

    public event HealthAction GetDamage;
    public event HealthAction nextGetDamage;

    public event SomeAction GameOver;
    public event SomeAction areaDamage;
    public event SomeAction nextAreaDamage;

    private bool isTakeDamage;
    private bool endGame;

    public void Start()
    {
        isItInvulnerable = false;
        Game.endGame += EndGame;
        endGame = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RespawnArea respawnArea))
        {
            float offsetDiscarding = respawnArea.transform.position.x - player.transform.position.x > 0 ? respawnArea.recoil : -respawnArea.recoil;
            AreaAtack(respawnArea, offsetDiscarding);
        }

        if (collision.TryGetComponent(out Attack atackEnemy) && !isItInvulnerable)
        {
            //Coroutines.StartRoutine(Game.shakeCamera.ProcessShake());
            Game.shakeCamera.Shake(50);
            if (atackEnemy.attackEffect == null)
                Instantiate(Game.mainObjects.playerActor.effectsPlayer.hit, collision.bounds.center, Quaternion.identity);
            player.health -= atackEnemy.TakeDamage() - (atackEnemy.TakeDamage() / 100 * armor);
            float offsetDiscarding = atackEnemy.strikerPosition.x - player.transform.position.x > 0 ? atackEnemy.recoil : -atackEnemy.recoil;
            Recoil(offsetDiscarding);
            GetDamage?.Invoke(player.health);
            if (player.health <= 0)
            {
                Death();
            }
        }

        if (collision.TryGetComponent(out HealthPlus healthPlus))
        {
            player.health += healthPlus.hpPlus;
            if (player.health > 100)
            {
                player.health = 100;
            }
            nextGetDamage?.Invoke(player.health);
        }
    }

    public void ChangeHealth()
    {
        GetDamage?.Invoke(player.health);
        nextGetDamage?.Invoke(player.health);
        if (player.health <= 0)
        {
            Death();
        }
    }


    private async void AreaAtack(RespawnArea respawnArea, float offsetDiscarding)
    {
        if (!isItInvulnerable)
        {
            player.health -= respawnArea.TakeDamage();
        }
        GetDamage?.Invoke(player.health);
        isItInvulnerable = true;
        player.rapierController.airController.isCameBack = true;

        if (player.health <= 0)
        {
            Death();
        }
        else
        {
            player.animator.SetBool("isDeath", true);
            player.transform.position = new Vector2(player.transform.position.x - offsetDiscarding, player.transform.position.y + offsetDiscarding / 10);
            Game.isPlayerStop = true;
            await Task.Delay(100);
            areaDamage?.Invoke();
            await Task.Delay(1000);
            Game.isPlayerStop = false;
            isItInvulnerable = false;
            if (!endGame)
            {
                player.animator.SetBool("isDeath", false);
                nextAreaDamage?.Invoke();
                nextGetDamage?.Invoke(player.health);
                player.transform.position = Game.respawnPlayerPosition.position;
            }
        }
    }

    private void Death()
    {
        player.isDeath = true;
        GameOver?.Invoke();
    }

    private async void Recoil(float offsetDiscarding)
    {
        var delay = 20.0f;
        isRecoil = true;
        player.animator.SetTrigger(nameof(isTakeDamage));
        isItInvulnerable = true;
        player.transform.position = new Vector2(player.transform.position.x - offsetDiscarding, player.transform.position.y + offsetDiscarding / 10);
        while (delay > 0)
        {
            delay--;
            material.color = material.color.r == 1 ? new Vector4(0, 0, 0, 1) : new Vector4(1, 1, 1, 1);
            if (endGame)
            {
                //player.animator.SetBool(nameof(isTakeDamage), false);
                break;
            }
            await Task.Delay(100);
            //player.animator.SetBool(nameof(isTakeDamage), false);
        }
        //transform.position = new Vector2(transform.position.x + offsetDiscarding, transform.position.y);
        material.color = new Vector4(1, 1, 1, 1);
        isItInvulnerable = false;
        isRecoil = false;
        if (!endGame)
            nextGetDamage?.Invoke(player.health);
    }


    private void EndGame()
    {
        endGame = true;
        Game.endGame -= EndGame;
    }
}
