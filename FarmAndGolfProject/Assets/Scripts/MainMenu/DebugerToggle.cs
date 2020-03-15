using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//点击时打印出自身文本，纯粹debug用
public class DebugerToggle : MonoBehaviour
{
    public Text text;
    public Toggle tog;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.transform.Find("Label").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void debuger()
    {
        if (tog.isOn)
        Debug.Log(text.text.ToString());
    }
}
