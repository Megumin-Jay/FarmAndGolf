using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AutoSceneChanger : MonoBehaviour
{
    public int videoLength;
    void Start()
    {
        StartCoroutine(waitChangeScene());
    }

    void Update()
    {
        //按ESC跳过开场CG
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Village");
        }
    }

    IEnumerator waitChangeScene()
    {
        yield return new WaitForSeconds(videoLength);
        SceneManager.LoadScene("Village");
    }
}
