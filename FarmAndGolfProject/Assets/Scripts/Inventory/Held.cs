using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Held", menuName = "Inventory/New Held")]
public class Held : ScriptableObject
{
    public int[] held = new int[36];
}
