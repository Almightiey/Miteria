using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public Sprite talkingPersonImage;
    [TextArea(3, 10)]
    public string[] sentences;
    // public string[] answers;
}
