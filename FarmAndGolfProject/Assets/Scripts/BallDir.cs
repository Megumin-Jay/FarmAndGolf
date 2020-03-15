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
    /*存储鼠标点击的点*/
    private Vector3 invariantPos;
    /*起点终点方向虚线uv的移动速度*/
    private float speed; 
    /*击球次数*/
    private int hitTimes;
    /*是否击球*/
    private bool isHit;
    /*真球*/
    private GameObject ball;
    /*是否复位*/
    private bool isReset;
    /*复位速度*/
    private float resetSpeed;
    /*初始击球方向(鼠标选择)*/
    private Vector3 hitDirection;
    /*是否选择终点*/
    public bool isCheck;
    /*键盘响应*/
    private float horizontal;
    
    /*人和球的向量*/
    private Vector3 playerBalloffset;
    /*球的初始位置*/
    private Vector3 ballInitialPos;
    
    /// /*摄像机*/
    private GameObject camera;
    /*摄像机初始位置*/
    private Vector3 cameraFirstPos;
    /*相机与球的偏移*/
    private Vector3 offset;
    /*相机移动速度*/
    private float cameraSpeed;
    /*摄像机移动的边界值*/
    private float cameraPosMaxX;
    private float cameraPosMaxY;
    private float cameraPosMinX;
    private float cameraPosMinY;

    /// /*主人公*/
    private GameObject player;
    /*主人公上一球的位置*/
    private Vector3 lastPlayerPos;

    /// /*高尔夫杆*/
    public float length;

    private Animator _animator;
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
        
        //初始化起始pos
        //pos = new Vector3(0,122,-1.15f);

        //初始化击打次数
        hitTimes = 0;
        
        //初始化击打状态
        isHit = false;

        isCheck = false;
        
        //主角相关
        player = GameObject.FindWithTag("Player");
        _animator = player.GetComponent<Animator>();
        playerBalloffset = new Vector3(- 1.73f, 5.27f);
        ballInitialPos = new Vector3(-101.0f, 122.0f, -1.15f);
        lastPlayerPos = Vector3.zero;
        
        //高尔夫球杆
        length = 100.0f;
        
        //记录相机初始位置
        camera = GameObject.FindWithTag("MainCamera");
        cameraFirstPos = camera.transform.position;
        //初始速度
        cameraSpeed = 3;
        //计算相机与球的初始偏移
        offset = transform.position - camera.transform.position;
        cameraPosMinY = 62;
        cameraPosMaxY = 146;
        cameraPosMaxX = 535;
        cameraPosMinX = 0;

        isReset = false;

        resetSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //暂时用来退出游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //Debug.Log(KeyStatus._Instance._KeyStatu);
        float offset = Time.time * speed;
        //让黄色(起点终点)虚线动起来
        material.mainTextureOffset = new Vector2(offset, 0);

        //获取水平输入
        if(KeyStatus._Instance._KeyStatu == KeyStatu.ChooseDirOne || KeyStatus._Instance._KeyStatu == KeyStatu.ChooseDirTwo)
        {horizontal = Input.GetAxis("Horizontal");}
        //Debug.Log(horizontal);
        
        //选终点
        if(/*Input.GetMouseButtonDown(0) &&*/ KeyStatus._Instance._KeyStatu == KeyStatu.Initiate)
        {
            if (lastPlayerPos != player.transform.position)
            {
                //当前按键状态
                KeyStatus._Instance._KeyStatu = KeyStatu.ChooseClub;
                //选终点时启用虚线
                lineRenderer.enabled = true;
            }

//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit _hit;
//            bool isHit = Physics.Raycast(ray, out _hit);
//            if (isHit)
//            {
//                pos = _hit.point;
                pos = player.transform.position + new Vector3(50, -5.5f, 0);
                invariantPos = pos;
                //Debug.Log(pos);
//                //选终点时启用虚线
//                lineRenderer.enabled = true;
                
                isCheck = true;

                //不复位
                isReset = false;

                //杆初始长度
                length = 100.0f;
//            }
            
            //CameraMove(pos);
        }

        //选杆
        if (KeyStatus._Instance._KeyStatu == KeyStatu.ChooseClub)
        {
            lastPlayerPos = player.transform.position;
            if (ball && isCheck)
            {
                hitDirection = (invariantPos - ball.transform.position).normalized * length;
                //_animator.SetFloat("DirectionX",hitDirection.x);
                //_animator.SetFloat("DirectionY",hitDirection.y);
                
                pos = ball.transform.position + hitDirection;
//                player.transform.position = new Vector3(ball.transform.position.x + playerBalloffset.x,
//                    ball.transform.position.y + playerBalloffset.y, -2);
                isCheck = false;
            }
            else if(!ball && isCheck)
            {
                //Debug.Log(1);
                hitDirection = (invariantPos - ballInitialPos).normalized * length;
                //_animator.SetFloat("DirectionX",hitDirection.x);
                //_animator.SetFloat("DirectionY",hitDirection.y);
                pos = ballInitialPos + hitDirection;
                isCheck = false;
            }
        }
        
