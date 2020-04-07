using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//点击时打印出自身文本，纯粹debug用
public class Debuger : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.transform.Find("Text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void debuger()
    {
        Debug.Log(text.text.ToString());
    }
}
