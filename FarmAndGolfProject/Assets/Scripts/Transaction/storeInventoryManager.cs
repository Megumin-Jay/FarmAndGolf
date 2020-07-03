using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storeInventoryManager : MonoBehaviour
{
    //单例模式
    public static storeInventoryManager storeInstance;

    public Inventory storeInventory;//商店背包,通过碰撞获取
    public InventoryManager playerInventoryManager;//玩家的库存管理器
    public Player player;//人物身上的脚本

    public Inventory myBag;//存储层的背包
    public GameObject slotGrid;//一堆格子,表层意义上的"背包"
    public GameObject emptySlot;
    public Text itemInformation;//物品描述
    public Text itemName;//物品名称
    public Text itemPrice;//物品价格
    public Text playerWealth;//人物财富
    public Image itemImagePanel;//物品的放大图片
    public List<GameObject> slots = new List<GameObject>();//格子列表
    public Item Coin;//人物的财富值
    public bool sellIsOn;//true为售卖模式, false为购买模式


    public GameObject storePanel;
    public GameObject SellButton;
    public GameObject BuyButton;
    public int itemNumber = 0;//物品数量
    public Item item = null;//物品
    public float coefficient;//倍率
    public Text priceText;//物品总价格文本
    public Text numberText;//物品件数文本




    void Awake()//单例
    {
        if (storeInstance != null)
            Destroy(this);
        storeInstance = this;
    }

    private void OnEnable()
    {
        if (sellIsOn)
        {
            SellButton.SetActive(true);
            BuyButton.SetActive(false);
        }

        if (!sellIsOn)
        {
            SellButton.SetActive(false);
            BuyButton.SetActive(true);
        }

        RefreshItem();
        storeInstance.itemPrice.text = "";
        storeInstance.itemName.text = "";
        storeInstance.itemInformation.text = "";//这个背包的描述框是一直显示的,所以要保证没点击物品的时候描述为"空"
        storeInstance.playerWealth.text = Coin.itemHeld.ToString();
    }

    //给碰撞体获得player脚本
    public void GotPlayer(Player gotPlayer)
    {
        storeInstance.player = gotPlayer;
    }

    //以售卖格式打开商店
    public void OpenSellStore()
    {
        sellIsOn = true;
        storePanel.SetActive(true);

    }

    //以购买格式打开商店
    public void OpenBuyStore(Inventory store)
    {
        sellIsOn = false;
        storePanel.SetActive(true);
        storeInventory = store;
    }
    public static void RefreshItem()
    {
        //直接毁掉整个 视觉层 的背包!
        for (int i = 0; i < storeInstance.slotGrid.transform.childCount; i++)
        {
            if (storeInstance.slotGrid.transform.childCount == 0)
                break;
            Destroy(storeInstance.slotGrid.transform.GetChild(i).gameObject);
            storeInstance.slots.Clear();
        }

        //再根据数据层的背包重建视觉层的背包(分为两种模式)

        if (storeInstance.sellIsOn)
        {
            int j = 0;
            for (int i = 0; i < storeInstance.myBag.itemList.Count; i++)
            {
                storeInstance.slots.Add(Instantiate(storeInstance.emptySlot));
                storeInstance.slots[i].transform.SetParent(storeInstance.slotGrid.transform);//设置父物体
                storeInstance.slots[i].GetComponent<Slot>().slotID = i;//同步ID
                if (storeInstance.myBag.itemList[i] != null && storeInstance.myBag.itemList[i].saleable == true)
                {
                    storeInstance.slots[j].GetComponent<Slot>().SetupStoreSlot(storeInstance.myBag.itemList[i]);//同步图片等物品信息
                    j++;
                }
            }
            for (int i = j; i < storeInstance.myBag.itemList.Count; i++)
            {
                storeInstance.slots[i].GetComponent<Slot>().SetupStoreSlot(null);//同步图片等物品信息
            }

        }

        else
        {
            for (int i = 0; i < storeInstance.storeInventory.itemList.Count; i++)
            {
                storeInstance.slots.Add(Instantiate(storeInstance.emptySlot));
                storeInstance.slots[i].transform.SetParent(storeInstance.slotGrid.transform);//设置父物体
                storeInstance.slots[i].GetComponent<Slot>().slotID = i;//同步ID
                storeInstance.slots[i].GetComponent<Slot>().SetupSlot(storeInstance.storeInventory.itemList[i]);//同步图片等物品信息
            }
        }

    }


    public static void UpdateItemInfo(string ItemName, string ItemDescription, Sprite ItemImage, string ItemPrice)
    {
        if (storeInstance.sellIsOn)
        {
            storeInstance.itemInformation.text = ItemDescription;
            storeInstance.itemName.text = ItemName;
            storeInstance.itemImagePanel.sprite = ItemImage;
            //卖价当然是有折扣的!
            storeInstance.itemPrice.text = "单价¥" + ((int)(int.Parse(ItemPrice) * 0.3)).ToString();
            storeInstance.item = storeInstance.player.AllItems.itemList[storeInstance.player.transaction.FindItem(ItemName)];
        }

        else
        {
            storeInstance.itemInformation.text = ItemDescription;
            storeInstance.itemName.text = ItemName;
            storeInstance.itemImagePanel.sprite = ItemImage;
            storeInstance.itemPrice.text = "单价¥" + ItemPrice;
            storeInstance.item = storeInstance.player.AllItems.itemList[storeInstance.player.transaction.FindItem(ItemName)];
        }

        storeInstance.itemNumber = 0;
        storeInstance.UpdateNumber();
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
                    priceText.text = (itemNumber * (int)(item.price * 0.3)).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else if (itemNumber <= item.itemHeld)
                {
                    priceText.text = (itemNumber * (int)(item.price * 0.3)).ToString();
                    numberText.text = itemNumber.ToString();
                }
                else if (itemNumber > item.itemHeld)
                {
                    itemNumber = item.itemHeld;
                    priceText.text = (itemNumber * (int)(item.price * 0.3)).ToString();
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


    public void SellOnClick()
    {
        UpdateNumber();
        if (item != null && itemNumber != 0)
        {
            coefficient = 0.3f;
            player.transaction.Sell(item, itemNumber, coefficient, true);


            itemNumber = 0;
            UpdateNumber();

            RefreshItem();
            storeInstance.playerWealth.text = Coin.itemHeld.ToString();
        }
    }

    public void BuyOnClick()
    {
        UpdateNumber();
        if (item != null && itemNumber != 0)
        {
            coefficient = 1.0f;
            player.transaction.Buy(item, itemNumber, coefficient, true);


            itemNumber = 0;
            UpdateNumber();

            RefreshItem();
            storeInstance.playerWealth.text = Coin.itemHeld.ToString();
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