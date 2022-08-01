using UnityEngine;

public class RuneTest : MonoBehaviour, IRune
{
    public float time;
    public int delay;
    public float eventRuneTime;
    private float next;
    public float timeLifeEffect { get; set; }
    public RuneParameters runeParameters;

    public float damage;
    public event IRune.RuneActive runeActive;
    public EffectRune effectRune;



    public void HitEnemy(Enemy enemy)
    {
        next = Time.time + eventRuneTime;
        effectRune = new EffectRune();
        effectRune.next = next;
        effectRune.damage = damage;
        effectRune.delay = delay;
        effectRune.HitEffect(enemy);
    }


    private void Start()
    {
        timeLifeEffect = time;
        if (Game.runeManager.activeRune != null)
        {
            Game.runeManager.isNewActiveRune = true;
            Game.runeManager.SwitchRune += SwitchRune;
        }
        else
        {
            Init();
        }
    }


    public void Init()
    {
        Game.runeManager.activeRune = this;
        Game.player.atackPlayer.hitOfTheEnemy += HitEnemy;
        Game.player.rapierController.DropRaier += DropRaier;
        Game.player.rapierController.ReturnedRaier += Returned;
        runeActive += Game.ActiveRune;
        runeParameters.image = Game.runeManager.runeParameters.image;
        runeActive?.Invoke();
        Game.runeManager.EndEffectRune += EndEffectRune;
    }

    private void SwitchRune()
    {
        Game.runeManager.SwitchRune -= SwitchRune;
        Init();
    }

    private void DropRaier()
    {
        Game.player.atackPlayer.hitOfTheEnemy -= HitEnemy;
        Game.player.rapierController.airController.rapierAtackPlayer.hitOfTheEnemy += HitEnemy;
    }

    private void Returned()
    {
        Game.player.rapierController.airController.rapierAtackPlayer.hitOfTheEnemy -= HitEnemy;
        Game.player.atackPlayer.hitOfTheEnemy += HitEnemy;
    }

    private void EndEffectRune()
    {
        if (Game.player.rapierController.dropRapier)
            Game.player.rapierController.airController.rapierAtackPlayer.hitOfTheEnemy -= HitEnemy;
        else
            Game.player.atackPlayer.hitOfTheEnemy -= HitEnemy;
        runeActive -= Game.ActiveRune;
        Game.runeManager.EndEffectRune -= EndEffectRune;
        Game.runeManager.activeRune = null;
        Destroy(gameObject);
    }

    public void RuneEffect()
    {
        Game.runeManager.ChangeImage(runeParameters.sprite);
    }
}
