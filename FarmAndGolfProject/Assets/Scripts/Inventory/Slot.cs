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
    public string slotDescription;//物品的描述信息
    public string slotName;//物品属性里的名称
    public string objectName;//物品obj名称

    public GameObject itemInSlot;

    //当被点击,传出自身的文本和图片
    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotName, slotDescription, slotImage.sprite);
        //截取objName前4个字符检测是不是球(憨批方法但是真的方便orz)
        if (objectName.Length > 4 && objectName.Substring(0, 4) == "Ball")
        {
            InventoryManager.getBallName(objectName, slotName);
        }
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
        slotDescription = item.itemInfo;
        slotName = item.itemName;
        objectName = item.name;
    }
}