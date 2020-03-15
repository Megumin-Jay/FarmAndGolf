using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//用来切换panel
public class PanelChange : MonoBehaviour
{
    public GameObject NowPanel;
    public GameObject NextPanel;


    public void change()
    {
        NowPanel.SetActive(false);
        NextPanel.SetActive(true);
    }
}
