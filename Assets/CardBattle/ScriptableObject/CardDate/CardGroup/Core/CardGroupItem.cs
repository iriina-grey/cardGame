using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardGroup", menuName = "CardData/CardGroup")]
public class CardGroupItem : ScriptableObject
{
    public List<CardItem> itemList = new List<CardItem>();
}
