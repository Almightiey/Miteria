using UnityEngine;

public class GeneralEnemy : MonoBehaviour, ITickUpdate
{
    private EnemyEasy enemyGeneral;
    private bool isBreaker;

    public EnemyEasy[] Minions;
    public GameObject breakerObject;
    public GameObject[] Loots;


    private void Start()
    {
        enemyGeneral = GetComponent<EnemyEasy>();
    }

    public void OnUpdate()
    {
        if (enemyGeneral.isDeath)
        {
            for(int i = 0; i < Minions.Length; i++)
            {
                isBreaker = Minions[i].isDeath;
                
                if (!isBreaker)
                {
                    return;
                }
            }
            if (!breakerObject.activeSelf)
                EndFight();
        }
    }


    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
    }

    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
    }


    private void EndDialogs()
    {
        Game.mainObjects.dialogueManager.dialogEnd -= EndDialogs;
        foreach (var loot in Loots)
            Instantiate(loot, new Vector2(transform.position.x + Random.Range(0, 2), transform.position.y - transform.localScale.y), Quaternion.identity);
    }

    private void EndFight()
    {
        breakerObject.SetActive(true);
        Game.mainObjects.dialogueManager.dialogEnd += EndDialogs;
        //breakerObject.gameObject.GetComponents<Rigidbody2D>().
        //for (int i = 0; i < Minions.Length; i++)
        //{
        //    Minions[i].gameObject.layer = 17;
        //}
    }
}
