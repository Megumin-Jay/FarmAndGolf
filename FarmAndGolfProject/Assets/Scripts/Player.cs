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
    public static Vector3 initialPosition = new Vector3(0, 1, 0);//场景切换后加载的初始位置
    void Start()
    {

    }
    void Awake()
    {
        gameObject.transform.position = initialPosition;//传送到切换场景后逻辑上应该在的位置
    }

    void Update()
    {
        OpenMyBag();
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

            //按下"L"时“跑步”（移动速度增加）
            if (Input.GetKey(KeyCode.L))
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
    public void AutoMove(float h, float v)
    {
        Vector3 movement = new Vector3(h, v, 0.0f);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);
        transform.position = transform.position + movement * Time.fixedDeltaTime * moveSpeed;
    }

    void OpenMyBag()//用I键开关背包
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            myBag.SetActive(!myBag.activeSelf);
            InventoryManager.RefreshItem();//打开时就刷新一次背包
        }
    }
}