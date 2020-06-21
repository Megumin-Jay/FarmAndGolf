using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用来存储物品的所有信息
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    //所有物品的基类,包含了物品的名字,图片,持有数量,描述,是否可售卖
    public Sprite itemImage;
    public int itemHeld;
    public string itemName;
    [TextArea] public string itemInfo;
    public bool saleable = true;//是否可售卖
    public int price;//价格,亦代表稀有度
}
