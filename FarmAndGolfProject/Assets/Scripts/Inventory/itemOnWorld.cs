using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//挂载在"直接放在场景中的物品上",角色可以通过碰撞拾取
public class itemOnWorld : MonoBehaviour
{
    public Item thisItem;//挂在图片上,获取自身是哪个"物品"
    public Inventory playerInventory;//获取自己该去哪个"背包"
    public TipsUI tips;//更新提示用

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tips.Show();
            tips.UpdateTooltip("按空格键拾取");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddNewItem();//添加物品到背包
                Destroy(gameObject);//销毁"物品"->销毁场景中的这个能看到的物品
                tips.Hide();//隐藏提示栏
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tips.UpdateTooltip("按空格键拾取");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddNewItem();//添加物品到背包
                Destroy(gameObject);//销毁"物品"->销毁场景中的这个能看到的物品
                tips.Hide();//隐藏提示栏
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tips.Hide();//隐藏提示栏
        }
    }

    public void AddNewItem()
    {
        if (!playerInventory.itemList.Contains(thisItem))//如果背包中没这个
        {
            for (int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)//找空格子,有就给他的物品赋值
                {
                    playerInventory.itemList[i] = thisItem;//赋值
                    thisItem.itemHeld = 1;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;//如果原本就有,则增加"这个物品"的数量
        }

        InventoryManager.RefreshItem();
    }
}

