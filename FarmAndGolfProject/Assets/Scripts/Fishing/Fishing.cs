using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public float H;
    public float V;//钓鱼点相对检测区域的方向
    public float timer;//计时器,控制播放动画时间
    private Vector3 position = new Vector3(0, 0, 1000);//用于判断人物是否静止的位置变量
    private bool fishIsOn = false;//判断是否启动钓鱼功能
    private int seed;


    public FishItems fishItems;//取得"鱼池"
    public Inventory playerInventory;//取得玩家的背包
    public TipsUI tips;//提示框
    public TipsUI popUps;//弹窗
    public FishMove fish;//"动画"里的鱼

    public Player player;//人物身上的脚本
    public Animator animator;//动画器

    //测试
    private bool condition = false;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")//检测碰撞物体是否为主角
        {
            player = collision.GetComponent<Player>();//获取角色的脚本
            animator = collision.GetComponent<Animator>();//获取角色挂载的动画器
            timer = 0.9f;//计时器赋初值,作为整个钓鱼动画播放时长

        }
    }

    //整个钓鱼过程中人物都应处在碰撞器中
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")//检测碰撞物体是否为主角
        {
            Test();
            //按下互动键(暂设为K),开启钓鱼功能
            if (Input.GetKeyDown(KeyCode.K))
            {
                fishIsOn = true;
                seed = Random.Range(0, 19);
            }


            if (!fishIsOn)
            {
                tips.Show();//显示提示栏
                tips.UpdateTooltip("按下\"K\"键开始钓鱼");
            }

            if (fishIsOn)
            {
                tips.Hide();//隐藏提示栏
                player.StopMove();//强制播放自动行走,剥夺玩家控制权
                player.AutoMove(H, V);
                //对比两帧物体位置是否改变,若不变 说明人物已到达目标位置,停止播放行走动画
                if (collision.transform.position == position)
                {
                    animator.SetFloat("Magnitude", 0);
                    FishEnd();//播放一轮钓鱼的动画并结算
                }
            }
            position = collision.transform.position;//保存人物上一帧的位置
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")//检测碰撞物体是否为主角
        {
            tips.Hide();//隐藏提示栏
        }
    }

    //播放钓鱼动画并结算
    private void FishEnd()
    {
        animator.SetBool("Fishing", true);//开始播放钓鱼动画
        animator.SetFloat("Horizontal", H);
        animator.SetFloat("Vertical", V);

        timer -= Time.deltaTime;
        fish.updateFishImage(fishItems.FishItemList[seed].itemImage);

        if (!condition)
        {
            fish.AutoMove();
            condition = true;
        }

        if (timer < 0.1f)//钓鱼动画播放完毕,准备结算
        {
            timer = 0.9f;
            animator.SetBool("Fishing", false);
            player.moveIsOn = true;
            fishIsOn = false;
            condition = false;

            GetFish(seed);
            popUps.UpdateTooltip("恭喜您获得\"" + fishItems.FishItemList[seed].name + "\"!");
            popUps.Show();


        }
    }

    //生成物品至背包
    public void GetFish(int a)
    {
        if (!playerInventory.itemList.Contains(fishItems.FishItemList[a]))//如果背包中没这个
        {
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)//找空格子,有就给他的物品赋值
                {
                    playerInventory.itemList[i] = fishItems.FishItemList[a];//赋值
                    break;
                }
            }
        }
        else
        {
            fishItems.FishItemList[a].itemHeld += 1;//如果原本就有,则增加"这个物品"的数量 
        }

        InventoryManager.RefreshItem();
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
