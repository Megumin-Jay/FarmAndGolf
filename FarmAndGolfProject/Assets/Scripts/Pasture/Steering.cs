using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 所有操控行为的基类
public class Steering : MonoBehaviour
{
    public float weight = 1f;           //操控力的权重

    //计算操控力，由派生类重写
    public virtual Vector3 Force(Vehicle me)
    {
        return Vector3.zero;
    }
}
