using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsUI : MonoBehaviour
{
    public Text OutlineText;//提示文本
    public Text ContentText;//提示文本,应与上一致

    public void UpdateTooltip(string text)//更新文本的接口
    {
        OutlineText.text = text;
        ContentText.text = text;
    }

    //显示
    public void Show()
    {
        gameObject.SetActive(true);
    }

    //隐藏
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //暂停
    public void Pause(bool click)
    {
        Time.timeScale = Convert.ToInt32(click);
    }

}
