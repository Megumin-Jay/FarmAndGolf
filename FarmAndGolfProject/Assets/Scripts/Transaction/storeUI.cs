using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storeUI : MonoBehaviour
{
    public int itemNumber = 0;//物品数量
    public Player player;//人物身上的脚本
    public Item item = null;//物品
    public float coefficient;//倍率
    public Inventory shopInventory;//商店背包
    public Text priceText;//物品总价格文本
    public Text numberText;//物品件数文本
    public Item Coin;//人物的财富值
    // static storeUI storeUIInstance;

    // void Awake()//单例
    // {
    //     if (storeUIInstance != null)
    //         Destroy(this);
    //     storeUIInstance = this;
    // }

    // public static void UpdateItem(string itemName)
    // {

    //     storeUIInstance.item = storeUIInstance.player.AllItems.itemList[storeUIInstance.player.transaction.FindItem("itemName")];
    //     Debug.Log(itemName);
    // }
    // 找不到办法获得ITEM!!!!!!

    private void OnEnable()
    {
        UpdateNumber();
    }

    public void SellOnClick()
    {
        UpdateNumber();
        if (item != null && itemNumber != 0)
        {
            coefficient = 0.3f;
            player.transaction.Sell(item, itemNumber, coefficient, true);

            item = null;
            itemNumber = 0;
            UpdateNumber();
        }
    }

    public void BuyOnClick()
    {
        UpdateNumber();
        if (item != null && itemNumber != 0)
        {
            coefficient = 1.0f;
            player.transaction.Buy(item, itemNumber, coefficient, true);

            item = null;
            itemNumber = 0;
            UpdateNumber();
        }
    }

    //之前一直没添加的对"购买数量"的限制,现在全部补上
    public void UpdateNumber()
    {
        if (item != null)
        {
            //售卖
            if (storeInventoryManager.storeInstance.sellIsOn)
            {
                if (itemNumber <= 0)
                {
                    itemNumber = 0;
                    priceText.text = (item.price * itemNumber).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else if (itemNumber <= item.itemHeld)
                {
                    priceText.text = (item.price * itemNumber).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else if (itemNumber > item.itemHeld)
                {
                    itemNumber = item.itemHeld;
                    priceText.text = (item.price * itemNumber).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else
                    Debug.Log("未曾设想的道路");
            }
            //购买
            else
            {
                if (itemNumber <= 0)
                {
                    itemNumber = 0;
                    priceText.text = (item.price * itemNumber).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else if (itemNumber > 50)
                {
                    itemNumber = 50;
                }
                else if (itemNumber * item.price > Coin.itemHeld)
                {
                    itemNumber--;
                    priceText.text = (item.price * itemNumber).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else if (itemNumber * item.price <= Coin.itemHeld)
                {
                    priceText.text = (item.price * itemNumber).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else
                    Debug.Log("未曾设想的道路");

            }

        }
        else
        {
            priceText.text = "0";
            numberText.text = "0";
        }
    }

    public void NumberUp()
    {
        if (item != null)
        {
            itemNumber++;
            UpdateNumber();
        }
    }

    public void NumberDown()
    {
        if (item != null)
        {
            itemNumber--;
            UpdateNumber();
        }
    }

}
