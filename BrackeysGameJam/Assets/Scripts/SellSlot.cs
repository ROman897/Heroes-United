using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlot : MonoBehaviour, IDropHandler
{
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

        BuyableUnit offered_unit = dragable_unit.linked_slot.get_unit();
        if (offered_unit == null) {
            return;
        }
        if (!dragable_unit.linked_slot.try_take_unit()) {
            return;
        }
        Shop.singleton().add_money((int)(offered_unit.price * 0.5));
    }
}
