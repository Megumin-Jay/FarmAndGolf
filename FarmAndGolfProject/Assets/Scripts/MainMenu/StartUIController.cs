using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour
{
    public GameObject Buttons;//初始按钮组
    public GameObject text;//提示文字
    private float colorA = 1;//提示文字透明度
    private bool colorChange = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (text != null && Buttons != null)
        {
            EnterGame();
            ColorChange();
        }
    }

    void EnterGame()
    {
        if (Input.anyKeyDown)
        {
            text.SetActive(false);
            Buttons.SetActive(true);
        }
    }

    void ColorChange()
    {
        if (colorChange)
        {
            colorA -= 0.03f;
            if (0.1f - colorA > 0)
                colorChange = false;
        }
        if (!colorChange)
        {
            colorA += 0.03f;
            if (colorA - 0.9f > 0)
                colorChange = true;
        }
        text.GetComponent<Text>().color = new Color(0, 0, 0, colorA);
    }

    //切换到CG
    public void toCG()
    {
        SceneManager.LoadScene("CG");
    }

    //退出游戏
    public void Exit()
    {
        Application.Quit();
    }
}
