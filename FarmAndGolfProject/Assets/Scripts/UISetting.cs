using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    //单例模式
    private static UISetting _instance;
    //让它在场景切换的时候不被摧毁
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
    //按键设置
    public Dictionary<string, KeyCode> btn_code = new Dictionary<string, KeyCode>();
    //屏幕分辨率
    [SerializeField] private bool isFullScreen;
    //音量设置方面
    [SerializeField] private float mainMusicValue;  //主音量的值
    [SerializeField] private float bgMusicValue;    //背景音量的值
    [SerializeField] private float effectMusicValue; //音效音量的值
    public float MainMusicValue
    { set { mainMusicValue = value;PlayerPrefs.SetFloat("mainMusicValue", mainMusicValue); } get { return mainMusicValue; } }
    public float BgMusicValue
    { set { bgMusicValue = value; PlayerPrefs.SetFloat("bgMusicValue", bgMusicValue); } get { return bgMusicValue; } }
    public float EffectMusicValue
    { set { effectMusicValue = value; PlayerPrefs.SetFloat("effectMusicValue", effectMusicValue); } get { return effectMusicValue; } }
    //屏幕分辨率设置
    public bool IsFullScreen
    { set {
            isFullScreen = value; SetScreen(value);
            PlayerPrefs.SetString("isFullScreen", isFullScreen.ToString());
          }
      get { return isFullScreen; }
    }
    private void SetScreen(bool value)  //如果不是全屏，则是1920×1080的分辨率
    {
        if (value)
            Screen.fullScreen = value;
        else
            Screen.SetResolution(1920, 1080, false);
    }
    private void Awake()
    {
        isFullScreen = bool.Parse(PlayerPrefs.GetString("isFullScreen", "false"));
        SetScreen(isFullScreen);
        mainMusicValue = PlayerPrefs.GetFloat("mainMusicValue", 1.0f);  //主音量的值
        bgMusicValue = PlayerPrefs.GetFloat("bgMusicValue", 1.0f);    //背景音量的值
        effectMusicValue = PlayerPrefs.GetFloat("effectMusicValue", 1.0f); //音效音量的值
    }

    //按键设置
    public void SetKeyCode(string key,KeyCode newkey)
    {
        btn_code[key] = newkey;
    }
}
