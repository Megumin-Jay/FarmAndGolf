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
    /*是否复位*/
    private bool isReset;
    /*复位速度*/
    private float resetSpeed;
    /*初始击球方向(鼠标选择)*/
    private Vector3 hitDirection;
    /*是否选择终点*/
    private bool isCheck;
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

    /// /*主人公*/
    private GameObject player;

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

        //初始化击打次数
        hitTimes = 0;
        
        //初始化击打状态
        isHit = false;

        isCheck = false;
        
        //主角相关
        player = GameObject.FindWithTag("Player");
        _animator = player.GetComponent<Animator>();
        playerBalloffset = new Vector3(- 1.73f, 5.27f);
        ballInitialPos = new Vector3(-50.0f, 57.0f, -1.15f);
        
        //记录相机初始位置
        camera = GameObject.FindWithTag("MainCamera");
        cameraFirstPos = camera.transform.position;
        //初始速度
        cameraSpeed = 3;
        //计算相机与球的初始偏移
        offset = transform.position - camera.transform.position;

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
        if(Input.GetMouseButtonDown(0) && KeyStatus._Instance._KeyStatu == KeyStatu.Initiate)
        {
            //当前按键状态
            KeyStatus._Instance._KeyStatu = KeyStatu.ChooseDirOne;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;
            bool isHit = Physics.Raycast(ray, out _hit);
            if (isHit)
            {
                pos = _hit.point;
                //Debug.Log(pos);
                //选终点时启用虚线
                lineRenderer.enabled = true;
                
                isCheck = true;

                isReset = false;
            }
            CameraMove(pos);
        }

        //选方向
        if (horizontal != 0)
        {
            //当前按键状态
            KeyStatus._Instance._KeyStatu = KeyStatu.ChooseDirTwo;
            
            isReset = false;
            if (ball && isCheck)
            {
                hitDirection = pos - ball.transform.position;
                player.transform.position = new Vector3(ball.transform.position.x + playerBalloffset.x,
                    ball.transform.position.y + playerBalloffset.y, -2);
                isCheck = false;
            }
            else if(!ball && isCheck)
            {
                hitDirection = pos - ballInitialPos;
                isCheck = false;
            }
            
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
        if(Input.GetKeyDown(KeyCode.Z) && KeyStatus._Instance._KeyStatu == KeyStatu.Reset)
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
        //复位
        if (Input.GetKey(KeyCode.R) && KeyStatus._Instance._KeyStatu == KeyStatu.ChooseDirTwo)
        {
            //当前按键状态
            KeyStatus._Instance._KeyStatu = KeyStatu.Reset;
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
        camera.transform.position = new Vector3(Mathf.Clamp(camera.transform.position.x, 0, 535),
            Mathf.Clamp(camera.transform.position.y, 62, 146), -113);
    }
}
