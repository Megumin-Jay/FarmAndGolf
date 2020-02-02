using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyStatus : MonoBehaviour
{
    private static KeyStatus _instance;

    public KeyStatu _KeyStatu = KeyStatu.Initiate;

    //显示的UI
    public Text Text;
    
    public static KeyStatus _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(KeyStatus)) as KeyStatus;
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_KeyStatu)
        {
            case KeyStatu.Initiate :
                Text.text = "鼠标<color=red><I>左键</I></color>点击屏幕某处";
                break;
            case KeyStatu.ChooseDirOne:
                Text.text = "通过<color=red><I>A，D</I></color>键\n选择球要击打的方向";
                break;
            case KeyStatu.ChooseDirTwo:
                Text.text = "按<color=red><I>R</I></color>键准备击打";
                break;
            case KeyStatu.Reset:
                Text.text = "按<color=red><I>Z</I></color>键将球打出";
                break;
            case KeyStatu.Shoot:
                Text.text = "当球落地时，可击打下一球";
                break;
        }
    }
}

public enum KeyStatu
{
    ChooseBall,
    ChooseDirOne,
    ChooseDirTwo,
    Reset,
    Shoot,
    Initiate
}
