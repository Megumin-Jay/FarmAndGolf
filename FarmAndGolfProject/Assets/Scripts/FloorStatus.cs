using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorStatus : MonoBehaviour
{
    public FloorStatu _floorStatu = FloorStatu.Area3;
    private static FloorStatus _instance;

    //当前地形
    public Floor _currentFloor;
    //显示当前地形的UI
    public Text Text;
    
//    public FloorStatu _FloorStatu
//    {
//        get
//        {
//            return _floorStatu;
//            
//        }
//        set
//        {
//            _floorStatu = value; 
//            
//        }
//    }
    
    public static FloorStatus _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(FloorStatus)) as FloorStatus;
            } 
            return _instance;
        }
    }

    void Start()
    {
        //_floorStatu = FloorStatu.Area3;
    }
    
    void Update()
    {
        Debug.Log(_floorStatu);
        switch (_floorStatu)
        {
            case FloorStatu.Area1:
                //_currentFloor = new 
                Text.text = "当前所在区域：推杆区域";
                break;
            case FloorStatu.Area2:
                //_currentFloor = new 
                Text.text = "当前所在区域：浅色区域";
                break;
            case FloorStatu.Area3:
                //_currentFloor = new 
                Text.text = "当前所在区域：深色区域";
                break;   
            case FloorStatu.Area4:
                //_currentFloor = new 
                Text.text = "当前所在区域：4";
                break;
        }
    }
}

public enum FloorStatu
{
    Area1,
    Area2,
    Area3,
    Area4
}
