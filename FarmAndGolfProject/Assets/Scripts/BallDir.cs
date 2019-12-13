using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDir : MonoBehaviour
{
    #region private
    /*射线检测*/
    //private RaycastHit _hit;
    /*鼠标所点击的点 即击球终点*/
    private Vector3 pos;
    /*起点终点方向虚线uv的移动速度*/
    private float speed; 
    /*击球次数*/
    private int hitTimes;
    /*是否击球*/
    private bool isHit;
    /*真球*/
    private GameObject ball;
    /*摄像机*/
    private GameObject camera;
    /*摄像机初始位置*/
    private Vector3 cameraFirstPos;
    /*是否复位*/
    private bool isReset;
    /*复位速度*/
    private float resetSpeed;
    /*初始击球方向(鼠标选择)*/
    private Vector3 hitDirection;
    #endregion
    
    #region public
    /*鼠标点击方向*/
    public Vector3 Pos
    {
        get => pos;
        set => pos = value;
    }
    /*击球次数*/
    public int HitTimes
    {
        get => hitTimes;
        set => hitTimes = value;
    }

    public bool IsHit
    {
        get => isHit;
        set => isHit = value;
    }

    /*画线组件*/
    public LineRenderer lineRenderer;
    /*起点终点方向虚线所用的材质*/
    public Material material;
    /*要实例化的球*/
    public GameObject ballObj;

    //public delegate void myDelegate(int _hitTimes, Vector3 pos);

    //public myDelegate publisher; 
    #endregion
    
    public static BallDir Instance
    {
        private set;
        get;
    }

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance != null)
        {
            Instance = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始化起点终点方向虚线uv的移动速度
        speed = 0.05f;

        //初始化击打次数
        hitTimes = 0;
        
        //初始化击打状态
        isHit = false;

        //记录相机初始位置
        camera = GameObject.FindWithTag("MainCamera");
        cameraFirstPos = camera.transform.position;

        isReset = false;

        resetSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * speed;
        //让黄色(起点终点)虚线动起来
        material.mainTextureOffset = new Vector2(offset, 0);

        //获取水平输入
        float horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);
        
        //选终点
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;
            bool isHit = Physics.Raycast(ray, out _hit);
            if (isHit)
            {
                pos = _hit.point;
                //Debug.Log(pos);
                //选终点时启用虚线
                lineRenderer.enabled = true;
            }
        }
        //选方向
        if (horizontal != 0)
        {
            if (ball)
            {
                hitDirection = pos - ball.transform.position;
            }
            else
            {
                hitDirection = pos - new Vector3(0, 1, -1.15f);
            }

//            if (Mathf.Acos(pos.x / hitDirection.magnitude) == Mathf.Asin(pos.y / hitDirection.magnitude))
//            {
                Debug.Log("x" +
                          (Mathf.Acos(Mathf.Clamp(pos.x / hitDirection.magnitude, -1, 1)) - horizontal * Time.deltaTime));
                Debug.Log("y" +
                          (Mathf.Asin(Mathf.Clamp(pos.y / hitDirection.magnitude, -1, 1)) - horizontal * Time.deltaTime));
                pos.x = hitDirection.magnitude *
                        Mathf.Cos(Mathf.Acos(Mathf.Clamp(pos.x / hitDirection.magnitude, -1, 1)) - horizontal * Time.deltaTime);
                if (pos.x <= 0)
                {
                    pos.y = hitDirection.magnitude *
                            Mathf.Sin(Mathf.PI - Mathf.Asin(Mathf.Clamp(pos.y / hitDirection.magnitude, -1, 1)) - horizontal * Time.deltaTime);
                }
                else if (pos.x > 0)
                {
                    pos.y = hitDirection.magnitude *
                            Mathf.Sin(Mathf.Asin(Mathf.Clamp(pos.y / hitDirection.magnitude, -1, 1)) - horizontal * Time.deltaTime); 
                }
//            }
        }
        
        //绘制起点终点方向虚线
        if(ball)
            lineRenderer.SetPosition(0, new Vector3(ball.transform.position.x, ball.transform.position.y, -1.15f));
        else 
            lineRenderer.SetPosition(0, new Vector3(0, 1, -1.15f));
        lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y,-1.15f));
        
        //打球
        if(Input.GetKeyDown(KeyCode.Z))
        {
            isReset = false;
            //击球次数加1
            hitTimes++;
            //将球打出后关闭虚线
            lineRenderer.enabled = false;
            //实例化球
            if (hitTimes <= 1)
            {
                ball = Instantiate(ballObj, new Vector3(0, 0, -1.15f), Quaternion.identity);
            }
            else
            {
                isHit = true;
            }
            //Debug.Log(publisher);
            //publisher(hitTimes, pos);
        }
        //复位
        if (Input.GetKey(KeyCode.R))
        {
            //击球次数归零
            hitTimes = 0;
            
            isReset = true;
        }

        if (isReset)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, cameraFirstPos, resetSpeed * Time.deltaTime); 
        }
    }
}
