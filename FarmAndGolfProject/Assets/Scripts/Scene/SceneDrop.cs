using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class SceneDrop : MonoBehaviour
{
    public Transform[] PointList;//生成路径的保存的目标点

    void Start()
    {
        Drop();
    }
    private void Drop()
    {
        var positions = PointList.Select(u => u.position).ToArray();
        transform.DOPath(positions, 1, PathType.CatmullRom, 0, 10);//(目标点, 运动总耗时, 运动方式:曲线, Lookat, 轨迹精度)
    }
}
