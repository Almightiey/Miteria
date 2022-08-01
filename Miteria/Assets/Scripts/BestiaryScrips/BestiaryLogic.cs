using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryLogic : MonoBehaviour, ITickUpdate
{
    public GameObject BestiaryMainObject;
    public GameObject ShowGameObject;
    public int BeastsCount;

    public List<BestiaryBeast> Beasts = new List<BestiaryBeast>();

    public Camera Camera;
    public BestiaryBeast currentBeast;
    public BBase BestiaryBase;
    public Invertory gear;

    public GameObject BackGround;
    public Animator BestiaryOpen;
    public Text Decription;
    private int idCurrent;
                                                

    public static bool bestiarySwipe;

    

    public void OnUpdate()
    {

        if (BackGround.activeSelf)
        {
            UpdateBestiary();
            if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Joystick1Button6))//????????? ? ????????? ?? ?????? U ??? L2(PS)
            {
                bestiarySwipe = !bestiarySwipe;
                BestiaryOpen.SetBool("BestiaryOpen", bestiarySwipe);
                SelectObject();
            }
            if (bestiarySwipe)
            {
                Decription.text = BestiaryBase.beasts[idCurrent].description;
                idCurrent = BestiaryMainObject.activeSelf && bestiarySwipe ? int.Parse(Invertory.es.currentSelectedGameObject.name) : 0;
            }
        }

    }
    public void UpdateBestiary()
    {
        for (int i = 0; i < BestiaryBase.beasts.Count; i++)
        {
            if (Beasts[i].id != 0 && Beasts[i].Count > 1)
            {
                Beasts[i].BeastGameObject.GetComponentInChildren<Text>().text = Beasts[i].Count.ToString();
                Beasts[i].BeastGameObject.GetComponentInChildren<Text>().text = Beasts[i].Count.ToString();
            }
            else
            {
                Beasts[i].BeastGameObject.GetComponentInChildren<Text>().text = "";
                Beasts[i].BeastGameObject.GetComponentInChildren<Text>().text = "";
            }

             Beasts[i].BeastGameObject.GetComponent<Image>().sprite = BestiaryBase.beasts[i].img;
        }
    }
    public void Start()
    {
        if (Beasts.Count == 0)
        {
            AddGraphics();
        }

        UpdateBestiary();

        Decription.text = BestiaryBase.beasts[0].description;
        Game.endGame += EndGame;
        UpdateManager.ticks.Add(this);
    }
    public void AddGraphics()
    {
        for (int i = 0; i < BestiaryBase.beasts.Count; i++)
        {
            GameObject beast = Instantiate(ShowGameObject, BestiaryMainObject.transform);

            beast.name = i.ToString();

            BestiaryBeast bestiaryBeast = new BestiaryBeast();
            bestiaryBeast.BeastGameObject = beast;
            
            RectTransform rt = beast.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            
            beast.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);
            //beast.GetComponent<Button>().transition =
            Beasts.Add(bestiaryBeast);
        }
    }

    public void SelectObject()
    {
        //yoooooooooooooooooooooooooooooooooooooooo
        if (bestiarySwipe)
        {
            Beasts[0].BeastGameObject.GetComponent<Button>().Select();
        }
        else
            gear.items[0].itemGameObject.GetComponent<Button>().Select();
    }

    private void EndGame()
    {
        Game.endGame -= EndGame;
        UpdateManager.ticks.Remove(this);
    }
}
[System.Serializable]
public class BestiaryBeast
{
    public int id;
    public GameObject BeastGameObject;
    public int Count;
}
