using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    public Dictionary<int ,IQuest> quest;
    public AudioSource questStart;
    public QuestManager(AudioClip clip)
    {
        quest = new Dictionary<int, IQuest>();
        questStart = Game.gameManagerObject.AddComponent<AudioSource>();
        questStart.clip = clip;
    }
}
