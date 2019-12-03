using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDir : MonoBehaviour
{
    #region private
    /*射线检测*/
    //private RaycastHit _hit;
    private Vector3 pos;
    #endregion
    
    #region public
    /*鼠标点击方向*/
    public Vector3 Pos
    {
        get => pos;
        set => pos = value;
    }
    /*画线组件*/
    public LineRenderer lineRenderer;
    /*起点终点方向虚线所用的材质*/
    public Material material;
    /*要实例化的球*/
    public GameObject ballObj;
    /*起点终点方向虚线uv的移动速度*/
    public float speed;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
//        if(lineRenderer != null)
//            material = lineRenderer.gameObject    
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * speed;
        material.mainTextureOffset = new Vector2(offset, 0);
        
        //点击
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;
            bool isHit = Physics.Raycast(ray, out _hit);
            if (isHit)
            {
                pos = _hit.point;
                Debug.Log(pos);
                lineRenderer.enabled = true;
                //GameObject bal    l = Instantiate(ballObj, new Vector3(0, 0, -1.15f), Quaternion.identity);
                lineRenderer.SetPosition(0, new Vector3(0, 0 , -1.15f));
                lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y,-1.15f));
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            lineRenderer.enabled = false;
            GameObject ball = Instantiate(ballObj, new Vector3(0, 0, -1.15f), Quaternion.identity);
        }
    }
}
