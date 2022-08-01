using UnityEngine;

public class RuneSpeed : MonoBehaviour, IRune
{
    public float time;
    public float speed;
    private float saveSpeed;
    public RuneParameters runeParameters;
    public float timeLifeEffect { get; set; }

    public event IRune.RuneActive runeActive;

    public void RuneEffect()
    {
        Game.player.speed = speed;
        Game.player.animator.speed = speed / saveSpeed;
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
        saveSpeed = Game.player.speed;
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
        Game.player.animator.speed = 1.0f;
        Game.player.speed = saveSpeed;
        runeActive -= Game.ActiveRune;
        Game.runeManager.EndEffectRune -= EndEffectRune;
        Game.runeManager.activeRune = null;
        Destroy(gameObject);
    }
}
