using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//用于隐藏一些初始不显示的panel
public class Passive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
