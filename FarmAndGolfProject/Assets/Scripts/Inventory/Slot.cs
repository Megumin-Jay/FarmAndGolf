using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{//挂在预制体上,这个预制体和我们原来那个背包的预制体的含义是一致的
 //一开始各个属性都是空的,在生成的时候咱再给它赋值,或者说 用的时候再赋值
    public int slotID;//空格ID 等于 物品ID
    public Item slotItem;//获取到Item 
    public Image slotImage;//Item的图片
    public Text slotNum;//Item有多少个
    public string slotInfo;//物品的描述信息

    public GameObject itemInSlot;
    public void ItemOnClicked()
    {//显然 要传入物品的文本, 不可能从背包管理脚本那里传啊, 只能从视觉层物品本身这里传过去,本来就是按钮嘛
        InventoryManager.UpdateItemInfo(slotInfo, slotImage.sprite);
    }

    //从数据层 生成物品 到视觉层 的方法
    public void SetupSlot(Item item)
    {
        if (item == null)//如果根据数据层,这个格子里没东西,隐藏未被赋值的"物品"
        {
            itemInSlot.SetActive(false);
            return;
        }
        //如果有东西,如下赋值
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.itemInfo;
    }
}