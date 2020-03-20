using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//非常非常简陋的时间暂停orz,暂时给弹窗用
public class OnPause : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }
}
