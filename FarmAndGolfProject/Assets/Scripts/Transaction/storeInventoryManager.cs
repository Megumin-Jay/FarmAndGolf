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

    //碰撞检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            player = other.GetComponent<Player>();//获取角色的脚本
        }
    }

    void Awake()//单例
    {
        if (storeInstance != null)
            Destroy(this);
        storeInstance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        storeInstance.itemPrice.text = "";
        storeInstance.itemName.text = "";
        storeInstance.itemInformation.text = "";//这个背包的描述框是一直显示的,所以要保证没点击物品的时候描述为"空"
        storeInstance.playerWealth.text = Coin.itemHeld.ToString();
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

        if (storeInstance.sellIsOn)
        {
            int j = 0;
            for (int i = 0; i < storeInstance.myBag.itemList.Count; i++)
            {
                storeInstance.slots.Add(Instantiate(storeInstance.emptySlot));
                storeInstance.slots[i].transform.SetParent(storeInstance.slotGrid.transform);//设置父物体
                storeInstance.slots[i].GetComponent<Slot>().slotID = i;//同步ID
                storeInstance.slots[j].GetComponent<Slot>().SetupStoreSlot(storeInstance.myBag.itemList[i]);//同步图片等物品信息
                j++;
                if (!storeInstance.slots[i].GetComponent<Slot>().itemInSlot.activeSelf)
                {
                    j--;
                }
            }

        }

        else
        {
            //再根据数据层的背包重建视觉层的背包
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
        }

        else
        {
            storeInstance.itemInformation.text = ItemDescription;
            storeInstance.itemName.text = ItemName;
            storeInstance.itemImagePanel.sprite = ItemImage;
            //卖价当然是有折扣的!
            storeInstance.itemPrice.text = "单价¥" + ItemPrice;
        }

    }

}