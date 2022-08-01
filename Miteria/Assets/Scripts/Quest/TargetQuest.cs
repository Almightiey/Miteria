using UnityEngine;

public class TargetQuest : MonoBehaviour
{
    public int key;
    public Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitPlayer>() && (enemy == null || enemy.isDeath))
        {
            if (Game.mainObjects.questManager.quest.TryGetValue(key, out IQuest quest))
            {
                if (quest.Count < quest.MaxCount)
                {
                    quest.Count++;
                    if (quest.Count == quest.MaxCount)
                    {
                        Game.mainObjects.questManager.quest[key].complete = true;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
