using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmOperation : MonoBehaviour
{
    [SerializeField] Earth earth = null;
    [SerializeField] int op = 0;//0表示未选中操作，1表示浇水，2表示施肥，3表示种植，4表示收获
    [SerializeField] GameObject[] plant;
    [SerializeField] int plant_num = -1;
    public int Plant_num { set { plant_num = value; } }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch(op)
            {
                case 0: Debug.Log("未选中操作");break;
                case 1: Water(); earth = null; break;
                case 2: Fertilize(); earth = null; break;
                case 3: Plant(); earth = null; break;
                case 4: Harvest(); earth = null; break;
            }
        }
    }

    public Earth My_earth
    {
        set { earth = value; }
    }
    public void SetWater()     { op = 1; plant_num = -1; }
    public void SetFertilize() { op = 2; plant_num = -1; }
    public void SetPlant()     { op = 3; }
    public void SetHarvest()   { op = 4; plant_num = -1; }
    public void SetReset()     { op = 0; plant_num = -1; }

    public void Plant()
    {
        if (earth == null||earth.Occupied||earth.Dry||plant_num == -1)
        {
            Debug.Log("种植失败");
            return;
        }
        GameObject myplant = Instantiate(plant[plant_num]);
        Plant pplant = myplant.GetComponent<Plant>();
        if (pplant == null)
        {
            Debug.Log("出错啦");
            return;
        }
        myplant.transform.parent = earth.transform;
        myplant.transform.localPosition = new Vector3(0,0,-0.01f);
        pplant.SetEarth(earth);
        earth.PlantTheEarth(pplant);
    }
    public void Water()
    {
        if (earth != null)
        {
            if (earth.WaterThread())
                Debug.Log("浇水成功");
            else
                Debug.Log("这块土地已经湿润，浇水失败");
        }
    }
    public void Fertilize()
    {
        if (earth != null)
        {
            if (earth.FertilizeThread())
                Debug.Log("施肥成功");
            else
                Debug.Log("这块土地已经肥沃，施肥失败");
        }
    }
    public void Harvest()
    {
        if (earth == null)
            return;
        if(!earth.Occupied)
            Debug.Log("还未种植");
        int numoffruit=0;
        earth.Myplant.Ripe(ref numoffruit);
        if (numoffruit <= 0)
            Debug.Log("果实还未成熟");
        else
        { Debug.Log("你获得了" + numoffruit + "个果实"); earth.HarvestTheEarth(); }
    }
}