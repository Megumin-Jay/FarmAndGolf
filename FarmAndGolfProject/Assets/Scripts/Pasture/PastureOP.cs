using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastureOP : MonoBehaviour
{
    public Tank watertank;
    public Tank foodtank;
    public Vector3 brithPoint;

    [SerializeField] private AnimalGrowth animal=null;
    public AnimalGrowth My_Ani { set { animal = value; } }
    [SerializeField] private GameObject ani;
    [SerializeField] private int op=0;
    [SerializeField] GameObject pasture = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (op)
            {
                case 0: Debug.Log("未选中操作"); break;
                case 1: Water(); break;
                case 2: Food(); break;
                case 3: NewAnimal();break;
                case 4: Harvest(); animal = null; break;
            }
        }
    }

    public void SetWater() { op = 1; }
    public void SetFood() { op = 2; }
    public void SetNewAnimal() { op = 3; }
    public void SetHarvest() { op = 4; }
    public void SetReset() { op = 0; }
    public void Water()
    {
        if (watertank != null)
            watertank.Supply();
        else Debug.Log("没有绑定水槽");
    }
    public void Food()
    {
        if (foodtank != null)
            foodtank.Supply();
        else Debug.Log("没有绑定饲料槽");
    }
    public void NewAnimal()
    {
        if (pasture == null || ani == null)
        {
            Debug.Log("新动物失败");
            return;
        }
        GameObject myani = Instantiate(ani);
        myani.transform.parent = pasture.transform;
        myani.transform.position = brithPoint;
        Debug.Log(myani.transform);
    }
    public void Harvest()
    {
        if (animal == null)
            return;
        int numofani = 0;
        animal.Harvest(ref numofani);
        if (numofani <= 0)
            Debug.Log("动物还未成熟");
        else
        { Debug.Log("你获得了" + numofani + "个果实"); }
    }
}
