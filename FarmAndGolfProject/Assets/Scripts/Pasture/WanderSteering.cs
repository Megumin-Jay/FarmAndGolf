using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderSteering : Steering //随机徘徊
{
    //预想动物在一个椭圆内随机徘徊
    //先生成一个向量，再随机生成一个长度，再将向量移动到目标点
    //这个是动物移动的目标，移动到目标后再生成下一个目标

    public Vector3 centerPoint;

    public float a = 10;
    public float b = 5;

    [SerializeField] private float _distance;
    [SerializeField] private float _timer;
    [SerializeField] private Vector3 _expectPoint;//目标点
    [SerializeField] private Vector3 _expectVelocity;
    [SerializeField] private Vector3 vec;

    // Start is called before the first frame update
    void Start()
    {
        _expectPoint = Vector3.zero;
        _timer = Time.time;
    }

    public override Vector3 Force(Vehicle me)
    {
        vec = _expectPoint - transform.position;
        if ((Time.time - _timer)>5.0f||vec.magnitude<0.15f)//一段时间后或者接近目标点时，重设目标点
        {
            float Radius = Random.Range(0, 2 * Mathf.PI);
            float RandomX = Mathf.Cos(Radius);
            float RandomY = Mathf.Sin(Radius);
            float k = Mathf.Tan(Radius);
            float MaxDistance = Mathf.Sqrt(a * a * b * b * (k * k + 1) / (b * b + a * a * k * k));
            _distance = Random.Range(0, MaxDistance);
            _expectPoint = new Vector3(RandomX, RandomY, transform.position.z);      
            _expectPoint = _expectPoint * _distance + centerPoint;//变换为世界坐标
            _timer = Time.time;
            //Debug.Log(_expectPoint);
            //Debug.Log(vec.magnitude);
        }
        _expectVelocity = _expectPoint - transform.position - me.velocity;

        if (me.my_animal.WaterQuantity < 20)
            me.state = 3;
        else if (me.my_animal.FoodQuantity < 20)
            me.state = 4;
        else if (me.Energy < 20)
            me.state = 2;

        return _expectVelocity;
    }
}
