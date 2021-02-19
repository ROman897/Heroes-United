using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShop : MonoBehaviour
{
    public float prize;
    public int unitNumber;
    public string unit;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("Money", 200);
        unitNumber = UnitSlot.UnitUsed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buyUnit()
    {
        if (PlayerPrefs.GetFloat("Money") >= prize && PlayerPrefs.GetString("Unit" + unitNumber) != unit)
        {
            Debug.Log("buy succesfull");
            PlayerPrefs.SetFloat("Money", PlayerPrefs.GetFloat("Money") - prize);
            PlayerPrefs.SetString("Unit" + unitNumber, unit);
        }
        else if (PlayerPrefs.GetFloat("Money") < prize)
            Debug.Log("not enough money");
        else
            Debug.Log("already bought");
    }
}
