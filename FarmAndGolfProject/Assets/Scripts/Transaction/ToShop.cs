using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToShop : MonoBehaviour
{
    private bool shopIsOn = false;//判断是否已打开商店
    public TipsUI tips;//提示框
    public storeInventoryManager siy;//不要再滥用static方法了!!
    public Inventory store;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            siy.GotPlayer(other.GetComponent<Player>());
        }
    }

    //测试用, 实装会接在对话后而非碰撞触发
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            if (Input.GetKeyDown(KeyCode.K) && shopIsOn == false)
            {
                shopIsOn = true;
                siy.OpenSellStore();

            }

            if (!shopIsOn)
            {
                tips.Show();//显示提示栏
                other.GetComponent<Player>().KeepMove();
                tips.UpdateTooltip("按下\"K\"键打开商店面板");
            }

            if (shopIsOn)
            {
                other.GetComponent<Player>().StopMove();//强制播放自动行走,剥夺玩家控制权
                tips.Hide();//隐藏提示栏
            }
            if (siy.storePanel.activeSelf == false)
            {
                shopIsOn = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            shopIsOn = false;
            tips.Hide();//隐藏提示栏
        }
    }

}
