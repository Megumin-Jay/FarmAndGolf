using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant:MonoBehaviour
{
    public string typename;//植物种类的名字，要和植物预制体的名字一样
    [SerializeField] int maxfruitnum;
    [SerializeField] int minfruitnum;
    [SerializeField] float totaltime = 0;//种植时间
    public float Totaltime { set { totaltime = value; } get { return totaltime; } }
    [SerializeField] int stage = 0;//种植阶段
    public int Stage { set { stage = value; } get { return stage; } }
    public float[] grewtime;//各阶段总生长所需时间，单位为s
    [SerializeField] float waterspeed = 1;//干旱生长变慢，湿润土地生长变快
    [SerializeField] float fertilizedspeed = 1;//施肥的土地，生长变快
    public Sprite[] sp;//各生长阶段的贴图
    SpriteRenderer this_sp;//现有的贴图
    [SerializeField] Earth this_earth = null;
    [SerializeField] GameObject fruit;//果实

    public bool Isharvest { get { return totaltime>=grewtime[grewtime.Length-1]; } }

    private void Start()
    {
        this_sp = this.gameObject.GetComponent<SpriteRenderer>();
        this_sp.sprite = sp[0];
        Growth();
    }
    private void Update()
    {
        if (totaltime <= grewtime[grewtime.Length - 1] + 1 && totaltime >= 0)
            totaltime += Time.deltaTime * waterspeed * fertilizedspeed;
        ChangeSpeed();
        Growth();
        this_sp.sprite = sp[stage];
    }

    void Growth()
    {
        if (stage < grewtime.Length && totaltime >= grewtime[stage])
            stage++;
    }
    public void SetEarth(Earth earth)//跟种植中的土地绑定
    {
        if (earth != null)
            this_earth = earth;
    }
    void ChangeSpeed()//更改生长速度
    {
        if (this_earth == null)
            return;
        if (this_earth.Dry)
        {
            waterspeed = 0.8f;
        }
        else if (this_earth.Wet)
        {
            waterspeed = 1.1f;
        }
        else
            waterspeed = 1.0f;
        if (this_earth.Fertilized)
            fertilizedspeed = 1.2f;
        else
            fertilizedspeed = 1.0f;
    }

    public void Ripe(ref int frunum)//差不多是产生随机数量的果实，然后销毁自身，参数是果实的数量
    {
        if (!Isharvest)
            return;
        Random.InitState((int)System.DateTime.Now.Ticks);
        int num = Random.Range(minfruitnum, maxfruitnum);
        frunum = num;
        for(int i=0;i<num;i++)
        {
            GameObject fru = Instantiate(fruit);
            fru.transform.position = this.transform.position + new Vector3(-0.1f * i, 0, -0.01f);
        }
        Destroy(this.gameObject);
    }
}