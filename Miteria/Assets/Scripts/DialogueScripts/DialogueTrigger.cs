using UnityEngine;

[CreateAssetMenu(menuName = "Dialog/DialogueTrigger")]
public class DialogueTrigger : ScriptableObject
{
    public Dialogue[] dialogue;
    public DialogueTrigger[] dialogueTrigger;
    public string optionsNameButton;
    private DialogueManager dm;

    private void Start()
    {
        dm = Game.mainObjects.dialogueManager;
    }

}
