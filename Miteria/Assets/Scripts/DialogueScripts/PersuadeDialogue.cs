using UnityEngine;

public class PersuadeDialogue : DialogueAnimator
{
    public int price;
    public int minusPopolarity;
    public string Text;
    internal string keyOption;

    private void Start()
    {
        isPlayerTrigger = false;
        dm = Game.mainObjects.dialogueManager;
        Text = $"Хотите залпатить {price} чтобы снять {minusPopolarity} с вашей популярности";
        if (dt.Count > 1)
            keyOption = dt[1].optionsNameButton;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && isPlayerTrigger)
        {
            dm.pauseDuringDialogue = false;
            dm.dialoguesTrigger = dt;
            dm.StartDialogue(dt[0].dialogue);
            dm.textPersuade = Text;
            Game.mainObjects.dialogueManager.onClickOption += OnClick;
        }
        if (Game.player.isStop)
        {
            startAnim.SetBool("StartDialogueOpen", false);
        }
    }



    private void OnClick(string optionsName)
    {
        if (optionsName == keyOption && Game.coins.Golds >= price)
        {
            Game.mainObjects.UIplayerController.ChangePopularity(-minusPopolarity);
            Game.SpendCoins(this, price);
        }
        Game.mainObjects.dialogueManager.onClickOption -= OnClick;
    }
}
