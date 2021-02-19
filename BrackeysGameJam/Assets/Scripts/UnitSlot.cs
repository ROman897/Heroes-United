using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour, IDropHandler
{
    protected BuyableUnit my_unit; 

    private DragableUnit my_dragable_unit;

    void Awake() {
        my_dragable_unit = transform.Find("DragableUnit").GetComponent<DragableUnit>();
    }

    public BuyableUnit get_unit() {
        return my_unit;
    }

    public virtual bool try_take_unit() {
        set_unit(null);
        return true;
    }

    public void set_unit(BuyableUnit buyable_unit) {
        my_unit = buyable_unit;
        if (buyable_unit == null) {
            my_dragable_unit.change_graphic(null);
        } else {
            my_dragable_unit.change_graphic(buyable_unit.icon);
        }
    }

    public void OnDrop(PointerEventData event_data) {
        if (event_data.button != PointerEventData.InputButton.Left) {
            return;
        }
        if (event_data.pointerDrag == null) {
            return;
        }

        DragableUnit dragable_unit = event_data.pointerDrag.GetComponent<DragableUnit>();
        if (dragable_unit == null) {
            return;
        }
        dragable_unit.reset_position();
        if (my_unit != null) {
            return;
        }

        BuyableUnit offered_unit = dragable_unit.linked_slot.get_unit();
        if (offered_unit == null) {
            return;
        }
        if (!dragable_unit.linked_slot.try_take_unit()) {
            return;
        }
        set_unit(offered_unit);
    }
}
