using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CardItem",menuName ="CardData/Item")]
public class CardItem : ScriptableObject
{
    public string cardName;
    public string cardID;
    public Sprite sprite;
    public int spend;
    

    public bool targetType;//true 为对己方生效，false 2为对敌方生效

    public bool enemyUsing;

    [TextArea]
    public string cardInfo;


}
