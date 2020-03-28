using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILocomotion : Vehicle
{
    //private CharacterController _controller;            //角色控制器
    private Rigidbody _rigidbody;                       //刚体属性
    private Vector3 _moveDistance;                      //移动距离

    void Start()
    {
       // _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _moveDistance = Vector3.zero;
        my_animal = GetComponent<AnimalGrowth>();
    }

    //物理计算在Update中更新
    void FixedUpdate()
    {
        //计算AI角色的当前速度
        velocity += _acceleration * Time.fixedDeltaTime;

        //限制AI角色的移动速度不超过最大速度
        if (velocity.sqrMagnitude > _sqrMaxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        //计算一帧移动距离
        _moveDistance = velocity * Time.fixedDeltaTime;

        //如果是在平面上移动，Z轴偏移量为0
        if (isPlanar)
        {
            velocity.z = 0;
            _moveDistance.z = 0;
        }

        //控制AI角色移动
        transform.position += _moveDistance;

        //优化人物转向，只有两个方向，左、右
        
        if (velocity.x < 0)
        {
            Vector3 newForward = new Vector3(0,0,-1);
            transform.forward = newForward;
        }
        else
        {
            Vector3 newForward = new Vector3(0, 0, 1);
            transform.forward = newForward;
        }
    }
}
