using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPage : MonoBehaviour
{
    public bool directionIsLeft = true;
    public GameObject Grid;
    public float onePageLength;//一页的长度
    public float upperBound;//上界
    public float lowerBound;//下界

    //换页
    public void pageTurn()
    {
        if (!directionIsLeft)
        {
            if (Grid.transform.position.y + onePageLength - upperBound < 0)
            {
                Grid.transform.position = new Vector3(Grid.transform.position.x, Grid.transform.position.y + onePageLength, Grid.transform.position.z);
            }
        }

        if (directionIsLeft)
        {
            if (Grid.transform.position.y - onePageLength - lowerBound > 0)
            {
                Grid.transform.position = new Vector3(Grid.transform.position.x, Grid.transform.position.y - onePageLength, Grid.transform.position.z);
            }
        }
    }
}
