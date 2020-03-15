using UnityEngine;
using System.Collections;

/// <summary>
/// 生成物品的代码，仅测试用
/// 顺便 用来打开背包
/// </summary>

public class InputDetector : MonoBehaviour
{

    public GameObject KnapsackUI;//背包

    private void Awake()
    {
        GameObject KnapsackUI = GameObject.Find("KnapsackUI");
    }

    private void Start()
    {
        //在一开始先运行一下背包的Awake()
        KnapsackUI.SetActive(true);
        KnapsackUI.SetActive(false);
    }

    void Update()
    {
        //点击空格键，生成物品
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomIndex = Random.Range(0, 7);
            KnapsackManager.Instance.StoreItem(randomIndex);
        }

        //将"I"设为背包键
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (KnapsackUI.activeInHierarchy)
                KnapsackUI.SetActive(false);
            else
                KnapsackUI.SetActive(true);
        }
    }
}