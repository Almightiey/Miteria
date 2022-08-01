using UnityEngine;

public class RuneDefferance : MonoBehaviour, IRune
{
    public float time;
    public float armor;
    public float timeLifeEffect { get; set; }
    public RuneParameters runeParameters;

    public event IRune.RuneActive runeActive;


    public void RuneEffect()
    {
        Game.player.hitDamage.armor = armor;
        Game.runeManager.ChangeImage(runeParameters.sprite);
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
        runeActive += Game.ActiveRune;
        runeParameters.image = Game.runeManager.runeParameters.image;
        runeActive?.Invoke();
        Game.runeManager.EndEffectRune += EndEffectRune;
    }

    private void SwitchRune()
    {
        Init();
        Game.runeManager.SwitchRune -= SwitchRune;
    }


    private void EndEffectRune()
    {
        Game.player.hitDamage.armor = 0;
        runeActive -= Game.ActiveRune;
        Game.runeManager.EndEffectRune -= EndEffectRune;
        Game.runeManager.activeRune = null;
        Destroy(gameObject);
    }
}
