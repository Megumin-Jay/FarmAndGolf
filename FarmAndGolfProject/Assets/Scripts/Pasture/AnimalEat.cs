using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEat : Steering
{
    public Tank foodtank;

    private Vector3 target;
    private Vector3 _expectVelocity;
    private Vehicle _vehicle;

    void Start()
    {
        _vehicle = GetComponent<Vehicle>();
        foodtank = GameObject.FindGameObjectWithTag("foodtank").GetComponent<Tank>();
    }

    public override Vector3 Force(Vehicle me)
    {
        target = foodtank.transform.position;

        if (_vehicle.isPlanar)
            target.z = 0;
        _expectVelocity = target - transform.position;
        if (_expectVelocity.magnitude < 0.5f)//离槽极近的时候
        {
            _expectVelocity = Vector3.zero;
            me.velocity = Vector3.zero;
            //播放喝水动画？
            Debug.Log(this.name + ":在吃饭");
            if (!foodtank.Consume(me.my_animal, 1))
            {
                //喝水失败，播放希望喝水动画
                Debug.Log(this.name + ":想吃饭");
            }
        }
        else
            _expectVelocity = _expectVelocity.normalized;
        if (me.my_animal.FoodQuantity >= 100)
        {
            if (me.my_animal.WaterQuantity < 20)
                me.state = 3;
            else if (me.Energy < 20)
                me.state = 2;
            else
                me.state = 1;
        }

        return (_expectVelocity - _vehicle.velocity.normalized) * _vehicle.maxSpeed;
    }
}
