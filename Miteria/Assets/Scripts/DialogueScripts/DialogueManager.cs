using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : IGameObject
{
    private DialogParameters dialogParameters { get; set; }
    public MainScipt mainScipt { get; set; }

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<Sprite> talkingPersonImage;
    public bool isEnd;
    internal bool pauseDuringDialogue;

    public delegate void DialogueTime();
    public event DialogueTime dialogPlay;
    public event DialogueTime dialogEnd;


    private Dictionary<string, string> colorsKeys;
    private Dictionary<string, string> iconKeys;
    public List<DialogueTrigger> dialoguesTrigger;
    public delegate void DialogueEvent(string key);
    public DialogueEvent onClickOption;
    public string textPersuade;

    public AudioSource audioSource;

    public DialogueManager(DialogParameters parameters)
    {
        mainScipt = new MainScipt();
        mainScipt.dialogueManager = this;
        dialogParameters = parameters;
        dialogParameters.nextButton.onClick.AddListener(delegate { DisplayNextSentences(); });
        isEnd = true;
        sentences = new Queue<string>();
        names = new Queue<string>();
        talkingPersonImage = new Queue<Sprite>();
        colorsKeys = new Dictionary<string, string>();
        colorsKeys.Add("(white)", "white");
        colorsKeys.Add("(green)", "green");
        colorsKeys.Add("(blue)", "blue");
        colorsKeys.Add("(red)", "red");
        colorsKeys.Add("(black)", "black");
        colorsKeys.Add("(aqua)", "#00FFFF");
        colorsKeys.Add("(#B40431)", "#B40431");
        colorsKeys.Add("(anlior)", "#045FB4");
        colorsKeys.Add("(abison)", "#A901DB");
        colorsKeys.Add("(shadow)", "#000000");

        iconKeys = new Dictionary<string, string>();
        for (int i = 0; i < dialogParameters.dialougeText.inspectorIconList.Length; i++)
        {
            iconKeys.Add(dialogParameters.dialougeText.inspectorIconList[i].name, dialogParameters.dialougeText.inspectorIconList[i].name);
        }
        audioSource = Game.gameManagerObject.AddComponent<AudioSource>();
        audioSource.clip = parameters.audioDialogue;

    }
    public void StartDialogue(Dialogue[] dialogue)
    {
        textPersuade = "";
        dialogParameters.nextButton.Select();
        isEnd = false;
        if (pauseDuringDialogue)
        {
            dialogPlay?.Invoke();
        }
        dialogParameters.nextButton.gameObject.SetActive(true);
        dialogParameters.downInverory.gameObject.SetActive(false);
        dialogParameters.boxAnim.gameObject.SetActive(true);
        dialogParameters.boxAnim.SetBool("DialogueBoxOpen", true);
        dialogParameters.OptionsAnimator.SetBool("DialogueOptionsOpen", false);
        sentences.Clear();
        foreach (var diolog in dialogue)
        {
            names.Enqueue(diolog.name);
            talkingPersonImage.Enqueue(diolog.talkingPersonImage);
            foreach (string sentence in diolog.sentences)
            {
                sentences.Enqueue(sentence);
            }
        }
        DisplayNextSentences();

    }

    public void DisplayNextSentences()
    {
        if (sentences.Count == 0)
        {
            if (dialoguesTrigger.Count > 1)
            {
                if (textPersuade != "")
                    dialogParameters.optionsPanel.GetComponentInChildren<Text>().text = textPersuade;
                dialogParameters.nextButton.gameObject.SetActive(false);
                dialogParameters.OptionsAnimator.gameObject.SetActive(true);
                dialogParameters.OptionsAnimator.SetBool("DialogueOptionsOpen", true);
                AddOptions();
            }
            else
                EndDialogue();
            return;
        }
        if (names.Count > 0)
        {
            dialogParameters.talkingPersonImage.sprite = talkingPersonImage.Dequeue();
            string name = names.Dequeue();
            var colorIndex = "";
            foreach (var colorId in colorsKeys)
            {
                if (name.Contains(colorId.Key))
                {
                    var namesClean = name.Substring(colorId.Key.Length);
                    colorIndex = colorId.Value;
                    dialogParameters.nameText.text = "<color=" + colorIndex + ">" + namesClean + "</color>";
                }
            }
            if (colorIndex == "")
                dialogParameters.nameText.text = name;
        }
        string sentence = sentences.Dequeue();
        TypeSentence(sentence);

    }

    public void EndDialogue()
    {
        names.Clear();
        isEnd = true;
        dialogEnd?.Invoke();
        dialogParameters.downInverory.gameObject.SetActive(true);
        dialogParameters.boxAnim.SetBool("DialogueBoxOpen", false);
        dialogParameters.OptionsAnimator.SetBool("DialogueOptionsOpen", false);
    }

    private void TypeSentence(string sentence)
    {
        dialogParameters.dialougeText.text = "";
        string[] words = sentence.Split(' ');
        Coroutines.StopAllRoutine();
        Coroutines.StartRoutine(GetColors(words));
    }



    public IEnumerator GetColors(string[] words)
    {
        audioSource.Play();
        foreach (string word in words)
        {
            var colorIndex = "";
            foreach (var colorId in colorsKeys)
            {
                if (word.Contains(colorId.Key))
                {
                    var wordClean = word.Substring(colorId.Key.Length);
                    colorIndex = colorId.Value;
                    foreach (char simbol in wordClean.ToCharArray())
                    {
                        dialogParameters.dialougeText.text += "<color=" + colorIndex + ">" + simbol + "</color>";
                        yield return null;
                    }
                }
            }
            //foreach (var iconId in iconKeys)
            //{
            //    if (word.Contains(iconId.Key))
            //    {
            //        word.Substring(iconId.Key.Length);
            //        colorIndex = " ";
            //        dialogParameters.dialougeText.text += iconId.Value;
            //        yield return null;
            //    }
            //}
            if (colorIndex == "")
            {
                foreach (char simbol in word.ToCharArray())
                {
                    dialogParameters.dialougeText.text += simbol;
                    yield return null;
                }
            }
            dialogParameters.dialougeText.text += " ";
        }
        audioSource.Stop();
    }


    private void AddOptions()
    {
        for (int i = 1; i < dialoguesTrigger.Count; i++)
        {
            names.Clear();
            sentences.Clear();
            GameObject newObjectOptions = Game.CreateObject(dialogParameters.buttonOptions, dialogParameters.optionsPanel.transform);
            newObjectOptions.name = dialoguesTrigger[i].optionsNameButton;

            Button tempButton = newObjectOptions.GetComponent<Button>();
            tempButton.GetComponentInChildren<Text>().text = dialoguesTrigger[i].optionsNameButton;
            var dialogueForButton = dialoguesTrigger[i].dialogue;
            var trigger = dialoguesTrigger[i];

            tempButton.onClick.AddListener(delegate { OnClickOptions(trigger); });
        }
        dialoguesTrigger.RemoveRange(0, dialoguesTrigger.Count);
    }

    private void OnClickOptions(DialogueTrigger trigger)
    {
        Button[] tempButton = dialogParameters.optionsPanel.GetComponentsInChildren<Button>();
        dialoguesTrigger.Add(trigger);
        if (trigger.dialogueTrigger != null)
        {
            foreach (var dt in trigger.dialogueTrigger)
                dialoguesTrigger.Add(dt);
        }
        for (int i = 0; i < tempButton.Length; i++)
            Game.Delete(tempButton[i].gameObject);
        StartDialogue(trigger.dialogue);
        onClickOption?.Invoke(trigger.optionsNameButton);
    }

    public void OnAwake()
    {
        throw new System.NotImplementedException();
    }
}







