using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{   //要实现这个,还得挂载一个新组件:CanvasGroup
    public Transform originalParent;
    public Inventory myBag;
    private int currentItemID;//当前物品ID

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;//被拖拽物品原来的父物体
        currentItemID = originalParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);//与父同级,这样就能渲染在最上层了
        transform.position = eventData.position;//物品和鼠标一起动
        GetComponent<CanvasGroup>().blocksRaycasts = false;//关掉手下这个物品遮挡鼠标射线
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;//物品和鼠标一起动
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //拽到UI外则删除物品              !!!最好最好还是要有弹窗确认
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {//删除物品,更新描述为"空",更新背包
            myBag.itemList[currentItemID] = null;
            InventoryManager.UpdateItemInfo("", Resources.Load<Sprite>("Graphics/Others/Transparent"));
            InventoryManager.RefreshItem();
        }

        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")//检测鼠标射线下的物品的名字
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);//避免进入Grid在组中闪烁
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
                var temp = myBag.itemList[currentItemID];
                //把目标物品的ID赋值给当前的物品
                myBag.itemList[currentItemID] = myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;



                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;//重新开启遮挡功能
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                myBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = myBag.itemList[currentItemID];
                //解决自己放自己的问题
                if (eventData.pointerCurrentRaycast.gameObject.transform.gameObject.GetComponent<Slot>().slotID != currentItemID)
                    myBag.itemList[currentItemID] = null;


                GetComponent<CanvasGroup>().blocksRaycasts = true;//重新开启遮挡功能
                return;
            }
        }
        //其他任何位置都归位
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
