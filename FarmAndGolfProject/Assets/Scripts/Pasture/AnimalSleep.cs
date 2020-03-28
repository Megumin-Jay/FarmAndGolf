using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSleep : Steering
{
    public override Vector3 Force(Vehicle me)
    {
        if (me.velocity != Vector3.zero)
            Debug.Log(this.name + ":在睡觉");
        if (me.Energy >= 100)
        {
            if (me.my_animal.WaterQuantity < 20)
                me.state = 3;
            else if (me.my_animal.FoodQuantity < 20)
                me.state = 4;
            else
                me.state = 1;
        }
        else
            me.velocity = Vector3.zero;
        return Vector3.zero;
    }
}
