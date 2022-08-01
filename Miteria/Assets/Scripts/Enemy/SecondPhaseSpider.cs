using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/SecondPhaseSpider")]
public class SecondPhaseSpider : State
{
    public override void Run()
    {
        if (isFinished)
        {
            return;
        }
        foreach(var atackspawn in MiddlePerson.atackSpawns)
        {
            atackspawn.gameObject.SetActive(true);
            atackspawn.localRotation = Quaternion.identity;
        }
        MiddlePerson.randomIndexOrbs = 0;
        MiddlePerson.SpawnTargetRandom = true;
        Person.transform.rotation = Quaternion.Euler(0, 0, 180);
        Person.rb.gravityScale = -1;
        Person.animation.SetInteger("State", 2);

        isFinished = !Person.isMoving || Person.isDeath || Game.player.isDeath;
    }
}