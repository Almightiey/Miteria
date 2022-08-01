using UnityEngine;

public class AnimationQuestPerson : MonoBehaviour, ITickUpdate
{
    private Animator _animation;
    private Quests questPesron;


    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
    }

    private void Start()
    {
        _animation = GetComponent<Animator>();
        questPesron = GetComponent<Quests>();
        Game.endGame += GameOver;
    }

    public void OnUpdate()
    {
        _animation.SetInteger(nameof(questPesron.state), questPesron.state);
    }


    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
    }



    private void GameOver()
    {
        Game.endGame -= GameOver;
        UpdateManager.ticks.Remove(this);
    }
}
