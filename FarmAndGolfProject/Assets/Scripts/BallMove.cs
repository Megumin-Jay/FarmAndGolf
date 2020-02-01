//由于3d高尔夫的抛物线在2d下，看到的轨迹并不符合常理，所以在2d平面根据映射关系绘制一条假抛物线，用一个假球沿着假抛物线轨迹运动

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    #region Const
    /*重力*/
    public readonly float gravity = 9.8f;
    /*空气密度*/
    [SerializeField]
    private float AirDensity = 0.1f;
    /*形状参数*/
    [SerializeField]
    private float shapeParameter = 1.0f;
    /*横截面积*/
    [SerializeField]
    private float crossArea = 1.0f;
    /*Z方向速度归零的阈值*/
    public float speedThresholdZ = 3.0f;
    /*y方向速度归零的阈值*/
    public float speedThresholdY = 0.2f;
    /*x方向速度归零的阈值*/
    public float speedThresholdX = 0.2f;
    #endregion

    /// Ball
    /*球的质量*/
    public float ballMass;
    /*球的初始速度*/
    public Vector3 moveSpeed = Vector3.zero;
    /*球当前旋转角度*/
    public float angle;
    /*弧度*/
    public float radian;
    public Rigidbody rb;
    /*球高度方向停止运动*/
    public bool canStop = false;
    /*球升力系数*/
    public float liftForceRatio = 0.1f;
    /*球升力*/
    public Vector3 liftForce;
    /*风力系数(跟判定有关)*/
    public float windRatio = 1.0f;
    /*三个方向上的初速度*/
    public float movespeedX;
    public float movespeedY;
    public float movespeedZ;
    /*模拟抛物线上的假球*/
    public GameObject fakeBallObj;
    private GameObject fakeBall;
    /*球最大的时候的大小*/
    public float MaxScale;
    /*球最小的时候的大小*/
    public float MinScale;
    /*假球的摧毁时间*/
    public float fakeBallRuinTime;
    /*真球的摧毁时间*/
    public float ballRuinTime;
    /*假球的透明化速度 用透明代表进洞*/
    public float fakeBallBleachSpeed;

    /// BallArm
    /*击打的方向和力*/
    public Vector3 hitDirection;
    /*击打的力*/
    //public float hitForce;
    /*击球次数*/
    public int hitTimes;
    
    /// Environment
    /*地面弹力*/
    public float groundBounceY;

    public float groundBounceX;
    public float groundBounceZ;
    /*地面摩擦力*/
    public float groundFrictionY;
    public float groundFrictionX;
    //public float windForce;
    /*空气阻力*/
    public Vector3 airForceDirection;
    /*风力(跟球有关)*/
    public Vector3 windDirection;
    /*地面高度*/
    public float groundHeight;
    /*地面类型*/
    //public FloorStatus _FloorStatus;

    /// camera
    private GameObject _camera;
    /*相机与球的偏移*/
    private Vector3 offset;
    /*相机移动速度*/
    private float cameraSpeed;

    /// 画线相关
    /*画线组件*/
    private LineRenderer lineRenderer;
    //索引
    private int index;
    //点的数目
    private int dotCount;

    private BallDir _ballDir;
