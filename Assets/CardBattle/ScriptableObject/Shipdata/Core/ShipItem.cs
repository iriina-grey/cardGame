using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ShipItem",menuName ="ShipData/Ship") ]
public class ShipItem : ScriptableObject
{
    public string shipName;
    public int type;
    public int ATK;
    public int Alpha;
    public int HP;
    public Sprite sprite;
   
}
