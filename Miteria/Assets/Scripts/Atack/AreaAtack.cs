using UnityEngine;

public class AreaAtack : MonoBehaviour
{
    public Transform pointAttack;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyEasy>())
        {
            var enemy = collision.GetComponent<EnemyEasy>();
            if (!enemy.isDeath)
            {
                pointAttack.position = collision.transform.position;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyEasy>())
        {
            pointAttack.localPosition = startPosition;
        }
    }
}
