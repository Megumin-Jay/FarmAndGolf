using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldManager : MonoBehaviour
{
    public Held Held;
    public Inventory items;


    public void Save()//按物品在全集中的顺序,写入物品的持有数
    {
        for (int i = 0; i < items.itemList.Count; i++)
        {
            Held.held[i] = items.itemList[i].itemHeld;
        }
    }

    public void Load()//加载数据
    {
        for (int i = 0; i < items.itemList.Count; i++)
        {
            items.itemList[i].itemHeld = Held.held[i];
        }
    }
}
