using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    //属性
    private float moveSpeed = 4;//角色移动的初始速度
    static float v = -1, h = 0;//记录上一帧的移动指令
    //引用
    public Animator animator;//动画器
    public bool moveIsOn = true;//判断是否为可移动状态

    public GameObject myBag;//获取我的背包
    public GameObject mySetting;//获取设置界面
    public GameSaveManager gameSaveManager;//数据存储器
    public HeldManager myHeld;//我的物品持有
    public Inventory AllItems;//全物品,用以计数
    public Inventory playerInventory;//取得玩家的背包
    public static Vector3 initialPosition = new Vector3(0, 1, 0);//场景切换后加载的初始位置



    void Awake()
    {
        gameObject.transform.position = initialPosition;//传送到切换场景后逻辑上应该在的位置


    }

    void Start()
    {
        gameSaveManager.LoadGame();//加载游戏数据
        myHeld.Load();//刷新物品的持有数
        GetBall();
    }

    void Update()
    {
        OpenUI();
    }
    void FixedUpdate()
    {
        Move();
    }

    //人物的常规移动方法
    public void Move()
    {
        if (moveIsOn)
        {
            //八向移动，结合混合树制作的动画
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);//移动矢量长度，用于判断是否移动
            transform.position = transform.position + movement * Time.fixedDeltaTime * moveSpeed;

            //控制静止时人物朝向
            if (movement.magnitude == 0)
            {
                animator.SetFloat("Horizontal", h);
                animator.SetFloat("Vertical", v);
            }
            else
            {
                v = movement.y;
                h = movement.x;
            }

            //按下"Shift"时“跑步”（移动速度增加）
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = 6;
                animator.speed = 1.3f;
            }
            else
            {
                moveSpeed = 4;
                animator.speed = 1.0f;
            }
        }
    }

    //停止移动方法
    public void StopMove()
    {
        moveIsOn = false;
        animator.SetFloat("Magnitude", 0);
    }

    //继续移动方法
    public void KeepMove()
    {
        moveIsOn = true;
    }

    //直线的自动移动
    public void AutoMove(float H, float V)
    {
        h = H;
        v = V;//确保退出自动行走后人物朝向不矛盾
        Vector3 movement = new Vector3(H, V, 0.0f);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);
        transform.position = transform.position + movement * Time.fixedDeltaTime * moveSpeed;
    }

    void OpenUI()
    {
        //用I键开关背包
        if (Input.GetKeyDown(KeyCode.I))
        {
            myBag.SetActive(!myBag.activeSelf);
            InventoryManager.RefreshItem();//打开时就刷新一次背包
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mySetting.SetActive(true);
        }
    }

    public void GetBall()
    {
        int a = 22;
        for (a = 22; a < 27; a++)
        {
            if (!playerInventory.itemList.Contains(AllItems.itemList[a]))//如果背包中没这个
            {
                for (int i = 0; i < playerInventory.itemList.Count; i++)
                {
                    if (playerInventory.itemList[i] == null)//找空格子,有就给他的物品赋值
                    {
                        playerInventory.itemList[i] = AllItems.itemList[a];//赋值
                        AllItems.itemList[a].itemHeld = 1;
                        break;
                    }
                }
            }
            InventoryManager.RefreshItem();
        }
    }

}