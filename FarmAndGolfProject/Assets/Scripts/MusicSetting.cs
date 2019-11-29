using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSetting : MonoBehaviour
{
    public Slider mainMusic_Slider;
    public Slider bgMusic_Slider;
    public Slider effectMusic_Slider;
    // Start is called before the first frame update
    void Start()
    {
        //不加这些if语句，unity会报错：调用了一个空的引用。也不知道为什么。。。
        if (mainMusic_Slider == null)
            mainMusic_Slider = GameObject.Find("MainMusic").GetComponent<Slider>();
        if (bgMusic_Slider == null)
            bgMusic_Slider = GameObject.Find("BgMusic").GetComponent<Slider>();
        if (effectMusic_Slider == null)
            effectMusic_Slider = GameObject.Find("EffectMusic").GetComponent<Slider>();
        mainMusic_Slider.value = UISetting.Instance.MainMusicValue;
        bgMusic_Slider.value = UISetting.Instance.BgMusicValue;
        effectMusic_Slider.value = UISetting.Instance.EffectMusicValue;
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
}
