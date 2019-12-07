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
        if (Input.GetKeyDown(KeyCode.Space) && !canRun && !canStop)
        {
            canRun = true;
            localTime = 0;
        }

        if (canRun)
        {
            MoveSlider();
            //isRunning = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canStop)
        {
            canRun = false;
            canStop = false;
        }
    }

    void MoveSlider()
    {
        //zeroTime = Time.time;
        _slider.value = Mathf.PingPong(localTime, 1);
        
        localTime += Time.deltaTime;
        if (localTime >= 2)
            canStop = true;
    }
}
