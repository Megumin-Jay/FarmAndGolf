using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onOrOff : MonoBehaviour
{
    //简单实现"单击打开,再击关闭"
    public GameObject obj;
    public void Switch()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
