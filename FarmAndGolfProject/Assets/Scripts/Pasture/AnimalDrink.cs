using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDrink : Steering
{
    public Tank watertank;

    private Vector3 target;
    private Vector3 _expectVelocity;
    private Vehicle _vehicle;

    void Start()
    {
        _vehicle = GetComponent<Vehicle>();
        watertank=GameObject.FindGameObjectWithTag("watertank").GetComponent<Tank>();
    }

    public override Vector3 Force(Vehicle me)
    {
        target = watertank.transform.position;
        if (_vehicle.isPlanar)
            target.z = 0;
        _expectVelocity = target - transform.position;
        if (_expectVelocity.magnitude < 0.5f)//离槽极近的时候
        {
            _expectVelocity = Vector3.zero;
            me.velocity = Vector3.zero;
            //播放喝水动画？
            Debug.Log(this.name+":在喝水");
            if (!watertank.Consume(me.my_animal, 0))
            {
                //喝水失败，播放希望喝水动画
                Debug.Log(this.name + ":想喝水");
            }
        }
        else
            _expectVelocity = _expectVelocity.normalized;
        if (me.my_animal.WaterQuantity>=100)
        {
            if (me.my_animal.FoodQuantity < 20)
                me.state = 4;
            else if (me.Energy < 20)
                me.state = 2;
            else
                me.state = 1;
        }

        return (_expectVelocity - _vehicle.velocity.normalized) * _vehicle.maxSpeed;
    }
}
