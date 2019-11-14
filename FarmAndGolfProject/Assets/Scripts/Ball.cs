using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ball
{
    protected float quality;  //球的质量
    protected string type;    //球的类型，例如："apple"、"orange"之类的
    protected float jumpfactor;   //弹跳的影响因子，比如：其他数据相同、落下场地相同时，葡萄比苹果弹得高
    protected float frictionfactor;   //摩擦力的影响因子，影响球滑多远
    protected int value;  //不同类型的球，得分可能不一样
    private Sprite sp;
    protected Rigidbody rgbd;
    //protected BallMove bm;    
    //public abstract void Move();
    public float Quality
    { get { return quality; } }
    public float Jumpfactor
    { get { return jumpfactor; } }
    public float Frictionfactor
    { get { return frictionfactor; } }
    public int Value
    { get { return value; } }
    public string Type
    { get { return type; } }
    public Sprite sprite
    {
        set { sp = value; }
    }
    public Rigidbody Rgbd
    {
        set { rgbd = value; }
        get { return rgbd; }
    }
    public abstract void SpecialEffect();    //球的特殊效果，例如：河豚会游泳
}
