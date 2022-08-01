using UnityEngine;

public class TimelineTrigger : MonoBehaviour
{
    public GameObject timeline;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitPlayer>())
        {
            timeline.SetActive(true);
        }
    }
}
