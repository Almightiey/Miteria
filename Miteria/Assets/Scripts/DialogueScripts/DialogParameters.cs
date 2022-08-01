using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[System.Serializable]
public class DialogParameters
{
    public TextPic dialougeText;
    public Text nameText;
   // public Text[] dialogueOptions;

    public Animator OptionsAnimator;

    public Animator boxAnim;

    public Canvas downInverory;

    public Button nextButton;

    public Image talkingPersonImage;

    public Image optionsPanel;

    public GameObject buttonOptions;

    public AudioClip audioDialogue;
}
