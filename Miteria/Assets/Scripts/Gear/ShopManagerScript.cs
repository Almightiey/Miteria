using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public DataBase dataBase;
    public Animator canAnim;
    public GameObject ShopMainObject;
    public AudioSource clickAudio;
    private Invertory invertory;

    public GameObject gameObjectShow;
    public List<ButtonInfo> cellItems;

    public void Start()
    {
        Game.mainObjects.shopManager = this;
    }


    public void Buy()
    {
        invertory = Game.mainObjects.invertory;
        clickAudio.Play();
        if (Game.coins.Golds >= cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item.price && cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item.count > 0)
        {
            cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item.count--;
            Game.SpendCoins(this, cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item.price);
            cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].QuantityTxt.text = cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item.count.ToString();
            for (int i = 0; i < invertory.maxCount; i++)
            {
                if (invertory.items[i].id == 0)
                {
                    invertory.AddGearItem(i, cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item, 1);
                    break;
                }
                if (invertory.items[i].id == cellItems[int.Parse(Invertory.es.currentSelectedGameObject.name)].item.id && invertory.items[i].count < 99)
                {
                    invertory.items[i].count++;
                    invertory.items[i].itemGameObject.GetComponentInChildren<Text>().text = invertory.items[i].count.ToString();
                    invertory.items[i].itemGameObjectTwo.GetComponentInChildren<Text>().text = invertory.items[i].count.ToString();
                    break;
                }
            }

        }
    }


    public void AddGraphics(List<ItemInvertory> items)
    {
        if (ShopMainObject.transform.childCount > 0)
        {
            foreach (Transform child in ShopMainObject.transform)
            {
                child.gameObject.AddComponent<DeathTime>().Delete();
            }
            cellItems.Clear();
        }
        for (int i = 0; i < items.Count; i++)
        {
            GameObject newItem = Instantiate(gameObjectShow, ShopMainObject.transform);

            newItem.name = i.ToString();//Превращает содержимое в текстовую часть 

            ItemInvertory ig = new ItemInvertory();
            ig.itemGameObject = newItem;

            Button tempButton = newItem.GetComponent<Button>();//Каждый пункт -- кнопка
            newItem.GetComponent<Image>().sprite = dataBase.items[items[i].id].img;
            tempButton.onClick.AddListener(delegate { Buy(); });

            cellItems.Add(newItem.GetComponent<ButtonInfo>());
            cellItems[i].item = items[i];
            cellItems[i].ID = i;

            cellItems[i].PirceTxt.text = cellItems[i].item.price.ToString();
            cellItems[i].QuantityTxt.text = cellItems[i].item.count.ToString();
        }

    }
}

