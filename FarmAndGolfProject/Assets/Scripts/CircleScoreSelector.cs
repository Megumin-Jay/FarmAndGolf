using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleScoreSelector : MonoBehaviour
{
    /*高尔夫球杆ui图标的字典*/
    private Dictionary<GameObject, string> golfClubsDictionary;
    /*场景中高尔夫球杆ui的集合*/
    private GameObject[] golfClubsList;
    /*场景中高尔夫球杆ui的初始位置*/
    private Vector3 FirstPos;
    
    /*场景中高尔夫球杆ui排列的圆的半径*/
    public float radius;
    
    /*计时器*/
    private float localTime;
    
    /*是否旋转*/
    private bool canRotate;
    /*旋转次数*/
    private int rotateTimes;
    // Start is called before the first frame update
    void Start()
    {
        //初始化
        localTime = 1.00f;
        canRotate = false;
        rotateTimes = 0;
        
        golfClubsList = GameObject.FindGameObjectsWithTag("GolfClubs");
        //记录旋转的中心点
        FirstPos = golfClubsList[0].gameObject.transform.position;
        //ui初始化时自动排列成圆环状
        UIArrangeInCircle(golfClubsList.Length, golfClubsList);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (rotateTimes == 5)
                rotateTimes = -1;
            canRotate = true;
            localTime = 1.00f;
            //旋转次数加一
            rotateTimes++;
        }
        //Debug.Log(Time.time - localTime);
        if (canRotate)
        {
            localTime -= Time.fixedUnscaledDeltaTime;
            //更新轮盘上按钮位置
            UpdateNewPos(golfClubsList.Length, 1);
            //更新状态
            UpdateNewState(rotateTimes);
            //过了1s 停止旋转
            if (localTime <= 0.01f)
            {
                canRotate = false;
            }
        }
    }

    /// <summary>
    /// ui初始化时自动排列成圆环状
    /// </summary>
    /// <param name="uiNum">ui个数</param>
    /// <param name="uiList">ui集合</param>
    void UIArrangeInCircle(int uiNum, GameObject[] uiList)
    {
        float angle = 60;
        for (int i = 0; i < uiNum; i++)
        {
            //角度转弧度
            float radian = (angle / 180) * Mathf.PI;
            float xPos = radius * Mathf.Cos(radian);
            float yPos = radius * Mathf.Sin(radian);

            golfClubsList[i].gameObject.transform.localPosition = new Vector3(xPos, yPos, 0);
            
            //ui间间隔的角度
            angle += 360 / uiNum;
        }
    }

    /// <summary>
    /// 更新轮盘上各按钮位置
    /// </summary>
    /// <param name="direction">正数为顺时针 负数为逆时针</param>
    void UpdateNewPos(int uiNum,int direction)
    {
//        Debug.Log(FirstPos);
        for (int i = 0; i < uiNum; i++)
        {
            golfClubsList[i].gameObject.transform.RotateAround(FirstPos, Vector3.back,Time.fixedUnscaledDeltaTime  * direction * 360/uiNum);
            golfClubsList[i].transform.localRotation = Quaternion.identity;
        }
    }

    void UpdateNewState(int index)
    {
        Debug.Log(index);
        Color color = golfClubsList[index].gameObject.GetComponent<Image>().color;
        if (index >= 1)
        {
            Color color0 = golfClubsList[index - 1].gameObject.GetComponent<Image>().color;
            golfClubsList[index].gameObject.GetComponent<Image>().color = 
                Color.Lerp(color, new Color(color.r, color.g, color.b, 255), Time.fixedUnscaledDeltaTime * 10);
            golfClubsList[index - 1].gameObject.GetComponent<Image>().color =
                Color.Lerp(color0, new Color(color0.r, color0.g, color0.b, 0), Time.fixedUnscaledDeltaTime * 10);
            //Debug.Log(1);
        }
        else
        {
            Color color0 = golfClubsList[5].gameObject.GetComponent<Image>().color;
            golfClubsList[index].gameObject.GetComponent<Image>().color =
                Color.Lerp(color, new Color(color.r, color.g, color.b, 255), Time.fixedUnscaledDeltaTime * 10);
            golfClubsList[5].gameObject.GetComponent<Image>().color =
                Color.Lerp(color0, new Color(color0.r, color0.g, color0.b, 0), Time.fixedUnscaledDeltaTime * 10);
        }
    }
}
