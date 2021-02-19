using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuyableUnit", menuName = "BuyableUnit")]
public class BuyableUnit : ScriptableObject
{
    public int price;
    public Sprite icon;
    public GameObject linked_prefab;
}
