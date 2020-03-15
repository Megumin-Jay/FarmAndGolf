using UnityEngine;
using System.Collections;

public class GridPanelUI : MonoBehaviour
{
    //持有所有格子
    public Transform[] Grids;

    //寻找空格子
    public Transform GetEmptyGrid()
    {
        for (int i = 0; i < Grids.Length; i++)
        {
            if (Grids[i].childCount == 0)
                return Grids[i];
        }
        return null;
    }
}
