using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//写到另外一个脚本上了，这个弃用
public class MusicSetting : MonoBehaviour
{
    public Slider mainMusic_Slider;
    public Slider bgMusic_Slider;
    public Slider effectMusic_Slider;
    // Start is called before the first frame update
    void Start()
    {
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