//        if (Input.GetKeyDown(KeyCode.T))
//        {
//            length = 70f;
//            isCheck = true;
//            //KeyStatus._Instance._KeyStatu = KeyStatu.ChooseDirOne;
//        }
//
//        if (Input.GetKeyDown(KeyCode.Y))
//        {
//            length = 20f;
//            isCheck = true;
//            //KeyStatus._Instance._KeyStatu = KeyStatu.ChooseDirOne;
//        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            KeyStatus._Instance._KeyStatu = KeyStatu.ChooseDirOne;
        }

        //选方向
        if (horizontal != 0)
        {
            //当前按键状态
            KeyStatus._Instance._KeyStatu = KeyStatu.ChooseDirTwo;
            //Debug.Log(hitDirection);
            isReset = false;
//            if (ball && isCheck)
//            {
//                hitDirection = (pos - ball.transform.position) * length;
//                pos = ball.transform.position + hitDirection;
////                player.transform.position = new Vector3(ball.transform.position.x + playerBalloffset.x,
////                    ball.transform.position.y + playerBalloffset.y, -2);
//                isCheck = false;
//            }
//            else if(!ball && isCheck)
//            {
//                hitDirection = (pos - ballInitialPos) * length;
//                pos = ballInitialPos + hitDirection;
//                isCheck = false;
//            }
            
            Vector3 direction = new Vector3(hitDirection.x, hitDirection.y, 0);
            //第一次选方向
            if (!ball)
            {
                if (pos.y > ballInitialPos.y)
                {
                    pos.x = ballInitialPos.x + direction.magnitude *
                            Mathf.Cos(Mathf.Acos(Mathf.Clamp((pos.x - ballInitialPos.x) / direction.magnitude, -1f,
                                          1)) -
                                      horizontal * Time.deltaTime);
                } 
                else if (pos.y <= ballInitialPos.y)
                {
                    pos.x = ballInitialPos.x + direction.magnitude *
                            Mathf.Cos(Mathf.PI * 2 - Mathf.Acos(Mathf.Clamp((pos.x - ballInitialPos.x) / direction.magnitude, -1f,
                                          1f)) -
                                      horizontal * Time.deltaTime);
                    //Debug.Log(1);
                }
                if (pos.x <= ballInitialPos.x)
                {
                    pos.y = ballInitialPos.y + direction.magnitude *
                            Mathf.Sin(Mathf.PI - Mathf.Asin(Mathf.Clamp((pos.y - ballInitialPos.y) / direction.magnitude, -1,
                                          1)) - 
                                      horizontal * Time.deltaTime);
                }
                else if (pos.x > ballInitialPos.x)
                {
                    pos.y = ballInitialPos.y + direction.magnitude *
                            Mathf.Sin(Mathf.Asin(Mathf.Clamp((pos.y - ballInitialPos.y) / direction.magnitude, -1,
                                          1)) -
                                      horizontal * Time.deltaTime);
                }
                _animator.SetFloat("DirectionX",pos.x - ballInitialPos.x);
                _animator.SetFloat("DirectionY",pos.y - ballInitialPos.y);
            }
            //第一次以后
            if (ball)
            {
                if (pos.y <= ball.transform.position.y)
                {
                    pos.x = ball.transform.position.x + direction.magnitude *
                            Mathf.Cos(Mathf.PI * 2 - Mathf.Acos(Mathf.Clamp((pos.x - ball.transform.position.x) / direction.magnitude,
                                          -1,
                                          1)) -
                                      horizontal * Time.deltaTime);
                }
                else if (pos.y > ball.transform.position.y)
                {
                    pos.x = ball.transform.position.x + direction.magnitude *
                            Mathf.Cos(Mathf.Acos(Mathf.Clamp((pos.x - ball.transform.position.x) / direction.magnitude,
                                          -1,
                                          1)) -
                                      horizontal * Time.deltaTime);
                }
                if (pos.x <= ball.transform.position.x)
                {
                    pos.y = ball.transform.position.y + direction.magnitude *
                            Mathf.Sin(Mathf.PI - Mathf.Asin(Mathf.Clamp((pos.y - ball.transform.position.y) / direction.magnitude, -1, 1)) - 
                                      horizontal * Time.deltaTime);
                }
                else if (pos.x > ball.transform.position.x)
                {
                    pos.y = ball.transform.position.y + direction.magnitude *
                            Mathf.Sin(Mathf.Asin(Mathf.Clamp((pos.y - ball.transform.position.y) / direction.magnitude, -1, 1)) -
                                      horizontal * Time.deltaTime);
                }
                _animator.SetFloat("DirectionX",pos.x - ball.transform.position.x);
                _animator.SetFloat("DirectionY",pos.y - ball.transform.position.y);
            }
            CameraMove(pos);
        }
        
        //绘制起点终点方向虚线
        if(ball)
            lineRenderer.SetPosition(0, new Vector3(ball.transform.position.x, ball.transform.position.y, ballInitialPos.z));
        else 
            lineRenderer.SetPosition(0, new Vector3(ballInitialPos.x, ballInitialPos.y, ballInitialPos.z));
        lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y,ballInitialPos.z));
        
        //打球
        if(/*Input.GetKeyDown(KeyCode.Z) &&*/ KeyStatus._Instance._KeyStatu == KeyStatu.Reset)
        {
            //当前按键状态
            KeyStatus._Instance._KeyStatu = KeyStatu.Shoot;
            
            isReset = false;
            //击球次数加1
            hitTimes++;
            //将球打出后关闭虚线
            lineRenderer.enabled = false;
            //实例化球
            if (hitTimes <= 1)
            {
                ball = Instantiate(ballObj, new Vector3(ballInitialPos.x, ballInitialPos.y, ballInitialPos.z), Quaternion.identity);
                //CameraMove(ball.transform.position);
            }
            else
            {
                isHit = true;
            }
            
            //播动画
            //_animator.Play("PlayerAction",0,0);
            _animator.SetBool("IsHit",true);
        }
        //摄像机跟着球
        if (KeyStatus._Instance._KeyStatu == KeyStatu.Shoot)
        {
            if(ball)
                CameraMove(ball.transform.position);
        }
        //复位
        if (/*Input.GetKey(KeyCode.R) && */KeyStatus._Instance._KeyStatu == KeyStatu.GetSliderValue)
        {
            //击球次数归零
            //hitTimes = 0;
            
            isReset = true;
        }

        if (isReset)
        {
            if(camera.transform.position.y <= 146 && camera.transform.position.y >= 62 && camera.transform.position.x >=0 &&
               camera.transform.position.x <= 535)
                if(!ball)
                    //camera.transform.position = Vector3.Lerp(camera.transform.position, cameraFirstPos, resetSpeed * Time.deltaTime); 
                    CameraMove(cameraFirstPos);
                if(ball)
//                    camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(ball.transform.position.x, 
//                            ball.transform.position.y, camera.transform.position.z), resetSpeed * Time.deltaTime); 
                    CameraMove( new Vector3(ball.transform.position.x, 
                            ball.transform.position.y, camera.transform.position.z));
        }
    }
    
    /// <summary>
    /// 摄像机移动
    /// </summary>
    /// <param name="pos">移动的终点 根据球与摄像机的偏移来计算</param>
    void CameraMove(Vector3 pos)
    {
        camera.transform.position = Vector3.Lerp(camera.transform.position, pos,
            cameraSpeed * Time.deltaTime);
        camera.transform.position = new Vector3(Mathf.Clamp(camera.transform.position.x, cameraPosMinX, cameraPosMaxX),
            Mathf.Clamp(camera.transform.position.y, cameraPosMinY, cameraPosMaxY), -113);
        if(Vector3.Distance(camera.transform.position, pos) >= 0.05f && KeyStatus._Instance._KeyStatu == KeyStatu.GetSliderValue)
        {
            if (camera.transform.position.x == cameraPosMaxX || camera.transform.position.x == cameraPosMinX)
            {
                //当前按键状态
                KeyStatus._Instance._KeyStatu = KeyStatu.Reset;
            }

            if (camera.transform.position.y == cameraPosMaxY || camera.transform.position.y == cameraPosMinY)
            {
                //当前按键状态
                KeyStatus._Instance._KeyStatu = KeyStatu.Reset;
            }
        }
        if (Vector3.Distance(camera.transform.position, pos) < 0.05f)
        {
            //当前按键状态
            KeyStatus._Instance._KeyStatu = KeyStatu.Reset;
        }
    }
}
