using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于检测玩家是否到达钓鱼地点
public class FishingTest : MonoBehaviour
{
    public Fishing fishing;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
            fishing.playerReached = true;
    }
}
