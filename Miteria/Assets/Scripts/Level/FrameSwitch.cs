using UnityEngine;

public class FrameSwitch : MonoBehaviour
{
    public GameObject activeFrame;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitPlayer>())
            activeFrame.SetActive(true);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<HitPlayer>())
            activeFrame.SetActive(false);
    }
}
