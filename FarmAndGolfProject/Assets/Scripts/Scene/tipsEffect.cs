using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class tipsEffect : MonoBehaviour
{
    public Transform[] PointList;//生成路径的保存的目标点

    void OnEnable()
    {
        Drop();
    }

    private void Drop()
    {
        gameObject.transform.position = PointList[0].position;//物品位置更新为第一个目标点(保证相对玩家的轨迹正常)
        var positions = PointList.Select(u => u.position).ToArray();
        transform.DOPath(positions, 0.3f, PathType.CatmullRom, 0, 10);//(目标点, 运动总耗时, 运动方式:曲线, Lookat, 轨迹精度)
    }
}
