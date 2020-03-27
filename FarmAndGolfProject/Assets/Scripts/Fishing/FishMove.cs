using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class FishMove : MonoBehaviour
{
    public GameObject player;//为了获取玩家碰撞体的位置
    private Vector3 originalSize;//原始"尺寸"
    public GameObject path;//总轨迹设计参考点
    private Vector3 fix = new Vector3(0, -0.7f, 0);//修正参考点位置的向量
    public Transform[] PointList;//生成路径的保存的目标点
    private Transform[] lurePointList;//上面那个的逆序



    void Start()
    {
        gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");//获取玩家位置
        originalSize = gameObject.transform.localScale;
        lurePointList = new Transform[PointList.Length];
        for (int i = 0; i < PointList.Length; i++)
        {
            lurePointList[i] = PointList[PointList.Length - 1 - i];
        }
    }

    //依靠DoTween插件实现"鱼"按既定目标点曲线运动
    public void AutoMove()
    {
        gameObject.transform.position = PointList[0].position;//物品位置更新为第一个目标点(保证相对玩家的轨迹正常)
        gameObject.SetActive(true);//激活
        path.transform.position = player.transform.position + fix;//加入修正向量,使总轨迹设计参考点与玩家下盘位置一致
        var positions = PointList.Select(u => u.position).ToArray();
        transform.DOPath(positions, 1, PathType.CatmullRom, 0, 10);//(目标点, 运动总耗时, 运动方式:曲线, Lookat, 轨迹精度)
    }
    //鱼 到达位置
    public void fishReached()
    {
        if (gameObject.transform.position == PointList[PointList.Length - 1].position)
            gameObject.SetActive(false);
    }




    //鱼饵 的自动移动,就是鱼的逆运动
    public void lureAutoMove()
    {
        gameObject.transform.position = lurePointList[0].position;//物品位置更新为第一个目标点(保证相对玩家的轨迹正常)
        gameObject.SetActive(true);//激活
        path.transform.position = player.transform.position + fix;//加入修正向量,使总轨迹设计参考点与玩家下盘位置一致
        var positions = lurePointList.Select(u => u.position).ToArray();
        transform.DOPath(positions, 1, PathType.CatmullRom, 0, 10);//(目标点, 运动总耗时, 运动方式:曲线, Lookat, 轨迹精度)
    }
    //鱼饵 到达位置
    public void lureReached()
    {
        if (gameObject.transform.position == lurePointList[PointList.Length - 1].position)
            gameObject.SetActive(false);
    }

    public void updateFishImage(Sprite sp)//更新"鱼"的图片
    {
        gameObject.transform.GetComponent<SpriteRenderer>().sprite = sp; //替换图片为随机到的"鱼"
        float w = sp.bounds.size.x;
        gameObject.transform.localScale = originalSize / w;//确保sprite的大小比较正常
    }

}
