using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//用来将画廊中每张图片时将其自身的图片最大化（利用panel实现最大化以及点击图片返回）
public class GalleryUI : MonoBehaviour
{
    public Image ige;
    public Sprite spr;
    public GameObject NextPanel;
    // Start is called before the first frame update
    void Start()
    {
        spr = gameObject.transform.GetComponent<Image>().sprite;
    }


    //鼠标单击方法 点击即可"放大图片"
    public void Gallery()
    {
        ige.sprite = spr;
        NextPanel.SetActive(true);
    }
}
