using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    //钓鱼点相对检测区域的方向
    public float H;
    public float V;

    //计时器,控制播放动画时间
    public float timer;

    private Vector3 position = new Vector3(0, 0, 1000);//用于判断人物是否静止的位置变量

    private bool fishIsOn = false;//判断是否启动钓鱼功能


    public Player player;//人物身上的脚本
    public Animator animator;//动画器


    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Protagonist")//检测碰撞物体是否为主角
        {
            player = collision.GetComponent<Player>();//获取角色的脚本
            animator = collision.GetComponent<Animator>();//获取角色挂载的动画器
            timer = 1.0f;//计时器赋初值,作为整个钓鱼动画播放时长
        }
    }

    //整个钓鱼过程中人物都应处在碰撞器中
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Protagonist")//检测碰撞物体是否为主角
        {
            Test();
            //按下互动键(暂设为K),开启钓鱼功能
            if (Input.GetKeyDown(KeyCode.K))
                fishIsOn = true;
            if (fishIsOn)
            {
                player.StopMove();//强制播放自动行走,剥夺玩家控制权
                player.AutoMove(H, V);
                //对比两帧物体位置是否改变,若不变 说明人物已到达目标位置,停止播放行走动画
                if (collision.transform.position == position)
                {
                    animator.SetFloat("Magnitude", 0);
                    FishAnimator();//播放钓鱼动画
                }
                position = collision.transform.position;//保存人物上一帧的位置
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Protagonist")//检测碰撞物体是否为主角
        {

        }
    }

    //播放钓鱼动画
    private void FishAnimator()
    {
        animator.SetBool("Fishing", true);//开始播放钓鱼动画
        animator.SetFloat("Horizontal", H);
        animator.SetFloat("Vertical", V);
        timer -= Time.deltaTime;
        if (timer < 0.1f)//钓鱼动画播放完毕,准备结算
        {
            timer = 1.0f;
            animator.SetBool("Fishing", false);
            player.moveIsOn = true;
            fishIsOn = false;
        }
    }

    //开发用 , J键强制脱出钓鱼状态
    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            player.moveIsOn = true;
            fishIsOn = false;
        }
    }
}