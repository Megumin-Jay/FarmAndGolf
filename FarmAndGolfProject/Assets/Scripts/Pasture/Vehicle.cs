using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public float maxForceValue = 100f;          //能施加到AI角色的最大操控力数值
    public float maxSpeed = 10f;                //AI角色的最大速度 
    public float mass = 1;                      //AI角色的质量
    public Vector3 velocity;                    //计算得到AI角色的速度
    public float computerInterval = 0.2f;       //计算操控力间隔时间（为了效率，不需每帧都执行）
    public bool isPlanar = true;                //是否在平面上移动
    public int state = 1;             //生物的行为状态，1：随即徘徊；2：睡觉；3：喝水；4：吃饭
    public AnimalGrowth my_animal;

    [SerializeField] float energyspeed = 5f; //精力值消耗速度，单位为s
    [SerializeField] float energy = 100;        //精力值，为0的时候，动物会睡觉
    public float Energy { set { energy = value; } get { return energy; } }
    protected float _timer;                     //计时器
    public float Timer { set { _timer = value; } get { return _timer; } }
    protected Vector3 _steeringForce;           //计算得到的操控力合力
    protected Vector3 _acceleration;            //计算得到AI角色加速度    
    protected float _sqrMaxSpeed;               //AI角色最大速度的平方
    public Steering[] _steerings;            //AI角色包含的所有操控行为列表

    void Start()
    {
        _timer = 0;
        _steeringForce = Vector3.zero;
        _sqrMaxSpeed = maxSpeed * maxSpeed;
        velocity = Vector3.zero;
        _steerings = GetComponents<Steering>();
    }

    void Update()
    {

        _timer += Time.deltaTime;

        _steeringForce = Vector3.zero;

        if (state != 2)//不是在睡觉的时候，减少精力值
            energy -= energyspeed * Time.deltaTime;
        else
            energy += energyspeed * Time.deltaTime * 2;
        //如果距离上一次计算操控力时间间隔超过设定值，重新计算操控力
        if (_timer > computerInterval)
        {
            _steeringForce = _steerings[state-1].Force(this);
            //限制操控力不大于AI角色所能承受的最大操控力
            _steeringForce = Vector3.ClampMagnitude(_steeringForce, maxForceValue);

            //计算AI角色的加速度
            _acceleration = _steeringForce / mass;

            //重置计时器
            _timer = 0;
        }
    }
}
