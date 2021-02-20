using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private static Shop instance;

    private Text money_text;

    [SerializeField]
    private GameObject buyable_unit_prefab;

    public static Shop singleton() {
        if (instance == null) {
            instance = GameObject.FindObjectOfType<Shop>();
            if (instance == null) {
                Debug.LogError("cannot find instance of Shop");
            }
        }
        return instance;
    }

    public bool try_spend_money(int amount) {
        bool ok = LevelManager.singleton().try_spend_money(amount);
        reload_money_text();
        return ok;
    }

    private void reload_money_text() {
        money_text.text = LevelManager.singleton().get_money() + "$";
    }

    void Awake() {
        money_text = GameObject.Find("MoneyText").GetComponent<Text>();
        reload_money_text();
        instance = this;
    }

    private void play_next_level() {
        ShopFormation shop_formation = GameObject.Find("ShopFormation").GetComponent<ShopFormation>();

        LevelManager.singleton().play_next_level(shop_formation.get_units());
    }

    void Start()
    {
        foreach (BuyableUnit buyable_unit in Resources.LoadAll<BuyableUnit>("BuyableUnits")) {
            GameObject buyable_unit_go = GameObject.Instantiate(buyable_unit_prefab, transform);
            buyable_unit_go.GetComponent<UnitSlot>().set_unit(buyable_unit);
            buyable_unit_go.transform.Find("PriceText").GetComponent<Text>().text = buyable_unit.price + "$";
        }
        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(play_next_level);
    }
}
