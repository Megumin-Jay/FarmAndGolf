using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    /*游标是否能动*/
    [SerializeField]
    private bool canRun;
    /*游标是否能停*/
    [SerializeField]
    private bool canStop;
    /*Slider组件*/
    private Slider _slider;
    /*方法执行的最短时间*/
    private float localTime;

    public float sliderValue;
    /*用来将pingpong第一个参数置零*/
    //private float zeroTime;
    // Start is called before the first frame update
    void Start()
    {
        canRun = false;
        canStop = false;
        //zeroTime = 0;

        _slider = GameObject.FindWithTag("Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRun)
        { 
            MoveSlider();
            //isRunning = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !canRun && !canStop && KeyStatus._Instance._KeyStatu == KeyStatu.ChooseDirTwo)
        {
            canRun = true;
            localTime = 0;
            KeyStatus._Instance._KeyStatu = KeyStatu.CheckSliderValue;
        }

        

        else if (Input.GetKeyDown(KeyCode.Space) && canStop && KeyStatus._Instance._KeyStatu == KeyStatu.CheckSliderValue)
        {
            canStop = false;
            KeyStatus._Instance._KeyStatu = KeyStatu.GetSliderValue;
            //canRun = false;
            canRun = false;
            //KeyStatus._Instance._KeyStatu = KeyStatu.GetSliderValue;
        }
    }

    void MoveSlider()
    {
        //zeroTime = Time.time;
        _slider.value = Mathf.PingPong(localTime, 1);
        sliderValue = _slider.value * 4;
        //Debug.Log(_slider.value);
        localTime += Time.deltaTime;
        if (localTime >= 2)
        {
            canStop = true;
        }
    }
}
