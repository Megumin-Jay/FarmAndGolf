using UnityEngine;
using System.Collections;

/// <summary>
/// 所有物品的基类
/// </summary>
public class Item
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int BuyPrice { get; private set; }
    public int SellPrice { get; private set; }
    public string Icon { get; private set; }//图片的路径
    public ItemType ItemType { get; protected set; }

    //构造函数
    public Item(int id, string name, string description, int buyPrice, int sellPrice, string icon)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.BuyPrice = buyPrice;
        this.SellPrice = sellPrice;
        this.Icon = icon;
    }
}

public enum ItemType
{
    Weapon,
    Consumable,
    Armor
}
