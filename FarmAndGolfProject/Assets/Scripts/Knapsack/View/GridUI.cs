using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class GridUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Enter&&Exit
    public static event Action<Transform> OnEnter;
    public static event Action OnExit;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.tag == "Grid")
        {
            if (OnEnter != null)
                OnEnter(transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter.tag == "Grid")
        {
            if (OnExit != null)
                OnExit();
        }
    }
    #endregion


    #region Drag
    public static event Action<Transform> OnLeftBeginDrag;
    public static event Action<Transform, Transform> OnLeftEndDrag;//第一个参数为拖拽物品的起始格子，第二个参数为目标格子

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            if (OnLeftBeginDrag != null)
                OnLeftBeginDrag(transform);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (OnLeftEndDrag != null)
            {
                if (eventData.pointerEnter == null)
                    OnLeftEndDrag(transform, null);
                else
                    OnLeftEndDrag(transform, eventData.pointerEnter.transform);
            }
        }
    }
    #endregion
}
