using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannotPass : MonoBehaviour
{
    public TipsUI tips;//提示框
    public string tip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            tips.Show();
            tips.UpdateTooltip(tip);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            tips.Show();
            tips.UpdateTooltip(tip);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            tips.Hide();//隐藏提示栏
        }
    }
}
