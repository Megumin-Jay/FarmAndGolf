using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUP : itemOnWorld
{
    [SerializeField] float time = 0;
    // Update is called once per frame
    void Update()
    {
        if (time <= 1.5f)
            time += Time.deltaTime;
        else
            Pick();
    }

    void Pick()
    {
        AddNewItem();//添加物品到背包
        Destroy(gameObject);//销毁"物品"->销毁场景中的这个能看到的物品
    }
}
