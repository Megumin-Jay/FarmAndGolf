using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    //单例模式
    private static UISetting _instance;
    public static UISetting Instance
    { get
        {
            if(_instance==null)
            {
                GameObject uiSetting = new GameObject("UISettingScript");
                _instance = uiSetting.AddComponent<UISetting>();
                DontDestroyOnLoad(uiSetting);
            }
            return _instance;
        }
    }
    //让它在场景切换的时候不被摧毁
    //按键设置
    public Dictionary<string, KeyCode> btn_code = new Dictionary<string, KeyCode>();
    //屏幕分辨率
    [SerializeField] private bool isFullScreen = false;
    //音量设置方面
    [SerializeField] private float mainMusicValue = 1.0f;  //主音量的值
    [SerializeField] private float bgMusicValue = 1.0f;    //背景音量的值
    [SerializeField] private float effectMusicValue = 1.0f; //音效音量的值
    public float MainMusicValue
    { set { mainMusicValue = value;} get { return mainMusicValue; } }
    public float BgMusicValue
    { set { bgMusicValue = value; } get { return bgMusicValue; } }
    public float EffectMusicValue
    { set { effectMusicValue = value;} get { return effectMusicValue; } }
    //屏幕分辨率设置
    public bool IsFullScreen
    { set { isFullScreen = value; SetScreen(value); }get { return isFullScreen; } }
    private void SetScreen(bool value)  //如果不是全屏，则是1920×1080的分辨率
    {
        if (value)
            Screen.fullScreen = value;
        else
            Screen.SetResolution(1920, 1080, false);
    }
    //按键设置
    public void SetKeyCode(string key,KeyCode newkey)
    {
        btn_code[key] = newkey;
    }
}
