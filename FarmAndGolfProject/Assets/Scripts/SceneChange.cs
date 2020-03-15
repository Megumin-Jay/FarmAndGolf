using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public string scene;//传送目标场景名，在检查器输入
    //public float x, y;//传送后坐标，在检查器输入


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //起“传送门”的作用，用来切换场景并传送角色到下一场景
        if (collision.tag == "Protagonist")//检测碰撞物体是否为主角
        {
            SceneManager.LoadScene(scene);//切换场景
            //collision.transform.localPosition = new Vector3(x, y, 0);//改变人物坐标
        }
    }
}

