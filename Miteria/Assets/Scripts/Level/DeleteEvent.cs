using UnityEngine;
using UnityEngine.Playables;

public class DeleteEvent : MonoBehaviour
{
    public PlayableDirector director;
    private void Start()
    {
        Game.mainObjects.dialogueManager.dialogEnd += Delete;
    }


    public void Delete()
    {
        //gameObject.SetActive(false);
        director.enabled = false;
    }
}
