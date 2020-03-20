using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class FishMove : MonoBehaviour
{
    public GameObject playerCollision;//为了获取玩家碰撞体的位置
    public GameObject path;


    private Vector3 originalSize;//原始"尺寸"

    public Transform[] PointList;//生成路径的保存的目标点



    void Start()
    {
        gameObject.SetActive(false);
    }


    public void AutoMove()//依靠DoTween插件实现"鱼"按既定目标点曲线运动
    {
        gameObject.transform.position = PointList[0].position;//物品位置更新为第一个目标点(保证相对玩家的轨迹正常)
        gameObject.SetActive(true);//激活
        path.transform.position = playerCollision.transform.position;//总轨迹设计参考点与玩家碰撞体位置一致
        var positions = PointList.Select(u => u.position).ToArray();
        transform.DOPath(positions, 1, PathType.CatmullRom, 0, 10);//(目标点, 运动总耗时, 运动方式:曲线, Lookat, 轨迹精度)
    }

    void Awake()
    {
        playerCollision = GameObject.FindGameObjectWithTag("PlayerCollision");//获取玩家碰撞体位置

        originalSize = gameObject.transform.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)//Enter不行...DoTween播放过程中忽略碰撞了
    {
        if (other.gameObject.CompareTag("Player"))//如果是主角在碰撞
        {
            gameObject.SetActive(false);
        }
    }


    public void updateFishImage(Sprite sp)//更新"鱼"的图片
    {
        gameObject.transform.GetComponent<SpriteRenderer>().sprite = sp; //替换图片为随机到的"鱼"
        float w = sp.bounds.size.x;
        gameObject.transform.localScale = originalSize / w;//确保sprite的大小比较正常
    }

}
