using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ball
{
    [SerializeField] protected float ballMass;  //球的质量
    [SerializeField] protected string type;    //球的类型，例如："apple"、"orange"之类的
    [SerializeField] protected float liftForceRatio=0.1f;   //球的升力
    [SerializeField] protected float windRatio=1.0f;   //球的风力系数
    [SerializeField] protected int value;  //不同类型的球，得分可能不一样
    protected Vector3 moveSpeed = Vector3.zero;  //球运动的速度，初始值为0
    private Sprite sp;   //球的贴图
    public Rigidbody rgbd;
    public float radius = 0f;  //球旋转的角度，弧度制
    public float angle = 0f;   //球旋转的角度，角度制
    public bool canStop = false;   //球停止运动

    public float BallMass
    { get { return ballMass; } }
    public float LiftForceRatio
    { get { return liftForceRatio; } }
    public float WindRatio
    { get { return windRatio; } }
    public int Value
    { get { return value; } }
    public string Type
    { get { return type; } }
    public float SpeedValue   //得到速度的大小
    { get { return moveSpeed.magnitude; } }
    public Vector3 MoveSpeed
    {
        set { moveSpeed = value; }
    }
    public Sprite sprite
    {
        set { sp = value; }
    }

    public virtual void SpecialMove()    //球的特殊移动，例如：河豚会游泳
    {}
}
