using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Environment
{
    protected float windPower;  //环境风力
    protected Vector3 windPowerDirection;  //环境风向
    private Sprite windSp;  //风向标图标
    protected float windSpAngle;  //风向标旋转角度

    public float WindPower  //环境风力
    { get { return windPower; } }
    public Vector3 WindPowerDirection  //环境风向
    { get { return windPowerDirection; } }
    public Sprite WindSprite  //风向标图标
    {
        set { windSp = value; }
    }
    public float WindSpriteAngle  //风向标旋转角度
    { get { return windSpAngle; } }
    public abstract void EnvironmentEffect();  //环境特效，如飘雪特效，蒲公英漂浮特效
}
