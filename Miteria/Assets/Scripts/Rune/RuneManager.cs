using System.Threading.Tasks;
using UnityEngine;

public class RuneManager
{
    public IRune activeRune;
    public IRune.RuneActive EndEffectRune;
    public IRune.RuneActive SwitchRune;
    public RuneParameters runeParameters;
    public RuneParameters deffaultRuneParameters;
    public bool isNewActiveRune;
    public AudioSource audioActiveRune;

    public RuneManager(RuneParameters parameters)
    {
        runeParameters = parameters;
        deffaultRuneParameters = runeParameters;
        audioActiveRune = Game.gameManagerObject.AddComponent<AudioSource>();
        audioActiveRune.clip = parameters.clip;
    }

    public async void Active()
    {
        audioActiveRune.Play();
        runeParameters.image.enabled = true;
        runeParameters.backgroundImage.enabled = true;
        var maxTimeValue = activeRune.timeLifeEffect;
        while (activeRune.timeLifeEffect > 0 && !isNewActiveRune)
        {
            if (Time.timeScale != 0)
            {
                activeRune.timeLifeEffect--;
            }
            activeRune.RuneEffect();
            runeParameters.image.fillAmount = (activeRune.timeLifeEffect / maxTimeValue);
            await Task.Delay(1000);
        }
        runeParameters.image.enabled = false;
        runeParameters.backgroundImage.enabled = false;
        activeRune = null;
        EndEffectRune?.Invoke();
        ChangeImage(deffaultRuneParameters.sprite);
        if (isNewActiveRune)
        {
            isNewActiveRune = false;
            SwitchRune?.Invoke();
        }
    }

    public void ChangeImage(Sprite sprite)
    {
        runeParameters.image.sprite = sprite;
        runeParameters.backgroundImage.sprite = sprite;
    }
}
