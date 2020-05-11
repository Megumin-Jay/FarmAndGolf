using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EarthDataSave : MonoBehaviour
{
    [SerializeField] GameObject[] earthsObject;
    // Start is called before the first frame update
    void Start()
    {
        earthsObject = GameObject.FindGameObjectsWithTag("Earth");
    }

    public void SaveDataXml()
    {
        int num = earthsObject.Length;
        string filePath = Application.dataPath + "/DataSaveXml/" + "EarthData.xml";
        XmlDocument xmldoc = new XmlDocument();
        XmlElement root = xmldoc.CreateElement("Earthdata");
        for(int i=0;i<num;i++)
        {
            Earth earth = earthsObject[i].GetComponent<Earth>();
            XmlElement name = xmldoc.CreateElement(earth.name);
            XmlElement elem = xmldoc.CreateElement("condition");
            elem.InnerText = earth.Condition.ToString();
            name.AppendChild(elem);
            elem = xmldoc.CreateElement("occupied");
            elem.InnerText = earth.Occupied.ToString();
            name.AppendChild(elem);
            elem = xmldoc.CreateElement("myplant");
            if (earth.Myplant != null)
            {
                XmlElement plant = xmldoc.CreateElement("plantType");
                plant.InnerText = earth.Myplant.typename;
                elem.AppendChild(plant);
                plant = xmldoc.CreateElement("totaltime");
                plant.InnerText = earth.Myplant.Totaltime.ToString();
                elem.AppendChild(plant);
                plant = xmldoc.CreateElement("stage");
                plant.InnerText = earth.Myplant.Stage.ToString();
                elem.AppendChild(plant);
            }
            else
                elem.InnerText = "null";
            name.AppendChild(elem);
            elem = xmldoc.CreateElement("watertime");
            elem.InnerText = earth.Watertime.ToString();
            name.AppendChild(elem);
            elem = xmldoc.CreateElement("waterbegintime");
            elem.InnerText = earth.Waterbegintime.ToString();
            name.AppendChild(elem);
            elem = xmldoc.CreateElement("fertilizedtime");
            elem.InnerText = earth.Fertilizedtime.ToString();
            name.AppendChild(elem);
            elem = xmldoc.CreateElement("fertilizedbegintime");
            elem.InnerText = earth.Fertilizedbegintime.ToString();
            name.AppendChild(elem);
            root.AppendChild(name);
        }
        xmldoc.AppendChild(root);
        xmldoc.Save(filePath);
        if (File.Exists(filePath))
        {
            Debug.Log(filePath + ":保存成功");
        }
        else
            Debug.Log("保存失败");
    }
}
