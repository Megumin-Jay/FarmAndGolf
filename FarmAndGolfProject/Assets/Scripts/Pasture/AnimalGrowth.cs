using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGrowth : MonoBehaviour
{
    [SerializeField] float totaltime = 0;//生长总时间
    public float Totaltime { set { totaltime = value; } get { return totaltime; } }
    [SerializeField] int stage = 0;//生长阶段
    public int Stage { set { stage = value; } get { return stage; } }
    public float[] grewtime;//各阶段总生长所需时间，单位为s
    [SerializeField] float grewspeed = 1;//成长速度
    //测试用，贴图应该在行为类里面更改
    public Sprite[] sp;//各生长阶段的贴图
    public SpriteRenderer this_sp;//现有的贴图
    public string typename;
    public int minanimalnum;
    public int maxanimalnum;
    public GameObject child;//动物幼崽，也可以当成球

    [SerializeField] float waterquantity = 0;//饮水量，共100
    public float WaterQuantity { set { waterquantity = value; } get { return waterquantity; } }
    [SerializeField] float waterspeed = 0;//水消耗速度，单位为/s
    [SerializeField] float foodquantity = 0;//食物量，共100
    public float FoodQuantity { set { foodquantity = value; } get { return foodquantity; } }
    [SerializeField] float foodspeed = 0;//食物消耗速度，单位为/s

    public bool Isharvest { get { return totaltime >= grewtime[grewtime.Length - 1]; } }//是否成熟
    public bool IsStopGrowth { get { return grewspeed <= 0; } }//是否停止生长，可能遇到饲料不足、水喂得不够的情况

    private void Start()
    {
        this_sp = this.gameObject.GetComponent<SpriteRenderer>();
        this_sp.sprite = sp[0];//测试用，贴图应该在行为类里面更改
        Growth();
    }
    private void Update()
    {
        if (totaltime <= grewtime[grewtime.Length - 1] + 1 && totaltime >= 0)
            totaltime += Time.deltaTime * grewspeed;
        ChangeSpeed();
        ConsumeWaterFood();
        Growth();
        this_sp.sprite = sp[stage];//测试用，贴图应该在行为类里面更改
    }
    void Growth()
    {
        if (stage < grewtime.Length && totaltime >= grewtime[stage])
            stage++;
    }
    void ChangeSpeed()
    {
        if(waterquantity == 0&& foodquantity == 0)
        {
            grewspeed = 0;
        }
        else if (waterquantity == 0 || foodquantity == 0)
        {
            grewspeed = 0.5f;
        }
        else
        {
            grewspeed = 1;
        }
    }
    void ConsumeWaterFood()
    {
        if (waterquantity > 0)
        {
            waterquantity -= Time.deltaTime * waterspeed;
        }
        else
            waterquantity = 0;
        if (foodquantity > 0)
        {
            foodquantity -= Time.deltaTime * foodspeed;
        }
        else
            foodquantity = 0;
    }
    public void Harvest(ref int aninum)
    {
        if (!Isharvest)
            return;
        Random.InitState((int)System.DateTime.Now.Ticks);
        int num = Random.Range(minanimalnum, maxanimalnum);
        aninum = num;
        for (int i = 0; i < num; i++)
        {
            GameObject fru = Instantiate(child);
            fru.transform.position = this.transform.position + new Vector3(-0.1f * i, 0, -0.01f);
        }
        Destroy(this.gameObject);
    }
}