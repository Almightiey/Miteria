using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public Image menu;
    public static bool isPause;
    public Button firstSelect;
    public AudioSource clickAudio;
    public GameObject controlImage;

    private void Start()
    {
        Time.timeScale = 1;
        menu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            controlImage.SetActive(false);
            Time.timeScale = 0;
            menu.gameObject.SetActive(true);
            firstSelect.Select();
        }
    }

    public void Control()
    {
        controlImage.SetActive(true);
        clickAudio.Play();
    }


    public void Proceed()
    {
        Time.timeScale = 1;
        clickAudio.Play();
        menu.gameObject.SetActive(false);
    }
}
