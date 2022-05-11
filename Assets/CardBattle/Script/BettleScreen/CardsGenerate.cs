using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardsGenerate : Singleton<CardsGenerate>
{ 
    static int startIndex;
    static int startIndex_Enemy;

    public GameObject cardPrefab;
    public Transform playerCardPanel;
    public Transform cardsPrepareArea;
    
    public Transform cardsPrepareArea_Enemy;


    public List<Transform> cardsList;
    public List<Transform> cardsList_Enemy;

     List<Transform> hand;
    
    
    int[] randomIndex= { 0,1,2
    ,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19};
    int[] randomIndex_Enemy = { 0,1,2
    ,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19};
    // Start is called before the first frame update
    void Awake()
    {
        //洗牌
        ShuffleCards();
        randomIndex_Enemy = randomIndex;
        ShuffleCards();
        

    }
  
    private void Start()
    {
        startIndex = -1;
        startIndex_Enemy = -1;
        CardsIns();
        ShipsIns();
    }
    //洗牌
    void ShuffleCards()
    {
        for (int i = 0; i < randomIndex.Length; i++)
        {
            int randomNum;
            randomNum = Random.Range(0, 20);
            int item = randomIndex[randomNum];
            randomIndex[randomNum] = randomIndex[i];
            randomIndex[i] = item;
        }

    }
    //生成卡牌实例并储存
    void CardsIns()
    {
        
        int[] cardsInedex = GameDateManger.Instance.cardsInsIndex;
        
        for(int i = 0; i < cardsInedex.Length; i++)
        {
            
            CardItem cardItem = GameDateManger.Instance.battleCardList[cardsInedex[i]];
            GameObject cardGameObjectItem;
            GameObject cardGameObjectItem_Enemy;
            cardGameObjectItem = GameObject.Instantiate(cardPrefab, cardsPrepareArea.transform.position, Quaternion.identity, cardsPrepareArea.transform);
            cardGameObjectItem_Enemy = GameObject.Instantiate(cardPrefab, cardsPrepareArea_Enemy.transform.position, Quaternion.identity, cardsPrepareArea_Enemy.transform);
            //设置卡牌的值
            SetCardValue(cardGameObjectItem,cardItem);
            SetCardValue(cardGameObjectItem_Enemy,cardItem);

            //Debug.Log(cardItem.cardName);

            cardsList.Add(cardGameObjectItem.transform);
            cardsList_Enemy.Add(cardGameObjectItem_Enemy.transform);
        }
       
    }
    void SetCardValue(GameObject cardGameObjectItem,CardItem cardItem)
    {
        cardGameObjectItem.transform.Find("Image").transform.GetComponent<Image>().sprite = cardItem.sprite;
        cardGameObjectItem.transform.Find("Text").transform.GetComponent<Text>().text = cardItem.cardInfo;
        cardGameObjectItem.transform.Find("SpendText").transform.GetComponent<Text>().text = ""+cardItem.spend;
        cardGameObjectItem.transform.GetComponent<DragONpic>().id = cardItem.cardID;
        cardGameObjectItem.transform.GetComponent<DragONpic>().isNeedTarget = cardItem.targetType;
        cardGameObjectItem.transform.GetComponent<DragONpic>().Spend = cardItem.spend;
        cardGameObjectItem.transform.GetComponent<DragONpic>().enemyUsing = cardItem.enemyUsing;
    }
    
    void ShipsIns()
    {
        int[] shipIndex_Player;
        List<ShipItem> shipItems;
        List<Transform> shipIns_Player;
        List<Transform> shipIns_Enemy;
        shipItems = GameDateManger.Instance.dataOfPlayerShip;
        shipIndex_Player = GameDateManger.Instance.shipsInsIndex;
        shipIns_Player = GetInsChild(GameDateManger.Instance.playerShipsPanel);
        shipIns_Enemy = GetInsChild(GameDateManger.Instance.enmShipsPanel);
        for (int i=0;i<4;i++)
        {
            
            InstallIns(shipItems[shipIndex_Player[i]], shipIns_Player[i]);
            InstallIns(shipItems[shipIndex_Player[i]], shipIns_Enemy[i]);
        }

    }
    //
    void InstallIns(ShipItem shipItem,Transform shipIns)
    {
        Ship ship= shipIns.transform.GetComponent<Ship>();
        ship.HP = shipItem.HP;
        ship.AP = shipItem.Alpha;
        ship.ATK = shipItem.ATK;

        shipIns.transform.Find("ShipImage").transform.GetComponent<Image>().sprite = shipItem.sprite;
        shipIns.transform.Find("Name").transform.GetComponent<Text>().text = shipItem.name;
        shipIns.transform.Find("HPText").transform.GetComponent<Text>().text = ""+shipItem.HP;
        shipIns.transform.Find("APText").transform.GetComponent<Text>().text = "" + shipItem.Alpha;
        shipIns.transform.Find("ATKText").transform.GetComponent<Text>().text = "" + shipItem.ATK;

    }
    //
    List<Transform> GetInsChild(GameObject gameObject)
    {
        List<Transform> items = new List<Transform>();
        foreach(Transform item in gameObject.transform)
        {
            items.Add(item);
        }
        return items;
    }
    //随机敌人
    void RandEnemy()
    {

        
    }
    //摸牌
    public List<Transform> GetCards(bool isEnemy)
    {
        int cardsNum = 4;//一次摸牌的数量
        if (isEnemy)
        {
            hand = new List<Transform>();
            while (startIndex_Enemy < 19 && cardsNum > 0)
            {
                startIndex_Enemy += 1;
                hand.Add(cardsList_Enemy[randomIndex_Enemy[startIndex_Enemy]]);
                cardsNum--;

            }
            if (startIndex_Enemy == 19)
            {
                startIndex_Enemy = -1;
            }
        }
        else
        {
            hand = new List<Transform>();
            while (startIndex < 19 && cardsNum > 0)
            {
                startIndex += 1;
                hand.Add(cardsList[randomIndex[startIndex]]);
                cardsNum--;

            }
            
            if (startIndex == 19)
            {
                ShuffleCards();
                
                startIndex = -1;
            }
        }
        
        return hand;
    }
    //
    
    //设置舰船实例
}
