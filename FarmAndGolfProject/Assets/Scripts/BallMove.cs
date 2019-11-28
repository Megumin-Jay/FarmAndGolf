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
    /*竖直方向速度归零的阈值*/
    public const float speedThreshold = 50.0f;
    #endregion

    /// Ball
    /*球的质量*/
    public float ballMass;
    /*球的初始速度*/
    public Vector3 moveSpeed = Vector3.zero;
    /*球当前旋转角度*/
    public float angle;
    /*弧度*/
    public float radius;
    public Rigidbody rb;
    /*球停止运动*/
    public bool canStop = false;
    /*球升力系数*/
    public float liftForceRatio = 0.1f;
    /*球升力*/
    public Vector3 liftForce;
    /*风力系数(跟判定有关)*/
    public float windRatio = 1.0f;

    /// BallArm
    /*击打的方向和力*/
    public Vector3 hitDirection;
    /*击打的力*/
    //public float hitForce;
    
    /// Environment
    /*地面弹力*/
    public float groundBounceY;

    public float groundBounceX;
    /*地面摩擦力*/
    public float groundFrictionY;
    public float groundFrictionX;
    //public float windForce;
    /*空气阻力*/
    public Vector3 airForceDirection;
    /*风力(跟球有关)*/
    public Vector3 windDirection;
    
    //
    public LineRenderer lineRenderer;
    //索引
    private int index;
    //点的数目
    private int dotCount;
    //修正后的轨迹的点的位置
    private Vector3 dotPos;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed.x = hitDirection.x;
        moveSpeed.y = hitDirection.y;
        moveSpeed.z = hitDirection.z;

        rb = this.GetComponent<Rigidbody>();
        dotCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //阻力
        airForceDirection = 0.5f * AirDensity * shapeParameter * crossArea * moveSpeed.magnitude * (-moveSpeed);
        
        //升力
        liftForce = Vector3.Cross(Vector3.right, moveSpeed).normalized;
        if(liftForce.z != 0)
            liftForce = new Vector3(0,liftForce.y * (liftForceRatio * moveSpeed.z) / liftForce.z, liftForceRatio * moveSpeed.z);
        
        //风力
        windDirection = ballMass * windRatio * windDirection.normalized;
        
        if (canStop)
        {
            //速度
            moveSpeed.z = 0;
            //f/m=a
            moveSpeed.y = (moveSpeed.y <= 0) ? 0 : moveSpeed.y - groundFrictionY / ballMass * Time.fixedDeltaTime;
            moveSpeed.x = (moveSpeed.x <= 0) ? 0 : moveSpeed.x - groundFrictionX / ballMass * Time.fixedDeltaTime;
            
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
        //点数加一
        dotCount++;
        dotPos += new Vector3(Mathf.Abs(transform.position.z - (-1)), transform.position.y,-1);

//        while (index < dotCount)
//        {
//            lineRenderer.SetPosition(index, dotPos);
//            index++;
//        }

        //角度
//        radius = Mathf.Atan(moveSpeed.z / moveSpeed.y);
//        angle = (180 / Mathf.PI * radius);
//        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
//            transform.eulerAngles.y,angle);

    }

    void OnCollisionEnter(Collision col)
    {
        //非弹性碰撞
        if (col.collider.tag == "Ground")
        {
            Debug.Log("碰撞");
            
            
            ContactPoint contactPoint = col.contacts[0];
            Vector3 newDir = Vector3.zero;
            Vector3 curDir = moveSpeed;
            newDir = Vector3.Reflect(curDir, contactPoint.normal);
            //竖直方向速度衰减，水平方向衰减
            moveSpeed = new Vector3(newDir.x, groundBounceX * newDir.y, groundBounceX * newDir.z);
            //moveSpeed = new Vector3(groundBounceX * newDir.x, newDir.y, newDir.z);
            
            //升力系数置零
            liftForceRatio = 0;
            
            //风力系数置零
            windRatio = 0;

            //竖直方向速度归零
            if (moveSpeed.sqrMagnitude < speedThreshold)
            {
                canStop = true;
            }
        }
    }

     
    /// <summary>
    /// 基础线绘制
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    void DrawBaseLine(Vector2 startPoint,Vector2 endPoint)
    {
        //Graphics.DrawMesh();
        //Debug.DrawLine(startPoint, endPoint, Color.black);
        lineRenderer.SetPosition(0,new Vector3(startPoint.x,startPoint.y,-1));
        lineRenderer.SetPosition(1,new Vector3(endPoint.x,endPoint.y,-1));
    }

    void DrawParaCurveLine()
    {
        
    }
}
