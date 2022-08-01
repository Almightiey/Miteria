using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawn : MonoBehaviour
{
    public EnemyInit[] enemy;
    public float minValue;
    public float maxValue = 100;
    private Enemy enemyObject;


    private void OnEnable()
    {
        var sortingGroup = GetComponent<SortingGroup>();
        var enemyCreate = CreateEnemy();
        if (enemyCreate != null)
        {
            enemyObject = Instantiate(enemyCreate, transform.position, Quaternion.identity, transform.parent);
            enemyObject.GetComponent<SpriteRenderer>().sortingOrder = sortingGroup.sortingOrder;
        }
    }

    private Enemy CreateEnemy()
    {
        List<float> chances = new List<float>();
        for (int i = 0; i < enemy.Length; i++)
        {
            chances.Add(enemy[i].curve.Evaluate(Game.player.popularity));
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;


        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if (value < sum)
            {
                if (enemy[i].enemy != null)
                {
                    enemy[i].enemy.chanse = Random.value;
                    enemy[i].enemy.aggressionController = Random.Range(minValue, maxValue);
                    return enemy[i].enemy;
                }
                else
                {
                    return null;
                }
            }
        }

        return enemy[enemy.Length - 1].enemy;
    }


    private void OnDisable()
    {
        if (enemyObject != null)
        {
            enemyObject.deathEnemy?.Invoke();
            Destroy(enemyObject.gameObject);
        }
    }
}
