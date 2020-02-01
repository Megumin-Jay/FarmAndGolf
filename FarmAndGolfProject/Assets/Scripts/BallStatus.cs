using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStatus : MonoBehaviour
{
    private static BallStatus _instance;

    //当前球名和球
    public string ballName;
    public Floor _currentFloor;

    public static BallStatus _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BallStatus)) as BallStatus;
            } 
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentFloor = null;
        ballName = null;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ballName);
//        to do 
//        if()
//        { 
//            Type ballType = Type.GetType(ballName);
//            if (ballType != null)
//            {
//                _currentFloor = Activator.CreateInstance(ballType) as Floor;
//            }
//        }
    }
}

