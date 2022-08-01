using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour, ITickUpdate
{
    public Animator startAnim;
    public DialogueManager dm;
    public LayerMask maskPlayer;
    public List<DialogueTrigger> dt;
    protected bool isPlayerTrigger;


    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
    }

    private void Start()
    {
        isPlayerTrigger = false;
        dm = Game.mainObjects.dialogueManager;
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M) && isPlayerTrigger)
        {
            dm.pauseDuringDialogue = false;
            dm.dialoguesTrigger = dt;
            dm.StartDialogue(dt[0].dialogue);
        }
        if (Game.player.isStop)
        {
            startAnim.SetBool("StartDialogueOpen", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<HitPlayer>())
        {
            isPlayerTrigger = true;
            startAnim.SetBool("StartDialogueOpen", true);
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<HitPlayer>())
        {
            startAnim.SetBool("StartDialogueOpen", false);
            dm.EndDialogue();
            isPlayerTrigger = false;
        }
    }

    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
    }
}

