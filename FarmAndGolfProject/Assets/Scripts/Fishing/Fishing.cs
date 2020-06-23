using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    public float H;
    public float V;//钓鱼点相对检测区域的方向
    private float timer;//计时器,控制播放动画时间
    private bool fishIsOn = false;//判断是否启动钓鱼功能
    public bool playerReached = false;//检测玩家是否到达钓鱼地点
    private int seed;//随机数,钓鱼的"种子"
    private int[] weight;//物品权重表
    private int weightTotal;//物品总权重
    private bool[] condition = new bool[2] { true, true };//确保有些方法在Update中只执行一次的布尔变量


    public Inventory fishItems;//取得"鱼池"
    public Inventory playerInventory;//取得玩家的背包
    public TipsUI tips;//提示框
    public TipsUI popUps;//弹窗
    public FishMove fish;//"动画"里的鱼

    public Player player;//人物身上的脚本
    public Animator animator;//动画器
    private Sprite lureSp;//鱼饵图片
    private Sprite fishSp;//鱼的图片



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            player = other.GetComponent<Player>();//获取角色的脚本
            animator = other.GetComponent<Animator>();//获取角色挂载的动画器

            //根据价格构建概率池
            weight = new int[fishItems.itemList.Count];
            weightTotal = 0;
            for (int i = 0; i < fishItems.itemList.Count; i++)
            {
                weight[i] = 10000 / fishItems.itemList[i].price;//价格越高权重越低
                weightTotal += weight[i];
            }
        }
    }

    //整个钓鱼过程中人物都应处在碰撞器中
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
        {
            //按下互动键(暂设为K),开启钓鱼功能
            if (Input.GetKeyDown(KeyCode.K) && fishIsOn == false)
            {
                fishIsOn = true;
                seed = GetSeed();
                fishSp = fishItems.itemList[seed].itemImage;//鱼的图片
                lureSp = Resources.Load<Sprite>("Graphics/Function/Inventory/FishItems/lure");//鱼饵的图片
                timer = 6.0f;//计时器赋初值,作为整个钓鱼动画播放时长
                playerReached = false;
            }


            if (!fishIsOn)
            {
                tips.Show();//显示提示栏
                tips.UpdateTooltip("按下\"K\"键,使用鱼饵开始钓鱼");
            }

            if (fishIsOn)
            {
                if (playerReached)//到达钓鱼地点
                {
                    animator.SetFloat("Magnitude", 0);
                    FishEnd();
                }
                else
                {
                    tips.UpdateTooltip("正在前往钓鱼地点");
                    player.StopMove();//强制播放自动行走,剥夺玩家控制权
                    player.AutoMove(H, V);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")//检测碰撞物体是否为主角
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

        if (timer > 4.5f)
            tips.UpdateTooltip("抛出鱼饵中...");
        else if (timer > 1.5f)
            tips.UpdateTooltip("等待中...");
        else
            tips.UpdateTooltip("收杆中...");

        if (timer < 5.6f)
            if (condition[0])
            {
                //替换为鱼饵的图片
                fish.updateFishImage(lureSp);
                fish.lureAutoMove();
                condition[0] = false;
            }

        if (timer < 1.1f)
            if (condition[1])
            {
                //替换为钓上来的物品
                fish.updateFishImage(fishSp);
                fish.AutoMove();
                condition[1] = false;
            }

        if (condition[1])//以 是否执行鱼的移动 为中介点,分别执行鱼和饵的位置判断
            fish.lureReached();
        else
            fish.fishReached();

        if (timer < -0.1f)//钓鱼动画播放完毕,准备结算
        {
            animator.SetBool("Fishing", false);//退出钓鱼动画
            player.moveIsOn = true;//恢复移动控制权
            fishIsOn = false;//退出钓鱼
            condition = new bool[2] { true, true };//重置单次函数的判断数组
            GetFish(seed);//将钓上的物品加入背包

            //消耗鱼饵
            player.transaction.Sell(player.AllItems.itemList[player.transaction.FindItem("鱼饵")], 1, 0, false);

            popUps.UpdateTooltip("恭喜您获得\"" + fishItems.itemList[seed].itemName + "\"!");
            popUps.Show();//显示"确认"弹窗
        }
    }

    //生成物品至背包
    public void GetFish(int a)
    {
        if (!playerInventory.itemList.Contains(fishItems.itemList[a]))//如果背包中没这个
        {
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)//找空格子,有就给他的物品赋值
                {
                    playerInventory.itemList[i] = fishItems.itemList[a];//赋值
                    fishItems.itemList[a].itemHeld = 1;
                    break;
                }
            }
        }

        else
        {
            fishItems.itemList[a].itemHeld += 1;//如果原本就有,则增加"这个物品"的数量 
        }

        InventoryManager.RefreshItem();
    }

    private int GetSeed()
    {
        int rand = Random.Range(1, weightTotal);
        int tmp = 0;
        for (int i = 0; i < weight.Length; i++)
        {
            tmp += weight[i];
            if (rand <= tmp)
            {
                return i;
            }
        }
        return 0;
    }
}
