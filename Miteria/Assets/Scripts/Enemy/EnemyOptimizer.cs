using UnityEngine;

public class EnemyOptimizer : MonoBehaviour
{
    private EnemyEasy enemy;
    private Animator anim;

    private void Start()
    {
        enemy = GetComponent<EnemyEasy>();
        anim = GetComponent<Animator>();
    }



    //private void OnBecameVisible()
    //{
    //    enemy.enabled = true;
    //    anim.enabled = true;
    //}

    //private void OnBecameInvisible()
    //{
    //    if (enemy.isDeath)
    //    {
    //        Destroy(gameObject);
    //    }
    //    enemy.enabled = false;
    //    anim.enabled = false;
    //}
}
