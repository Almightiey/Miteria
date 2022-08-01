using System.Collections.Generic;
using UnityEngine;

public class TutorialDialog : MonoBehaviour
{
    internal DialogueManager dm;
    public List<DialogueTrigger> dt;
    [Tooltip("Сделайте активным если хотите, чтобы игрок замирал во время диалога")]
    public bool isPausePlayer;

    private void Start()
    {
        dm = Game.mainObjects.dialogueManager;
    }



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<HitPlayer>())
        {
            dm.pauseDuringDialogue = isPausePlayer;
            dm.dialoguesTrigger = dt;
            dm.StartDialogue(dt[0].dialogue);
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<HitPlayer>())
        {
            dm.EndDialogue();
            Destroy(gameObject);
        }
    }
}
