using UnityEngine;

public class Quests : MonoBehaviour, IQuest
{
    public GameObject giftObject;
    public GameObject helpGift;

    public DialogueAnimator animatorDeffault;
    public DialogueAnimator animatorNoComplete;
    public DialogueAnimator animatorComplete;
    public DialogueAnimator afterAnimatorComplete;
    internal string keyOption;

    public int id;
    public int count;
    public int maxCount;
    internal int state;


    public int Id 
    {
        get => id;
        set => id = Id;
    }
    public int Count 
    {
        get => count;
        set => count = value;
    }
    public int MaxCount 
    {
        get => maxCount;
        set => maxCount = value; 
    }
    public GameObject gift 
    {
        get => giftObject;
        set => giftObject = value;
    }
    public bool complete { get; set; }

    private void OnEnable()
    {
        OffAnimatorDialogue();
        if (animatorDeffault.dt.Count > 1)
            keyOption = animatorDeffault.dt[1].optionsNameButton;
    }


    private void OffAnimatorDialogue()
    {
        animatorDeffault.gameObject.SetActive(false);
        animatorComplete.gameObject.SetActive(false);
        animatorNoComplete.gameObject.SetActive(false);
        afterAnimatorComplete.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitPlayer _))
        {
            if (Game.mainObjects.questManager.quest.ContainsKey(Id))
            {
                if (Game.mainObjects.questManager.quest[Id].complete)
                {
                    state = 1;
                    OffAnimatorDialogue();
                    animatorComplete.gameObject.SetActive(true);
                    Game.mainObjects.dialogueManager.dialogEnd += TaskComplete;
                }
                else
                {
                    state = 2;
                    OffAnimatorDialogue();
                    animatorNoComplete.gameObject.SetActive(true);
                }
            }
            else if (!complete)
            {
                Interaction();
                state = 0;
                animatorDeffault.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitPlayer _))
            state = 0;
        if (!complete)
            Game.mainObjects.dialogueManager.onClickOption -= AddTask;
    }

    private void Interaction()
    {
        if (Game.mainObjects.dialogueManager.isEnd)
        {
            Game.mainObjects.dialogueManager.onClickOption += AddTask;
        }
    }

    private void AddTask(string optionButtonName)
    {
        if (optionButtonName == keyOption)
        {
            state = 1;
            if (helpGift != null)
                Instantiate(helpGift.gameObject, transform.position, Quaternion.identity);
            Game.mainObjects.questManager.quest.Add(Id, this);
            complete = false;
            Game.mainObjects.questManager.questStart.Play();
        }
        else
            state = 2;
        Game.mainObjects.dialogueManager.onClickOption -= AddTask;
    }

    private void TaskComplete()
    {
        Instantiate(gift.gameObject, transform.position, Quaternion.identity);
        Game.mainObjects.questManager.quest.Remove(Id);
        animatorComplete.dm.dialogEnd -= TaskComplete;
        OffAnimatorDialogue();
        state = 3;
        afterAnimatorComplete.gameObject.SetActive(true);
        Destroy(animatorDeffault.gameObject);
        Destroy(animatorComplete.gameObject);
        Destroy(animatorNoComplete.gameObject);
        enabled = false;
    }
}
