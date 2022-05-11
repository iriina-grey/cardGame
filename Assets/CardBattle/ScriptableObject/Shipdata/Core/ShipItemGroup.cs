using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShipItemGroup", menuName = "ShipData/ShipGroup")]
public class ShipItemGroup : ScriptableObject
{
    public List<ShipItem> list = new List<ShipItem>();
}
