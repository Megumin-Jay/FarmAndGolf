using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmOperation : MonoBehaviour
{
    [SerializeField] Earth earth = null;
    public Earth My_earth
    {
        set { earth = value; }
    }
    [SerializeField] int op = 0;//0表示未选中操作，1表示浇水，2表示施肥，3表示种植，4表示收获
    public int OP { get { return op; } }
    [SerializeField] GameObject[] plant;
    [SerializeField] int plant_num = -1;
    public int Plant_num { set { plant_num = value; } }
    public TipsUI tips;//更新提示用

    private void Start()
    {
        tips.Hide();
    }
    private void Update()
    {

    }

    public void SetWater()     { op = 1; plant_num = -1; tips.UpdateTooltip("现在是浇水操作"); }
    public void SetFertilize() { op = 2; plant_num = -1; tips.UpdateTooltip("现在是施肥操作"); }
    public void SetPlant()     { op = 3; tips.UpdateTooltip("现在是种植操作"); }
    public void SetHarvest()   { op = 4; plant_num = -1; tips.UpdateTooltip("现在是收获操作"); }
    public void SetReset()     { op = 0; plant_num = -1; tips.UpdateTooltip("还未选中任何操作哦"); }

    public void Plant()
    {
        if (earth == null)
        {
            tips.UpdateTooltip("还未选中任何土地哦");
            return;
        }
        else if(earth.Occupied)
        {
            tips.UpdateTooltip("很遗憾，选中土地已经有其他植物啦");
            earth = null;
            return;
        }
        else if (earth.Dry)
        {
            tips.UpdateTooltip("很遗憾，选中土地是干旱的，要先为土地浇水哦");
            earth = null;
            return;
        }
        else if (plant_num == -1)
        {
            tips.UpdateTooltip("还未选中任何植物哦");
            earth = null;
            return;
        }
        GameObject myplant = Instantiate(plant[plant_num]);
        Plant pplant = myplant.GetComponent<Plant>();
        if (pplant == null)
        {
            tips.UpdateTooltip("很遗憾，出错了");
            earth = null;
            return;
        }
        myplant.transform.parent = earth.transform;
        myplant.transform.localPosition = new Vector3(0,0,-0.01f);
        pplant.SetEarth(earth);
        earth.PlantTheEarth(pplant);
        tips.UpdateTooltip("种植成功啦");
        earth = null;
    }
    public void Water()
    {
        if (earth != null)
        {
            string t = "";
            if (earth.WaterThread())
                t = "浇水成功啦\n";
            else
                t = "这块土地已经湿润，浇水失败了\n";
            if (earth.Wet&& earth.Fertilized)
                t +="土地的状态是：湿润并肥沃";
            else if(earth.Wet)
                t += "土地的状态是：湿润";
            else if (earth.Fertilized)
                t += "土地的状态是：肥沃";
            else
                t += "土地的状态是：普通";
            tips.UpdateTooltip(t);
            earth = null;
        }
        earth = null;
    }
    public void Fertilize()
    {
        if (earth != null)
        {
            string t = "";
            if (earth.FertilizeThread())
                t = "施肥成功\n";
            else if (earth.Dry)
                t = "很遗憾，选中土地是干旱的，要先为土地浇水哦";
            else if (earth.Fertilized)
                t = "这块土地已经肥沃，施肥失败了";
            if (earth.Wet && earth.Fertilized)
                t += "土地的状态是：湿润并肥沃";
            else if (earth.Wet)
                t += "土地的状态是：湿润";
            else if (earth.Fertilized)
                t += "土地的状态是：肥沃";
            else
                t += "土地的状态是：普通";
            tips.UpdateTooltip(t);
            earth = null;
        }
        earth = null;
    }
    public void Harvest()
    {
        if (earth == null)
            return;
        if (!earth.Occupied)
        { tips.UpdateTooltip("这块土地没有种植植物哦"); earth = null; return; }
        int numoffruit=0;
        earth.Myplant.Ripe(ref numoffruit);
        if (numoffruit <= 0)
            tips.UpdateTooltip("果实还未成熟再等等吧");
        else
        { tips.UpdateTooltip("恭喜，你获得了" + numoffruit + "个果实");earth.HarvestTheEarth(); }
        earth = null;
    }
}