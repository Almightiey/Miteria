using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour, ITickUpdate
{
    [Tooltip("Please indicate: ID, Count adn Price")]
    public List<ItemInvertory> items = new List<ItemInvertory>();
    public Animator startAnim;
    protected bool isPlayerTrigger;

    private void OnEnable()
    {
        UpdateManager.ticks.Add(this);
    }


    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M) && isPlayerTrigger)
        {
            Game.mainObjects.shopManager.AddGraphics(items);
            Game.mainObjects.shopManager.canAnim.gameObject.SetActive(true);
            Game.mainObjects.shopManager.canAnim.SetBool("isShoping", true);
        }
        if (Game.player.isStop)
        {
            startAnim.SetBool("StartDialogueOpen", false);
        }

        if (Input.GetButtonDown("ShowInvertory") && Game.isInvertoryOpen)//Открывает и закрывает по кнопке I
        {
            Game.mainObjects.shopManager.canAnim.SetBool("isShoping", false);
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
            isPlayerTrigger = false;
            startAnim.SetBool("StartDialogueOpen", false);
        }
    }


    private void OnDisable()
    {
        UpdateManager.ticks.Remove(this);
    }
}
