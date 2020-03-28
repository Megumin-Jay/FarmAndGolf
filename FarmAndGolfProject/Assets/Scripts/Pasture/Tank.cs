using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] float max = 2500f;//最多存放量
    [SerializeField] float now = 0.0f;//现有量

    public float Now { set { now = value; } get { return now; } }

    public void Supply()
    {
        if(now>=max)
        {
            Debug.Log("槽已满");
        }
        else
        {
            now = max;
        }
    }

    public bool Consume(AnimalGrowth animal,int state)//state=0表示要喝水，state=1表示要吃饲料
    {
        if(now<=0)
        {
            Debug.Log("槽空了，请补充");
            return false;
        }
        float con;
        if (state==0)
        {
            con = 100 - animal.WaterQuantity;
            if (con <= now)
            {
                now -= con;
                animal.WaterQuantity = 100;
            }
            else
            {
                animal.WaterQuantity += now;
                now = 0;
            }
        }
        else
        {
            con = 100 - animal.FoodQuantity;
            if (con <= now)
            {
                now -= con;
                animal.FoodQuantity = 100;
            }
            else
            {
                animal.FoodQuantity += now;
                now = 0;
            }
        }
        return true;
    }
}
