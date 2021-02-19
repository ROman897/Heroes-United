using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Unit
{
    public string name;
    public Sprite unitSprite;
    public GameObject unitPrefab;
}
public class UnitSlot : MonoBehaviour
{
    [SerializeField]
    GameObject shopMenu;
    [SerializeField]
    int UnitNumber;
    [SerializeField]
    Image slotGFX;
    [SerializeField]
    Unit[] units;
    bool opened = false;
    public static int UnitUsed = 0;
    [SerializeField]
    UnitSlot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Unit" + UnitNumber))
            PlayerPrefs.SetString("Unit" + UnitNumber, "");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Unit currentUnit = getUnitByName(PlayerPrefs.GetString("Unit" + UnitNumber), units);
        if (currentUnit != null)
        {
            slotGFX.sprite = currentUnit.unitSprite;
            slotGFX.color = new Color(1, 1, 1, 1);
        }
        else
            slotGFX.color = new Color(1, 1, 1, 0);
        GetComponent<Image>().color = UnitUsed == UnitNumber ? Color.grey : Color.white;
    }
    public void openMenu()
    {
        if (UnitUsed != 0 && UnitUsed != UnitNumber)
            slots[UnitUsed - 1].opened = false;
        UnitUsed = opened ? 0 : UnitNumber;
        shopMenu.SetActive(!opened);
        opened = !opened;
    }
    Unit getUnitByName(string name, Unit[] unitArray)
    {
        foreach(Unit unit in unitArray)
        {
            if (unit.name == name)
            {
                return unit;
            }
        }
        return null;
    }
}
