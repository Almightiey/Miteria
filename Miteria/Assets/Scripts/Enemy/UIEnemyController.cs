using UnityEngine;
using UnityEngine.UI;

public class UIEnemyController : MonoBehaviour
{
    private Enemy enemy;
    public Slider sliderHealth;

    void Start()
    {
        sliderHealth.gameObject.SetActive(false);
        enemy = GetComponent<Enemy>();
        enemy.killingAPersonByAPlayer += DeathEnemy;
        enemy.damageEnemy += EnemyDamage;
        enemy.silenceEnemy += SilenceEnemy;

        sliderHealth.maxValue = enemy.health;
        sliderHealth.value = enemy.health;
    }

    private void DeathEnemy(float newValue)
    {
        Game.mainObjects.UIplayerController.ChangePopularity(newValue);
    }


    private void EnemyDamage(float newHealhtValue)
    {
        //Coroutines.StartRoutine(Game.shakeCamera.ProcessShake());
        Game.shakeCamera.Shake(1);
        sliderHealth.gameObject.SetActive(true);
        sliderHealth.value = newHealhtValue;

        if (newHealhtValue <= 0)
        {
            sliderHealth.gameObject.SetActive(false);
        }
    }


    private void SilenceEnemy()
    {
        sliderHealth.gameObject.SetActive(false);
    }
}
