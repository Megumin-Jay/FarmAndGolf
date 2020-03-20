using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FishItems", menuName = "Inventory/New FishItems")]
public class FishItems : ScriptableObject
{
    //"鱼池",放所有能被钓出来的物品
    public List<Item> FishItemList = new List<Item>();
}