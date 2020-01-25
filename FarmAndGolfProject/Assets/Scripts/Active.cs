using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//用来切换panel
public class Active : MonoBehaviour
{
    public GameObject NowPanel;
    public GameObject NextPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changer()
    {
        NowPanel.SetActive(false);
        NextPanel.SetActive(true);
    }
}
