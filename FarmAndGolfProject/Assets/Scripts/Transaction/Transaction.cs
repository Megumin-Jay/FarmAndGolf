﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transaction : MonoBehaviour
{
    public Inventory playerInventory;//取得玩家背包
    public Inventory shopInventory;//商店背包
    public TipsUI popUps;//弹窗

    // 嘛,写的时候没咋考虑number的取值范围,要在调number的地方下手了
    // buy的时候非负,sell的时候非负且小于等于item什么的


    //输入物品+数量+倍率,实现从"收到售出请求"到"金币入库"之间所有操作(阿,确实比buy好写多了orz
    public void Sell(Item item, int number, float coefficient)
    {
        item.itemHeld -= number;
        InventoryManager.wealthValue += (int)coefficient * item.price * number;
        InventoryManager.RefreshItem();
        ShowPopups("物品已售出!您获得了" + number.ToString() + "枚金币!");
    }

    //输入物品+数量+倍率,实现从"收到买入请求"到"物品入库"之间所有操作    
    public void Buy(Item item, int number, float coefficient)
    {
        if (InventoryManager.wealthValue - (int)coefficient * item.price * number > 0)
        {
            if (!playerInventory.itemList.Contains(item))//如果背包中没这个
            {
                for (int i = 0; i < playerInventory.itemList.Count; i++)
                {
                    if (playerInventory.itemList[i] == null)//找空格子,有就给他的物品赋值
                    {
                        playerInventory.itemList[i] = item;//赋值
                        item.itemHeld = number;
                        break;
                    }
                }
            }
            else
            {
                item.itemHeld += number;//如果原本就有,则增加"这个物品"的数量
            }
            InventoryManager.wealthValue -= (int)coefficient * item.price * number;
            InventoryManager.RefreshItem();
            ShowPopups("\"" + item.name + "\"×" + number.ToString() + "已放入您的背包!");
        }
        else
        {
            ShowPopups("哼哼,还是多挣些钱再来吧!");
        }
    }

    //弹窗
    public void ShowPopups(string str)
    {
        popUps.UpdateTooltip(str);
        popUps.Show();//显示"确认"弹窗
    }
}
