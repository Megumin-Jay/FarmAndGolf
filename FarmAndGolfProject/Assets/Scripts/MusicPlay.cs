using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlay : MonoBehaviour
{
    //单例模式
    private static MusicPlay _instance;
    //让它在场景切换的时候不被摧毁
    public static MusicPlay Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject musicPlayer = new GameObject("musicPlayer");
                _instance = musicPlayer.AddComponent<MusicPlay>();
                DontDestroyOnLoad(musicPlayer);
            }
            return _instance;
        }
    }

    public AudioClip[] music;
    [SerializeField] private AudioSource musicSetting;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        if (SceneManager.GetActiveScene().name == "GamePlay")
            musicSetting.clip = music[1]; 
        else
            musicSetting.clip = music[0];
        musicSetting.loop = true;
        musicSetting.volume = UISetting.Instance.BgMusicValue;
        musicSetting.Play();
    }
    public void ChangeMusic(int num)
    {
        if (num >= music.Length)
        { Debug.Log("设置了错误的音乐");return; }
        musicSetting.clip = music[num];
        musicSetting.Play();
    }
    // Update is called once per frame
    void Update()
    {
        musicSetting.volume = UISetting.Instance.BgMusicValue*UISetting.Instance.MainMusicValue;
    }
}
