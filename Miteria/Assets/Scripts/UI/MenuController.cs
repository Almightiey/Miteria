using UnityEngine;

public class MenuController : MonoBehaviour
{
    public AudioSource audioClickButton;
    public void StartGame()
    {
        audioClickButton.Play();
        //SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void Control(GameObject gameObject)
    {
        audioClickButton.Play();
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ExitGame()
    {
        audioClickButton.Play();
        Application.Quit();
    }
}
