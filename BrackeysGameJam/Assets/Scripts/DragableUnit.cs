using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableUnit : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private CanvasGroup canvas_group;
    private RectTransform rect_transform;
    private Canvas canvas;
    public Transform parent;
    private Transform drag_priority_transform;

    private Image image;

    public UnitSlot linked_slot;

    void Awake() {
        canvas_group = GetComponent<CanvasGroup>();
        rect_transform = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        parent = transform.parent;
        drag_priority_transform = canvas.transform.Find("DragPriority");
        image = GetComponent<Image>();
        linked_slot = transform.parent.GetComponent<UnitSlot>();
    }

    public void OnPointerDown(PointerEventData event_data) {
    }

    public void OnPointerUp(PointerEventData event_data) {
        if (event_data.button != PointerEventData.InputButton.Right) {
            return;
        }
        if (event_data.dragging) {
            return;
        }
        // this is click event, can put something here like unit info
    }

    public void OnBeginDrag(PointerEventData event_data) {
        if (event_data.button != PointerEventData.InputButton.Left) {
            return;
        }
        rect_transform.SetParent(drag_priority_transform, true);
        canvas_group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData event_data) {
        if (event_data.button != PointerEventData.InputButton.Left) {
            return;
        }
        rect_transform.anchoredPosition += event_data.delta / canvas.scaleFactor;
    }

    public void reset_position() {
        canvas_group.blocksRaycasts = true;
        rect_transform.SetParent(parent, true);
        rect_transform.anchoredPosition = Vector2.zero; 
    }

    public void OnEndDrag(PointerEventData event_data) {
        if (event_data.button != PointerEventData.InputButton.Left) {
            return;
        }
        reset_position();
    }

    public void change_graphic(Sprite sprite) {
        if (sprite == null) {
            image.enabled = false;
        } else {
            image.sprite = sprite;
            image.enabled = true;
        }
    }
}
