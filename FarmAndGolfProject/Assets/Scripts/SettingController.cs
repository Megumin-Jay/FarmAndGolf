using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Slider mainMusic_Slider;
    public Slider bgMusic_Slider;
    public Slider effectMusic_Slider;
    public Toggle fullScreen_Toggle;
    public Toggle otherScreen_Toggle;
    public GameObject musicSetting;
    public GameObject screensizeSetting;
    public GameObject buttonchangeSetting;

    private void Start()
    {
        mainMusic_Slider.value = UISetting.Instance.MainMusicValue;
        bgMusic_Slider.value = UISetting.Instance.BgMusicValue;
        effectMusic_Slider.value = UISetting.Instance.EffectMusicValue;
        fullScreen_Toggle.isOn = UISetting.Instance.IsFullScreen;
        otherScreen_Toggle.isOn = !fullScreen_Toggle.isOn;
        MusicScence();
    }
    //打算用Slider自带的事件
    public void Con_MainMusic()
    {
        if (mainMusic_Slider == null)
            return;
        UISetting.Instance.MainMusicValue = mainMusic_Slider.value;
    }
    public void Con_BgMusic()
    {
        if (bgMusic_Slider == null)
            return;
        UISetting.Instance.BgMusicValue = bgMusic_Slider.value;
    }
    public void Con_EffectMusic()
    {
        if (effectMusic_Slider == null)
            return;
        UISetting.Instance.EffectMusicValue = effectMusic_Slider.value;
    }
    //打算用Toggle自带的事件
    public void Con_ScreenSize()
    {
        UISetting.Instance.IsFullScreen = fullScreen_Toggle.isOn;
    }
    //打算用Button自带的事件
    public void MusicScence()
    {
        musicSetting.SetActive(true);
        screensizeSetting.SetActive(false);
        buttonchangeSetting.SetActive(false);
    }
    public void ScreenSizeScence()
    {
        musicSetting.SetActive(false);
        screensizeSetting.SetActive(true);
        buttonchangeSetting.SetActive(false);
    }
    public void ButtonChangeScence()
    {
        musicSetting.SetActive(false);
        screensizeSetting.SetActive(false);
        buttonchangeSetting.SetActive(true);
    }
    public void ButtonClosedScence()
    {
        GameObject UIset = GameObject.Find("UIsetting");
        if (UIset != null)
            UIset.SetActive(false);
    }
    //暂时先用这个函数恢复时间流速	
    public void Pause(bool click)
    {
        Time.timeScale = Convert.ToInt32(click);
    }
}
