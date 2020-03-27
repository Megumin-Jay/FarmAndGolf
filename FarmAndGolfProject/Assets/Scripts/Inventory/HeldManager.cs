using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldManager : MonoBehaviour
{
    public int[] Held;
    public Inventory items;

    public void Save()
    {
        Held = new int[items.itemList.Count];
        for (int i = 0; i < items.itemList.Count; i++)
        {
            Held[i] = items.itemList[i].itemHeld;
        }
    }

    public void Load()
    {
        for (int i = 0; i < Held.Length; i++)
        {
            items.itemList[i].itemHeld = Held[i];
        }
    }
}
