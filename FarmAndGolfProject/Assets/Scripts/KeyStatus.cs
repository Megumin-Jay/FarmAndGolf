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
        //返回上一个按键状态
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            switch (_KeyStatu)
            {
                case KeyStatu.Initiate:
                    _KeyStatu = KeyStatu.Initiate;
                    break;
                case KeyStatu.ChooseClub:
                    _KeyStatu = KeyStatu.Initiate;
                    break;
                case KeyStatu.ChooseDirOne:
                    _KeyStatu = KeyStatu.ChooseClub;
                    break;
                case KeyStatu.ChooseDirTwo:
                    _KeyStatu = KeyStatu.ChooseDirOne;
                    break;
                case KeyStatu.CheckSliderValue:
                    _KeyStatu = KeyStatu.ChooseDirTwo;
                    break;
                case KeyStatu.GetSliderValue:
                    _KeyStatu = KeyStatu.CheckSliderValue;
                    break;
                case KeyStatu.Reset:
                    _KeyStatu = KeyStatu.GetSliderValue;
                    break;
                case KeyStatu.Shoot:
                    _KeyStatu = KeyStatu.Reset;
                    break;
            }
        }
        switch (_KeyStatu)
        {
            case KeyStatu.Initiate :
                Text.text = /*"鼠标<color=red><I>左键</I></color>点击屏幕某处"*/ "";
                break;
            case KeyStatu.ChooseClub:
                Text.text = "请按<color=red><I>左Ctrl/左Shift</I></color>键\n选择你的球杆,选择完\n按<color=red><I>U</I></color>键确定";
                break;
            case KeyStatu.ChooseDirOne:
                Text.text = "通过<color=red><I>A，D</I></color>键\n选择球要击打的方向";
                break;
            case KeyStatu.ChooseDirTwo:
                Text.text = "按<color=red><I>空格</I></color>键滑条开始运动";
                break;
            case KeyStatu.CheckSliderValue:
                Text.text = "第二次按<color=red><I>空格</I></color>键，\n确定击打的精准度";
                break;
            case KeyStatu.GetSliderValue:
                Text.text = /*"按<color=red><I>R</I></color>键准备击打"*/ "";
                break;
            case KeyStatu.Reset:
                Text.text = /*"按<color=red><I>Z</I></color>键将球打出"*/ "";
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
    ChooseClub,
    CheckSliderValue,
    GetSliderValue,
    Reset,
    Shoot,
    Initiate
}
