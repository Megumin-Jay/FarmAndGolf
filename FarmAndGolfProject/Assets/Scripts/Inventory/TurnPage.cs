using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPage : MonoBehaviour
{
    public bool directionIsLeft = true;
    public GameObject Grid;
    public void pageTurn()
    {
        if (!directionIsLeft)
        {
            if (Grid.transform.position.y + 582 < 100)
            {
                Grid.transform.position = new Vector3(Grid.transform.position.x, Grid.transform.position.y + 582, Grid.transform.position.z);
            }
        }

        if (directionIsLeft)
        {
            if (Grid.transform.position.y - 582 > -1600)
            {
                Grid.transform.position = new Vector3(Grid.transform.position.x, Grid.transform.position.y - 582, Grid.transform.position.z);
            }
        }
    }
}
