using UnityEngine;

public class ShowCutSceneSwitcher : MonoBehaviour
{
    public GameObject showCutsceneGameObject;

    private void Start()
    {
        Game.mainObjects.dialogueManager.dialogEnd += EndCutscene;
        Game.mainObjects.dialogueManager.dialogPlay += StartCutscene;
        Game.endGame += EndGame;
    }

    private void EndCutscene()
    {
        showCutsceneGameObject.SetActive(false);
    }

    private void StartCutscene()
    {
        showCutsceneGameObject.SetActive(true);
    }

    private void EndGame()
    {
        Game.endGame -= EndGame;
        Game.mainObjects.dialogueManager.dialogEnd -= EndCutscene;
        Game.mainObjects.dialogueManager.dialogPlay -= StartCutscene;
    }
}
