using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//通过3个摄像机实现农场的预览和切换
public class Farm : MonoBehaviour
{
    public bool farmIsOn = false;//判断是否打开农场

    public Player player;//人物身上的脚本
    public Animator animator;//动画器
    public TipsUI tips;//提示框

    public GameObject mainCamera;//主摄
    public GameObject extraCamera;//投影农场的摄像机
    public GameObject farmCamera;//农场摄像机

    public GameObject farmBG;//农场背景
    public GameObject farmTips;//农场提示
    public GameObject farmOPTips;//农场操作提示
    public GameObject farmUI;//农场UI

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<Player>();//获取角色的脚本
            animator = other.GetComponent<Animator>();//获取角色挂载的动画器
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //按下互动键(暂设为K),切换到农场界面
        if (Input.GetKeyDown(KeyCode.K))
        {
            farmIsOn = true;
        }

        if (!farmIsOn)
        {
            tips.Show();
            tips.UpdateTooltip("按下\"K\"键前往农场");
        }

        if (farmIsOn)
        {
            tips.Hide();//隐藏提示栏
            ToFarm();//切换到农场
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            tips.Hide();//隐藏提示栏
        }
    }

    //要实现:禁止玩家移动 关闭主摄和投影摄像机 激活农场摄像机 
    private void ToFarm()
    {
        player.StopMove();//剥夺移动控制权
        mainCamera.SetActive(false);
        extraCamera.SetActive(false);
        farmCamera.SetActive(true);
        farmBG.SetActive(true);
        farmTips.SetActive(true);
        farmOPTips.SetActive(true);
        farmUI.SetActive(true);
    }

    //要实现:关闭农场摄像机 激活主摄和投影摄像机 恢复玩家移动
    public void Back()
    {
        farmIsOn = false;
        player.moveIsOn = true;//恢复移动控制权
        farmCamera.SetActive(false);
        mainCamera.SetActive(true);
        extraCamera.SetActive(true);
        farmBG.SetActive(false);
        farmTips.SetActive(false);
        farmOPTips.SetActive(false);
        farmUI.SetActive(false);
    }
}


