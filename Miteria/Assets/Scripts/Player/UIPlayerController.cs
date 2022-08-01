using UnityEngine;
using UnityEngine.UI;

public class UIPlayerController : MonoBehaviour, IGameObject
{
    public Slider[] slider;
    public Slider sliderHealth;
    public Animator panelAnimation;
    public Button selectButtonDeath;
    public Animator healthPanel;
    public FastGlitch fastGlitch;
    public AudioSource audioDeath;

    public MainScipt mainScipt { get; set; }

    void Start()
    {
        Game.mainObjects.UIplayerController = this;
        slider[0].value = Game.player.popularity;
        slider[1].value = Game.player.popularity;
        panelAnimation.SetBool("PlayerDeath", false);
        mainScipt = new MainScipt();
        mainScipt.UIplayerController = this;
        Game.player.hitDamage.GetDamage += HitDamage;
        Game.player.hitDamage.nextGetDamage += NextHitDamage;
        Game.player.hitDamage.areaDamage += AreaDamage;
        Game.player.hitDamage.nextAreaDamage += NextAreaDamge;
        Game.endGame += Death;
        sliderHealth.value = Game.player.health;
    }

    private void HitDamage(float newHealthValue)
    {
        sliderHealth.value = newHealthValue;
        healthPanel.Play("TakeDamage");
        //if (health <= 0)
        //{
        //    StartCoroutine(Death());
        //}
    }

    private void NextHitDamage(float newHealthValue)
    {
        sliderHealth.value = newHealthValue;
        if (newHealthValue > 50)
        {
            healthPanel.Play("Deffault");
        }
    }

    public void ChangePopularity(float newValue)
    {
        Game.player.popularity += newValue;
        if (Game.player.popularity > 100)
            Game.player.popularity = 100;
        if (Game.player.popularity < 0)
            Game.player.popularity = 0;
        slider[0].value = Game.player.popularity;
        slider[1].value = Game.player.popularity;
    }

    private void AreaDamage()
    {
        panelAnimation.SetBool("isPlayerAreaAtack", true);
    }

    private void NextAreaDamge()
    {
        panelAnimation.SetBool("isPlayerAreaAtack", false);
    }

    private void Death()
    {
        panelAnimation.SetBool("PlayerDeath", true);
        selectButtonDeath.Select();
        Game.endGame -= Death;
        Game.player.hitDamage.GetDamage -= HitDamage;
        Game.player.hitDamage.nextGetDamage -= NextHitDamage;
        Game.player.hitDamage.areaDamage -= AreaDamage;
        Game.player.hitDamage.nextAreaDamage -= NextAreaDamge;
    }

    public void OnAwake()
    {
        
    }
}
