using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Area2")
        {
            //Debug.Log(1);
            FloorStatus._Instance._floorStatu = FloorStatu.Area2;
        }

        if (col.tag == "Area3")
        {
            FloorStatus._Instance._floorStatu = FloorStatu.Area4;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Area2" || col.tag == "Area3")
        {
            //Debug.Log(1);
            FloorStatus._Instance._floorStatu = FloorStatu.Area3;
        }
    }
}
