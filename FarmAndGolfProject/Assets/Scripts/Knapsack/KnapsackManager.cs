using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class KnapsackManager : MonoBehaviour
{
    private static KnapsackManager _instance;
    public static KnapsackManager Instance { get { return _instance; } }


    public GridPanelUI GridPanelUI;
    public TooltipUI TooltipUI;
    public DragItemUI DragItemUI;

    private bool isDrag = false;
    private bool isShow = false;

    private Dictionary<int, Item> ItemList = new Dictionary<int, Item>();


    void Awake()
    {
        //单例
        _instance = this;

        //数据读取
        Load();

        //注册事件
        GridUI.OnEnter += GridUI_OnEnter;
        GridUI.OnExit += GridUI_OnExit;
        GridUI.OnLeftBeginDrag += GridUI_OnLeftBeginDrag;
        GridUI.OnLeftEndDrag += GridUI_OnLeftEndDrag;

        //不销毁
        if (GameObject.FindGameObjectsWithTag("Protagonist").Length > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        //鼠标的坐标转为UI坐标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("KnapsackUI").transform as RectTransform, Input.mousePosition, null, out position);

        //拖拽的时候不显示物品信息
        if (isDrag)
        {
            DragItemUI.Show();
            DragItemUI.SetLocalPosition(position);
        }
        else if (isShow)
        {
            TooltipUI.Show();
            TooltipUI.SetLocalPosition(position);
        }
    }


    //存储物品
    public void StoreItem(int itemID)
    {
        if (!ItemList.ContainsKey(itemID))
            return;

        //取到一个空的格子
        Transform emptyGrid = GridPanelUI.GetEmptyGrid();
        if (emptyGrid == null)
        {
            Debug.Log("格子已满！");
            return;
        }

        //如果背包未满则进行以下处理

        Item newItem = ItemList[itemID];

        CreatNewItem(newItem, emptyGrid);
    }


    //模拟读取数据库的过程
    private void Load()
    {
        ItemList = new Dictionary<int, Item>();

        Weapon w1 = new Weapon(000, "无尽之刃", "据说集齐六把就无人可当了", 300, 200, "", 20);
        Weapon w2 = new Weapon(001, "多兰剑", "经典之作", 300, 200, "", 20);
        Weapon w3 = new Weapon(002, "黑切", "我黑切呢？？？", 300, 200, "", 20);

        Consumable c1 = new Consumable(003, "血瓶", "强迫症出门必带，没钱就等到有钱orz", 200, 120, "", 50, 0);
        Consumable c2 = new Consumable(004, "蓝瓶", "听说被删了？？", 200, 120, "", 0, 50);

        Armor a1 = new Armor(005, "多兰盾", "抗压老哥", 4500, 2000, "", 10, 20, 40);
        Armor a2 = new Armor(006, "沙漏", "这个真的有必要算作防具么？？", 6000, 2000, "", 5, 5, 80);

        ItemList.Add(w1.Id, w1);
        ItemList.Add(w2.Id, w2);
        ItemList.Add(w3.Id, w3);
        ItemList.Add(c1.Id, c1);
        ItemList.Add(c2.Id, c2);
        ItemList.Add(a1.Id, a1);
        ItemList.Add(a2.Id, a2);
    }



    private void GridUI_OnEnter(Transform gridTransform)
    {
        //获取到格子里物品的信息
        Item item = ItemModel.GetItem(gridTransform.name);
        if (item == null)
            return;
        string info = GetTooltipText(item);
        TooltipUI.UpdateTooltip(info);
        isShow = true;
    }

    #region 事件回调
    private void GridUI_OnExit()
    {
        isShow = false;
        TooltipUI.Hide();
    }

    /// <summary>
    /// 1.拖拽时先销毁原来格子里物品
    /// 2.拖拽物品的显示与信息跟新
    /// 3.判定拖拽的结果：
    /// 3.1扔掉物品：直接从背包数据中删除掉
    /// 3.2移动物品：删除原来格子里的物品信息，并在新格子里添加物品
    /// 3.3交换物品：删除交换前后格子里的物品信息，并在交换前后格子里添加物品
    /// 3.4还原物品：在原来格子里添加物品显示
    /// </summary>
    private void GridUI_OnLeftBeginDrag(Transform gridTransform)
    {
        if (gridTransform.childCount == 0)
            return;
        else
        //格子里有物品则
        {
            //获取到格子里的物品
            Item item = ItemModel.GetItem(gridTransform.name);
            if (item == null)
            {
                Debug.LogWarning("出错：格子里有子物体而背包数据中有没有物品！");
                return;
            }

            //更新拖拽物品的显示
            DragItemUI.UpdateItem(item.Name);

            //销毁原来格子里的物品
            Destroy(gridTransform.GetChild(0).gameObject);

            //拖拽开始
            isDrag = true;
        }
    }

    private void GridUI_OnLeftEndDrag(Transform preTransform, Transform enterTransform)
    {
        if (isDrag == false) return;
        //隐藏拖拽物品
        DragItemUI.Hide();

        #region 开始判定拖拽结果

        //1.拖到格子里
        if (enterTransform.tag == "Grid")
        {
            //1.1拖到空格子里：扔进去
            if (enterTransform.childCount == 0)
            {
                //从背包数据中获取到之前格子里的物品信息
                Item item = ItemModel.GetItem(preTransform.name);

                //从背包数据中删除掉原来格子里的物品信息
                ItemModel.DeleteItem(preTransform.name);

                //在结束拖拽的格子里添加之前的物品
                CreatNewItem(item, enterTransform);

                Debug.Log("移动物品");
            }
            //1.2拖到有物品的格子里：交换
            else
            {
                //删除掉结束拖拽的格子里的物品
                Destroy(enterTransform.GetChild(0).gameObject);

                //从背包数据中获取到开始拖拽格子和结束拖拽格子里的物品信息
                Item prevGirdItem = ItemModel.GetItem(preTransform.name);
                Item enterGirdItem = ItemModel.GetItem(enterTransform.name);

                //从背包数据中删除掉开始拖拽格子和结束拖拽格格子里的物品信息
                ItemModel.DeleteItem(preTransform.name);
                ItemModel.DeleteItem(enterTransform.name);

                //添加交换后的两个物体
                this.CreatNewItem(prevGirdItem, enterTransform);
                this.CreatNewItem(enterGirdItem, preTransform);

                Debug.Log("交换");
            }
        }
        //2.拖到面板上：还原拖拽物品
        else if (enterTransform.tag == "GridPanel")
        {
            //从背包数据中获取到之前格子里的物品信息
            Item item = ItemModel.GetItem(preTransform.name);

            //在开始拖拽的格子里添加之前的物品
            CreatNewItem(item, preTransform);

            Debug.Log("还原");
        }
        //3.未拖到物体上：扔东西
        else
        {
            //删除背包中这个物品的信息
            ItemModel.DeleteItem(preTransform.name);

            Debug.Log("物品已扔");
        }
        #endregion

        //拖拽完成
        isDrag = false;
    }
    #endregion

    private string GetTooltipText(Item item)//添加信息栏的信息
    {
        if (item == null)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("<color=red>{0}</color>\n\n", item.Name);
        switch (item.ItemType)
        {
            case ItemType.Armor:
                Armor armor = item as Armor;
                sb.AppendFormat("力量:{0}\n防御:{1}\n敏捷:{2}\n\n", armor.Power, armor.Defend, armor.Agility);
                break;
            case ItemType.Consumable:
                Consumable consumable = item as Consumable;
                sb.AppendFormat("HP:{0}\nMP:{1}\n\n", consumable.BackHp, consumable.BackMp);
                break;
            case ItemType.Weapon:
                Weapon weapon = item as Weapon;
                sb.AppendFormat("攻击:{0}\n\n", weapon.Damage);
                break;
            default:
                break;
        }
        //共有属性
        sb.AppendFormat("<size=25><color=white>购买价格：{0}\n出售价格：{1}</color></size>\n\n<color=yellow><size=20>描述：{2}</size></color>", item.BuyPrice, item.SellPrice, item.Description);
        return sb.ToString();
    }

    /// <summary>
    /// 在背包中创建一个物品（提供一个物品和空格子的信息）
    /// </summary>
    private void CreatNewItem(Item newItem, Transform emptyGrid)
    {
        if (newItem == null)
        {
            Debug.Log("格子里没有物品");
            return;
        }

        //获取到Prefab
        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Item");

        //实例化物品信息面板
        GameObject itemGo = GameObject.Instantiate(itemPrefab);

        //更新显示
        itemGo.GetComponent<ItemUI>().UpdateItem(newItem.Name);

        //设置物品信息的位置
        itemGo.transform.SetParent(emptyGrid);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;

        //向背包数据里添加
        ItemModel.StoreItem(emptyGrid.name, newItem);

    }
}
