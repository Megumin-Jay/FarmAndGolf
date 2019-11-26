﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ground
{
    protected string groundType;  //地皮的类型，例如“lawn”、“snowfield”
    protected float groundFrictionFactor;  //不同地皮摩擦力影响因子不同
    protected float groundJumpFactor;  //不同地皮弹跳影响因子不同
    private Sprite groundSp;  //地皮贴图
    protected Collider cd;  //碰撞器
    
    public float GroundJumpFactor  //地皮弹跳因子
    { get { return groundJumpFactor; } }
    public float GroundFrictionFactor  //地皮摩擦力因子
    { get { return groundFrictionFactor; } }
    public string GroundType  //地皮类型
    { get { return groundType; } }
    public Sprite GroundSprite  //地皮贴图
    {
        set { groundSp = value; }
    }
    public Collider collider  //碰撞器
    {
        set { cd = value; }
        get { return cd; }
    }
    //地面与球接触后的特殊效果，如球入洞，球入水后的波纹，球在雪地上砸出坑，滚出痕迹等
    public abstract void GroundEffect();
}


/// Environment
/*地面弹力*/
//public float groundBounceY;

//public float groundBounceX;
/*地面摩擦力*/
//public float groundFriction;
//public float windForce;
/*空气阻力*/
public Vector3 airForceDirection;
/*风力(跟球有关)*/
public Vector3 windDirection;