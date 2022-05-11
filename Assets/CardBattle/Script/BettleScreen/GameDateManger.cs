using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDateManger : Singleton<GameDateManger>
{
    //卡牌数据
    //资源文件
    public List<CardItem> battleCardList;
    public CardGroupItem cardGroup;
    
    //所需卡牌索引
    public int[] cardsInsIndex;


    //舰船信息
    public List<ShipItem> dataOfPlayerShip;
    public ShipItemGroup shipGroup;

    //
    public int[] shipsInsIndex;
    //UI界面
    public GameObject playerCardsPanel;
    public GameObject playerShipsPanel;
    public GameObject enmCarsPanel;
    public GameObject enmShipsPanel;
    public GameObject roundButton;
    public GameObject playerPowerValueText;
    public GameObject enmPowerValueText;



    //动态UI单位
    public List<GameObject> hand;
    public List<GameObject> playerShip;
    public List<GameObject> enmShip;
    public List<Transform> cardPositions;
    public List<Transform> enemyPositions;
    public GameObject BoomAnim;
    public GameObject winPanle;
    //预制体
    public GameObject boltPrefab;
    public GameObject hurtText;

    //辅助
    public GameObject CardsCancelPanel;
    public Transform CardsPrepreaArea;
    public Transform CardsGraveyard;
    public GameObject EventControlArea;
    
    private void Awake()
    {
        CardsCancelPanel.SetActive(false);
        battleCardList = cardGroup.itemList;
       dataOfPlayerShip= shipGroup.list;
        foreach ( Transform item in playerCardsPanel.transform)
        {
            cardPositions.Add(item);
        }
        foreach(Transform item in enmCarsPanel.transform)
        {
            enemyPositions.Add(item);
        }
        
        
    }

}
