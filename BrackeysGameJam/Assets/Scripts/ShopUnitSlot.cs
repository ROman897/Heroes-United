using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUnitSlot : UnitSlot
{
    public override bool try_take_unit() {
        return Shop.singleton().try_spend_money(my_unit.price);
    }
}