//    //修正后的轨迹的点的位置
//    private Vector3 dotPos;
    
    // Start is called before the first frame update
    void Start()
    {
        //_ballDir = GameObject.FindWithTag("GameController").GetComponent<BallDir>();
//        //注册
//        BallDir.Instance.publisher += HitBall;

        HitBall(BallDir.Instance.HitTimes, BallDir.Instance.Pos, movespeedZ);

        //初始速度
        cameraSpeed = 3;
        
        //击球次数
        hitTimes = BallDir.Instance.HitTimes;

        canStop = false;

        //一些组件
        lineRenderer = GameObject.FindWithTag("GameController").GetComponent<LineRenderer>();
        //初始 假抛物线绘画组件启用
        lineRenderer.enabled = true;
        rb = this.GetComponent<Rigidbody>();
        _camera = GameObject.FindWithTag("MainCamera").gameObject;
        //_FloorStatus = GameObject.FindWithTag("GameController").GetComponent<FloorStatus>();

        //计算相机与球的初始偏移
        offset = transform.position - _camera.transform.position;
        //CameraMove(transform.position - offset);
        //为了绘制假抛物线绘制的点个数清零
        dotCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //_FloorStatus._floorStatu = FloorStatu.Area2;
        //Debug.Log(FloorStatus._Instance._floorStatu);
        //CameraMove(transform.position - offset);
        //Debug.Log(_camera);
        //阻力
        airForceDirection = 0.5f * AirDensity * shapeParameter * crossArea * moveSpeed.magnitude * (-moveSpeed);
        
        //升力
        liftForce = Vector3.Cross(Vector3.right, moveSpeed.normalized).normalized;
//        if(liftForce.z != 0)
//            liftForce = new Vector3(0,liftForce.y * (liftForceRatio * moveSpeed.z) / liftForce.z, liftForceRatio * moveSpeed.z);
        
        //风力
        windDirection = ballMass * windRatio * windDirection.normalized;

        if (transform.position.z >= -1)
        {
            canStop = true;
        }
        
        //当球无法在弹跳 只能在地上摩擦时
        if (canStop)
        {
            //速度
            moveSpeed.z = 0;
            //f/m=a
            moveSpeed.y = (Mathf.Abs(moveSpeed.y) <= speedThresholdY) ? 0 : moveSpeed.y - 
                                                                            moveSpeed.y / Mathf.Abs(moveSpeed.y)
                                                                            * groundFrictionY / ballMass * Time.fixedDeltaTime;
            moveSpeed.x = (Mathf.Abs(moveSpeed.x) <= speedThresholdX) ? 0 : moveSpeed.x - 
                                                                            moveSpeed.x / Mathf.Abs(moveSpeed.x) 
                                                                            * groundFrictionX / ballMass * Time.fixedDeltaTime;
            if (moveSpeed.x == 0 && moveSpeed.y == 0 && moveSpeed.z == 0)
            {
                //一次击球结束后 会将fakeball销毁 所以这里用来判断一次击球是否结束
//                if(fakeBall)
//                    CameraMove(transform.position - offset);
                fakeBallBleach();
                if (BallDir.Instance.IsHit)
                {
                    if (FloorStatus._Instance._floorStatu != FloorStatu.Area1)
                    {
                        HitBall(BallDir.Instance.HitTimes, BallDir.Instance.Pos, movespeedZ);
                    }

                    if (FloorStatus._Instance._floorStatu == FloorStatu.Area1)
                    {
                        HitBall(BallDir.Instance.HitTimes, BallDir.Instance.Pos, 0);
                    }

                    BallDir.Instance.IsHit = false;
                }
            }
        }
        if (!canStop)
        {
            //速度
            moveSpeed.z += gravity * Time.fixedDeltaTime;
            moveSpeed += (airForceDirection + liftForce + windDirection) / ballMass * Time.fixedDeltaTime;
            //rb.MovePosition(moveSpeed * Time.deltaTime);
        }
        transform.Translate(moveSpeed * Time.fixedDeltaTime,
            Space.World);
        
        ///绘制红色的轨迹
        //点数加一
        dotCount++;
        Vector3 dotPos = new Vector3(transform.position.x + Mathf.Abs(transform.position.z - (groundHeight)), transform.position.y,groundHeight);
        lineRenderer.SetVertexCount(dotCount);
        //让假球沿着假抛物线的轨迹运动 大小根据真球离地面的高度进行模拟
        if (fakeBall)
        {
            fakeBall.transform.position = dotPos;
            fakeBall.transform.localScale = new Vector3(
                Mathf.Clamp(Mathf.Abs(transform.position.z - (groundHeight)) , MinScale, MaxScale),
                Mathf.Clamp(Mathf.Abs(transform.position.z - (groundHeight)) , MinScale, MaxScale), 1);
        }

        while (index < dotCount)
        {
            lineRenderer.SetPosition(index, dotPos);
            index++;
        }

        //角度
//        radian = Mathf.Atan(moveSpeed.z / moveSpeed.y);
//        angle = (180 / Mathf.PI * radian);
//        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
//            transform.eulerAngles.y,angle);

    }
    
    /// <summary>
    /// 打球
    /// </summary>
    /// <param name="_hitTimes">挥棒次数</param>
    /// <param name="pos">球的终点</param>
    void HitBall(int _hitTimes, Vector3 pos, float speedZ)
    {
        canStop = false;
        //Debug.Log(1);
        //挥棒的力
        hitDirection = (pos - transform.position);
        //pos.x = hitDirection.magnitude * Mathf.Cos()
        
        
        //初始速度
        moveSpeed.x = hitDirection.x / movespeedX;
        moveSpeed.y = hitDirection.y / movespeedY;
        moveSpeed.z = speedZ;

        //假球
        fakeBall = Instantiate(fakeBallObj, transform.position, Quaternion.identity);
    }
    
    /// <summary>
    /// 摄像机移动
    /// </summary>
    /// <param name="pos">移动的终点 根据球与摄像机的偏移来计算</param>
    void CameraMove(Vector3 pos)
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, pos,
            cameraSpeed * Time.deltaTime);
        _camera.transform.position = new Vector3(Mathf.Clamp(_camera.transform.position.x, 0, 535),
            Mathf.Clamp(_camera.transform.position.y, 62, 146), -113);
    }
    
    //计算球的弹跳 摩擦
    void OnCollisionEnter(Collision col)
    {
        //非弹性碰撞
        if (col.collider.tag == "Ground")
        {
            //Debug.Log("碰撞");
            
            
            ContactPoint contactPoint = col.contacts[0];
            Vector3 newDir = Vector3.zero;
            Vector3 curDir = moveSpeed;
            newDir = Vector3.Reflect(curDir, contactPoint.normal);
            //Debug.Log(newDir + "newDir");
            //速度衰减
            moveSpeed = new Vector3(groundBounceX* newDir.x, groundBounceY * newDir.y, groundBounceZ * newDir.z);
            
            
            //升力系数置零
            liftForceRatio = 0;
            
            //风力系数置零
            windRatio = 0;

            //竖直方向速度归零
            if (Mathf.Abs(moveSpeed.z) < speedThresholdZ)
            {
                canStop = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //球进洞
        if (col.gameObject.tag == "Hole")
        {
            //摧毁假球
            //fakeBall.transform.Translate(Vector3.forward,Space.World);
            Destroy(fakeBall,fakeBallRuinTime);
            //摧毁真球
            Destroy(gameObject,ballRuinTime);
        }
    }

    void OnTriggerStay(Collider col)
    {
        //进洞动画
        if (col.gameObject.tag == "Hole")
        {
            fakeBallBleach();
        }
        //暂定推杆区
        if (col.gameObject.tag == "Area1")
        {
            Debug.Log(1);
            FloorStatus._Instance._floorStatu = FloorStatu.Area1;
        }
    }    

//    void OnTriggerExit(Collider col)
//    {
//        //暂定推杆区
//        if (col.gameObject.tag == "Area1")
//        {
//            inArea = false;
//        }
//    }

    /// <summary>
    /// 假球渐变消失
    /// </summary>
    void fakeBallBleach()
    {
        if (fakeBall)
        {
//            fakeBall.GetComponent<SpriteRenderer>().color = new Color(fakeBall.GetComponent<SpriteRenderer>().color.r,
//                fakeBall.GetComponent<SpriteRenderer>().color.g, fakeBall.GetComponent<SpriteRenderer>().color.b,
//                fakeBall.GetComponent<SpriteRenderer>().color.a - Time.deltaTime * fakeBallBleachSpeed);
            Destroy(fakeBall, fakeBallRuinTime);
        }
    }
     
//    /// <summary>
//    /// 基础线绘制
//    /// </summary>
//    /// <param name="startPoint"></param>
//    /// <param name="endPoint"></param>
//    void DrawBaseLine(Vector2 startPoint,Vector2 endPoint)
//    {
//        //Graphics.DrawMesh();
//        //Debug.DrawLine(startPoint, endPoint, Color.black);
//        lineRenderer.SetPosition(0,new Vector3(startPoint.x,startPoint.y,-1));
//        lineRenderer.SetPosition(1,new Vector3(endPoint.x,endPoint.y,-1));
//    }

    void DrawParaCurveLine()
    {
        
    }
}
