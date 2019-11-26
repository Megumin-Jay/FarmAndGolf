using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Floor
{
    protected string groundType;  //地皮的类型，例如“lawn”、“snowfield”
    protected float groundFrictionFactor;  //不同地皮摩擦力影响因子不同
    protected float groundBounceFactor;  //不同地皮弹力影响因子不同
    private Sprite groundSp;  //地皮贴图
    protected Collider cd;  //碰撞器

    public float GroundBounceFactor  //地皮弹力因子
    { get { return groundBounceFactor; } }
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
