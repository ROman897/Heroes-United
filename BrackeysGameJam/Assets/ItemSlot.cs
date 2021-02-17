using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject CurrentObject;
    public float Amount;
    public void OnDrop(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            //eventData.pointerDrag = CurrentObject;
            //Amount++;
        }
    }
}
