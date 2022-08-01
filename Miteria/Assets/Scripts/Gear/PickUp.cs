using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public  ItemInvertory itemGear; 
    private Invertory inventory;
    public int Golds;

    private int remainder;
    private bool idNoResult;



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HitPlayer>())
        {
            idNoResult = false;
            remainder = 0;
            if (Golds != 0)
            {
                Game.AddCoins(this, Golds);
                Destroy(gameObject);
                return;
            }
            inventory = Game.mainObjects.invertory;
            for (int i = 0; i < inventory.maxCount; i++)
            {
                if (inventory.items[i].id == itemGear.id)
                {
                    if (inventory.items[i].count + itemGear.count > 100)
                    {
                        remainder = inventory.items[i].count + itemGear.count - 99;
                        inventory.items[i].count = 99;
                    }
                    else
                        inventory.items[i].count += itemGear.count;
                    inventory.items[i].itemGameObject.GetComponentInChildren<Text>().text = inventory.items[i].count.ToString();
                    inventory.items[i].itemGameObjectTwo.GetComponentInChildren<Text>().text = inventory.items[i].count.ToString();
                    if (remainder > 0)
                        itemGear.count = remainder;
                    else
                    {
                        idNoResult = true;
                        Destroy(gameObject);
                        break;
                    }
                }
            }

            if (!idNoResult)
            {
                for (int i = 0; i < inventory.maxCount; i++)
                {
                    remainder = 0;
                    if (inventory.items[i].id == 0)
                    {
                        if (itemGear.count < 100)
                        {
                            inventory.AddGearItem(i, itemGear, itemGear.count);
                            Destroy(gameObject);
                            break;
                        }
                        else
                        {
                            inventory.AddGearItem(i, itemGear, 99);
                            remainder = itemGear.count - 99;
                            itemGear.count = remainder;
                        }
                    }
                }
            }
        }

    }

}
