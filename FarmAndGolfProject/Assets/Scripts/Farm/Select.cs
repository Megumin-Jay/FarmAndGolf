using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    [SerializeField] Camera cam;
    public GameObject player;
    [SerializeField] Vector3 mp;//鼠标位置
    private float targetingRayLength = Mathf.Infinity;//射线的长度

    void Start()
    {
        cam = this.GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TarRaycast();
        }
    }

    public void TarRaycast()
    {
        mp = Input.mousePosition;
        if (cam != null)
        {
            RaycastHit hitInfo;
            Ray ray = cam.ScreenPointToRay(new Vector3(mp.x, mp.y, 0f));
            if (Physics.Raycast(ray, out hitInfo, targetingRayLength))
            {
                if(hitInfo.transform.tag=="Earth")
                {
                    FarmOperation far = player.GetComponent<FarmOperation>();
                    if(far!=null)
                    {
                        far.My_earth = hitInfo.transform.gameObject.GetComponent<Earth>();
                    }
                }
                else if(hitInfo.transform.tag == "Animal")
                {
                    PastureOP pas = player.GetComponent<PastureOP>();
                    if(pas!=null)
                    {
                        pas.My_Ani= hitInfo.transform.gameObject.GetComponent<AnimalGrowth>();
                    }
                }
            }
        }
    }
}
