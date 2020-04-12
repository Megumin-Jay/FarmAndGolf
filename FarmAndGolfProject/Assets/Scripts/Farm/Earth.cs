using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    public float waterTimeLength = 5;//浇一次水，可保持土地湿润4H
    public float fertilizedTimeLength = 5;//施一次肥，可保持土地肥沃4H
    public float dryTimeLength = 5;//

    [SerializeField] int condition;//土地的状态，0代表干旱，1代表普通，2代表湿润，3代表普通+施肥，4代表湿润+施肥
    [SerializeField] bool occupied;
    [SerializeField] Plant myplant = null;
    [SerializeField] float watertime = -1;//浇水时间
    [SerializeField] float waterbegintime = -1;//浇水开始时间
    [SerializeField] float fertilizedtime = -1;//施肥时间
    [SerializeField] float fertilizedbegintime = -1;//施肥开始时间
    public Sprite[] sp;//各状态的贴图
    SpriteRenderer this_sp;//现有的贴图

    public bool Fertilized //检查是否施肥
    {
        get { return condition == 3 || condition == 4; }
    }
    public bool Dry  //检查是否干旱
    {
        get { return condition == 0; }
    }
    public bool Wet//检查是否湿润
    {
        get { return condition == 2 || condition == 4; }
    }
    public bool Occupied//检查是否占有
    {
        get { return occupied; }
    }
    public int Condition { get { return condition; } }
    public Plant Myplant { get { return myplant; } }
    public float Watertime { get { return watertime; } }
    public float Waterbegintime { get { return waterbegintime; } }
    public float Fertilizedtime { get { return fertilizedtime; } }
    public float Fertilizedbegintime { get { return fertilizedbegintime; } }

    private void Start()
    {
        if (!LoadXml())
        {
            condition = 0;
            occupied = false;
        }
        else
        {
            if (Wet)
                StartCoroutine("WaterTheEarth");
            if (Fertilized)
                StartCoroutine("FertilizeTheEarth");
        }
        this_sp = this.gameObject.GetComponent<SpriteRenderer>();
        this_sp.sprite = sp[0];
    }
    private void Update()
    {
        if (watertime >= 0)
            watertime = Time.time - waterbegintime;
        else
        { watertime = -1; waterbegintime = -1; }
        if (fertilizedtime >= 0)
            fertilizedtime = Time.time - fertilizedbegintime;
        else
        { fertilizedtime = -1; fertilizedbegintime = -1; }
        switch (condition)
        {
            case 0: this_sp.sprite = sp[0]; break;
            case 1: this_sp.sprite = sp[1]; break;
            case 2: this_sp.sprite = sp[2]; break;
            case 3: this_sp.sprite = sp[3]; break;
            case 4: this_sp.sprite = sp[4]; break;
        }
    }


    //用协程控制土地干湿、施肥状况
    public bool WaterThread()
    {
        switch (condition)
        {
            case 4:
            case 2: return false;
            default: if (!Dry) { watertime = 0; waterbegintime = Time.time; } StartCoroutine("WaterTheEarth"); break;
        }
        return true;
    }
    IEnumerator WaterTheEarth()
    {
        if (Dry)
        {
            condition = 1;
            yield break;
        }
        else if (!Wet)
        {
            if (Fertilized)
                condition = 4;
            else
                condition = 2;
            yield return new WaitForSeconds(waterTimeLength - watertime);
            watertime = -1;
            if (Fertilized)
                condition = 3;
            else
                condition = 1;

        }
    }
    public bool FertilizeThread()
    {
        if (Fertilized || Dry)
            return false;
        fertilizedtime = 0; fertilizedbegintime = Time.time;
        StartCoroutine("FertilizeTheEarth");
        return true;
    }
    IEnumerator FertilizeTheEarth()//施肥时，改变土地施肥状况
    {
        if (Wet)
            condition = 4;
        else
            condition = 3;
        yield return new WaitForSeconds(fertilizedTimeLength - fertilizedtime);
        fertilizedtime = -1;
        if (Wet)
            condition = 2;
        else
            condition = 1;

    }
    public bool PlantTheEarth(Plant plant) //种植时，改变土地占有状况
    {
        if (occupied)
            return false;
        else
        {
            occupied = true;
            myplant = plant;
            return true;
        }
    }
    public bool HarvestTheEarth() //收获时，改变土地占有状况
    {
        if (occupied)
        {
            occupied = false;
            myplant = null;
            return true;
        }
        return false;
    }

    bool LoadXml()
    {
        string filePath = Application.dataPath + "/" + "EarthData.xml";
        if (File.Exists(filePath))
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filePath);
            XmlElement elem = xmldoc.GetElementsByTagName(this.name)[0] as XmlElement;
            XmlNode con = elem.SelectSingleNode("condition");
            condition = int.Parse(con.InnerText);
            con = elem.SelectSingleNode("myplant");
            if (con.InnerText != "null")
            {
                XmlNode next = elem.SelectSingleNode("myplant/plantType");
                string type = next.InnerText;
                GameObject gameObject = (GameObject)Resources.Load(type);
                gameObject = Instantiate(gameObject);
                myplant = gameObject.GetComponent<Plant>();
                if (myplant != null)
                {
                    myplant.transform.parent = this.transform;
                    myplant.transform.localPosition = new Vector3(0, 0, -0.01f);
                    myplant.SetEarth(this);
                    next = elem.SelectSingleNode("myplant/totaltime");
                    myplant.Totaltime = float.Parse(next.InnerText);
                    next = elem.SelectSingleNode("myplant/stage");
                    myplant.Stage = int.Parse(next.InnerText);
                }
            }
            else
                myplant = null;
            con = elem.SelectSingleNode("watertime");
            watertime = float.Parse(con.InnerText);
            con = elem.SelectSingleNode("waterbegintime");
            waterbegintime = float.Parse(con.InnerText);
            con = elem.SelectSingleNode("fertilizedtime");
            fertilizedtime = float.Parse(con.InnerText);
            con = elem.SelectSingleNode("fertilizedbegintime");
            fertilizedbegintime = float.Parse(con.InnerText);
            return true;
        }
        else
        {
            Debug.Log("文档不存在");
            return false;
        }
    }
}