using UnityEngine;
using UnityEngine.UI;

public class DeathScreenMessage : MonoBehaviour
{
    public string[] message;
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
        text.text = message[Random.Range(0, message.Length - 1)];
        Game.mainObjects.deathScreenMessage = this;
    }
}
