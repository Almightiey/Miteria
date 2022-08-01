using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Invertory : MonoBehaviour, ITickUpdate, IGameObject
{
    public DataBase data;

    public List<ItemInvertory> items = new List<ItemInvertory>();
    
    public GameObject gameObjectShow;


    public GameObject GearMainObject;
    public GameObject GearDownObject;

    public int maxCount;
    public int maxCountDown;

    public Camera cam;
    public static EventSystem es; 

    public int currentID;
    public ItemInvertory currentItem;

    public RectTransform movingObject;
    public Vector3 offset;

    public GameObject backGround;
    internal static bool isInvertoryVisible;
    private int idCurrent;

    public MainScipt mainScipt { get; set; }

    public AudioClip audioClipButton;
    private AudioSource audioGearButton;

    public AudioClip audioclipShowGear;
    private AudioSource audioShowGear;

    public void Start()
    {
        es = FindObjectOfType<EventSystem>();
        if (items.Count == 0)
        {
            AddGraphics();
        }


        //for (int i = 0; i < maxCount; i++)//Тест для заполнения 
        //{
        //    AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(1, 99));
        //}
        int numberCell = 0;
        for (int i = 0; i < data.items.Count; i++)
        {
            if(data.items[i].count == 0)
            {
                continue;
            }
            AddItem(numberCell, data.items[i], data.items[i].count);
            numberCell++;
        }
        UpdateGear();

        //Тест для заполнения нижнего
        //for (int i = 0; i < maxCountDown; i++)//Тест для заполнения 
        //{
        //    AddItem(i, data.items[Random.Range(0, data.items.Count)], Random.Range(1, 99));
        //}
        //UpdateGear();
        //___________________________
        mainScipt = new MainScipt();
        mainScipt.invertory = this;
        Game.mainObjects.invertory = this;
        Game.endGame += EndGame;
        UpdateManager.ticks.Add(this);

        audioGearButton = gameObject.AddComponent<AudioSource>();
        audioShowGear = gameObject.AddComponent<AudioSource>();
        audioGearButton.clip = audioClipButton;
        audioShowGear.clip = audioclipShowGear;
    }

    public void OnUpdate()
    {
        if (currentID != -1)
        {
            MoveObject();
        }
        //idCurrent = backGround.activeSelf ? int.Parse(es.currentSelectedGameObject.name) : 0;

        if (Input.GetButtonDown("ShowInvertory"))//Открывает и закрывает по кнопке I
        {
            items[0].itemGameObject.GetComponent<Button>().Select();
            backGround.SetActive(!backGround.activeSelf);
            isInvertoryVisible = backGround.activeSelf;
            if (!audioShowGear.isPlaying && backGround.activeSelf)
                audioShowGear.Play();
        }

        if (backGround.activeSelf)
        {
            UpdateGear();
        }

        if (Input.GetKeyDown(KeyCode.L) && backGround.activeSelf)
        {
            items[idCurrent].count = 0;
            items[idCurrent].itemGameObject.GetComponentInChildren<Text>().text = "";
            items[idCurrent].itemGameObject.GetComponent<Image>().sprite = data.items[0].img;
            if (idCurrent < maxCountDown)
            {
                items[idCurrent].itemGameObjectTwo.GetComponentInChildren<Text>().text = "";
                items[idCurrent].itemGameObjectTwo.GetComponent<Image>().sprite = data.items[0].img;
            }
        }

        KeyContoller(Input.GetKeyDown(KeyCode.Alpha1), 0);
        KeyContoller(Input.GetKeyDown(KeyCode.Alpha2), 1);
        KeyContoller(Input.GetKeyDown(KeyCode.Alpha3), 2);
        KeyContoller(Input.GetKeyDown(KeyCode.Alpha4), 3);


        KeyContoller(Input.GetButton("SelectInvertory") && Input.GetButtonDown("Fire1"), 0);
        KeyContoller(Input.GetButton("SelectInvertory") && Input.GetButtonDown("Jump"), 1);
        KeyContoller(Input.GetButton("SelectInvertory") && Input.GetButtonDown("Back"), 2);
        KeyContoller(Input.GetButton("SelectInvertory") && Input.GetButtonDown("Down"), 3);
    }

    private void KeyContoller(bool keyCode, int index)
    {
        if (keyCode && items[index].count > 0 && items[index].item != null)
        {
            Instantiate(items[index].item, new Vector2(Game.player.transform.position.x,
                Game.player.transform.position.y + 1),
                Quaternion.identity);
            items[index].count--;
            items[index].itemGameObject.GetComponentInChildren<Text>().text = items[index].count.ToString();
            items[index].itemGameObjectTwo.GetComponentInChildren<Text>().text = items[index].count.ToString();
            items[index].itemGameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            items[index].itemGameObjectTwo.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            if (items[index].count == 0)
            {
                items[index].itemGameObject.GetComponentInChildren<Text>().text = "";
                items[index].itemGameObjectTwo.GetComponentInChildren<Text>().text = "";
                items[index].itemGameObject.GetComponent<Image>().sprite = data.items[0].img;
                items[index].itemGameObjectTwo.GetComponent<Image>().sprite = data.items[0].img;
                items[index].itemGameObject.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
                items[index].itemGameObjectTwo.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
            }
        }
    }

    public void SearchForSameItem(Item item, int count)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id == item.id)
            {
                if (items[0].count < 100)
                {
                    items[i].count += count;

                    if (items[i].count > 100)
                    {
                        count = items[i].count - 100;
                        items[i].count = 50;
                    }
                    else
                    {
                        count = 0;
                        i = maxCount;
                    }
                }
            }
        }

        if (count > 0)
        {
            for (int i = 0; i < maxCount; i++)
            {
                if (items[i].id == 0)
                {
                    AddItem(i, item, count);
                    i = maxCount;
                }
            }
        }







        //Тест для нижнего
        for (int i = 0; i < maxCountDown; i++)
        {
            if (items[i].id == item.id)
            {
                if (items[0].count < 100)
                {
                    items[i].count += count;

                    if (items[i].count > 100)
                    {
                        count = items[i].count - 100;
                        items[i].count = 50;
                    }
                    else
                    {
                        count = 0;
                        i = maxCountDown;
                    }
                }
            }
        }

        if (count > 0)
        {
            for (int i = 0; i < maxCountDown; i++)
            {
                if (items[i].id == 0)
                {
                    AddItem(i, item, count);
                    i = maxCountDown;
                }
            }
        }
        //_______________






    }

    public void AddItem(int id, Item item, int count)
    {
        int remainder = 0;
        if (count > 100)
        {
            remainder = count - 99;
            count = 99;
        }
        items[id].id = item.id;
        items[id].count = count;
        items[id].itemGameObject.GetComponent<Image>().sprite = item.img;
        items[id].itemGameObjectTwo.GetComponent<Image>().sprite = item.img;
        items[id].item = item.item;

        if (count > 1 && item.id != 0)
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = count.ToString();
            items[id].itemGameObjectTwo.GetComponentInChildren<Text>().text = count.ToString();
        }
        else
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
            items[id].itemGameObjectTwo.GetComponentInChildren<Text>().text = "";
        }

        if (remainder > 0)
        {
            AddItem(id + 1, item, remainder);
        }
    }


    public void AddGearItem(int id, ItemInvertory gerItem, int count)
    {
        count = count > 100 ? 99 : count;

        items[id].id = gerItem.id;
        items[id].count = count;
        items[id].itemGameObject.GetComponent<Image>().sprite = data.items[gerItem.id].img;
        items[id].itemGameObjectTwo.GetComponent<Image>().sprite = data.items[gerItem.id].img;
        items[id].itemGameObjectTwo.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
        items[id].item = data.items[gerItem.id].item;

        if (gerItem.count > 1 && gerItem.id != 0)
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = items[id].count.ToString();
            items[id].itemGameObjectTwo.GetComponentInChildren<Text>().text = items[id].count.ToString();
        }
        else
        {
            items[id].itemGameObject.GetComponentInChildren<Text>().text = "";
            items[id].itemGameObjectTwo.GetComponentInChildren<Text>().text = "";
        }
        //items[id].itemGameObjectTwo = items[id].itemGameObject;
    }


    public void AddGraphics()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newItem = Instantiate(gameObjectShow, GearMainObject.transform);

            newItem.name = i.ToString();//Превращает содержимое в текстовую часть 

            ItemInvertory ig = new ItemInvertory();
            ig.itemGameObject = newItem;

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);//При взамодествие с предметом что бы масштаб был такой же


            Button tempButton = newItem.GetComponent<Button>();//Каждый пункт -- кнопка

            tempButton.onClick.AddListener(delegate { SelectObject(); });

            if (i < maxCountDown)
            {
                newItem = Instantiate(gameObjectShow, GearDownObject.transform);
                newItem.name = i.ToString();//Превращает содержимое в текстовую часть 
                ig.itemGameObjectTwo = newItem;


                rt = newItem.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(0, 0, 0);
                rt.localScale = new Vector3(1, 1, 1);
                newItem.GetComponentInChildren<RectTransform>().localScale = new Vector3(1, 1, 1);//При взамодествие с предметом что бы масштаб был такой же

                Button newTempButton = newItem.GetComponent<Button>();//Каждый пункт -- кнопка
                
            }
            else
            {
                ig.itemGameObjectTwo = newItem;
            }
            items.Add(ig);
        }
    }

    public void UpdateGear()
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (items[i].id != 0 && items[i].count > 0)
            {
                if (items[i].count > 1)
                {
                    items[i].itemGameObject.GetComponentInChildren<Text>().text = items[i].count.ToString();
                    items[i].itemGameObjectTwo.GetComponentInChildren<Text>().text = items[i].count.ToString();
                }

                items[i].itemGameObject.GetComponent<Image>().sprite = data.items[items[i].id].img;
                items[i].itemGameObjectTwo.GetComponent<Image>().sprite = data.items[items[i].id].img;

                items[i].itemGameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                items[i].itemGameObjectTwo.GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            }
            else
            {
                items[i].itemGameObject.GetComponentInChildren<Text>().text = "";
                items[i].itemGameObjectTwo.GetComponentInChildren<Text>().text = "";
                items[i].itemGameObject.GetComponent<Image>().sprite = data.items[0].img;
                items[i].itemGameObjectTwo.GetComponent<Image>().sprite = data.items[0].img;
                items[i].itemGameObject.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
                items[i].itemGameObjectTwo.GetComponent<Image>().color = new Vector4(0, 0, 0, 0);
            }
        }

        //__________________
        //for (int i = 0; i < maxCountDown; i++)
        //{
        //    if (items[i].id != 0 && items[i].count > 1)
        //    {
        //        items[i].itemGameObject.GetComponentInChildren<Text>().text = items[i].count.ToString();
        //    }
        //    else
        //    {
        //        items[i].itemGameObject.GetComponentInChildren<Text>().text = "";
        //    }

        //    items[i].itemGameObject.GetComponent<Image>().sprite = data.items[items[i].id].img;
        //}
        //__________________
    }

    public void SelectObject()
    {
        if (currentID == -1)
        {
            audioGearButton.Play();
            currentID = int.Parse(es.currentSelectedGameObject.name);
            currentItem = CopyGearItem(items[currentID]);
            movingObject.gameObject.SetActive(true);//Что то перемешаем
            movingObject.GetComponent<Image>().sprite = data.items[currentItem.id].img;

            

            AddItem(currentID, data.items[0], 0);
        }
        else
        {
            ItemInvertory ig = items[int.Parse(es.currentSelectedGameObject.name)];

            if (currentItem.id != ig.id)
            {
                audioGearButton.Play();
                AddGearItem(currentID, ig, ig.count);

                AddGearItem(int.Parse(es.currentSelectedGameObject.name), currentItem, currentItem.count);
            }
            else
            {
                if (ig.count + currentItem.count < 100)
                {
                    ig.count += currentItem.count;
                }
                else
                {
                    AddItem(currentID, data.items[ig.id], ig.count + currentItem.count - 99);

                    ig.count = 99;
                }

                ig.itemGameObject.GetComponentInChildren<Text>().text = ig.count.ToString();
                ig.itemGameObjectTwo.GetComponentInChildren<Text>().text = ig.count.ToString();

            }
            currentID = -1;

            movingObject.gameObject.SetActive(false);

        }
    }

    public void MoveObject()//Работа с камерой
    {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = GearMainObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(pos);

        Vector3 posd = Input.mousePosition + offset;
        posd.z = GearDownObject.GetComponent<RectTransform>().position.z;
        movingObject.position = cam.ScreenToWorldPoint(posd);
    }

   

    public ItemInvertory CopyGearItem(ItemInvertory old)
    {
        ItemInvertory New = new ItemInvertory();
        
        New.id = old.id;
        New.itemGameObject = old.itemGameObject;
        New.itemGameObjectTwo = old.itemGameObjectTwo;
        New.count = old.count;
        New.item = old.item;

        return New;
    }

    public void OnAwake()
    {
        
    }

    private void EndGame()
    {
        Game.endGame -= EndGame;
        UpdateManager.ticks.Remove(this);
    }
}

[System.Serializable]

public class ItemInvertory
{
    public int id;
    public GameObject itemGameObject;
    public GameObject itemGameObjectTwo;
    public GameObject item;
    public Sprite img;
    public int price;

    public int count;//показывает сколько в инвенторе элементов
}