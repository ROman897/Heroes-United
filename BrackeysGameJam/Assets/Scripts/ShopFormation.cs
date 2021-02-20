using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopFormation : MonoBehaviour
{
    [SerializeField]
    private Vector2Int formation_size;

    [SerializeField]
    private GameObject unit_slot_prefab;

    private UnitSlot[][] slots;

    void Start()
    {
        Dictionary<Vector2Int, BuyableUnit> remaining_heroes = LevelManager.singleton().get_remaining_heroes(); 

        slots = new UnitSlot[formation_size.x][];

        for (int y = 0; y < formation_size.y; ++y) {
            slots[y] = new UnitSlot[formation_size.x];
            for (int x = 0; x < formation_size.x; ++x) {
                BuyableUnit buyable_unit;
                slots[y][x] = GameObject.Instantiate(unit_slot_prefab, transform).GetComponent<UnitSlot>();

                if (remaining_heroes.TryGetValue(new Vector2Int(x, y), out buyable_unit)) {
                    slots[y][x].set_unit(buyable_unit);
                }
            }
        }
    }

    public Dictionary<Vector2Int, BuyableUnit> get_units() {
        Dictionary<Vector2Int, BuyableUnit> res = new Dictionary<Vector2Int, BuyableUnit>();

        for (int y = 0; y < formation_size.y; ++y) {
            for (int x = 0; x < formation_size.x; ++x) {
                BuyableUnit buyable_unit = slots[y][x].get_unit();
                if (buyable_unit != null) {
                    res[new Vector2Int(x, y)] = buyable_unit; 
                }
            }
        }

        return res;
    }

    void Update()
    {
        
    }
}
