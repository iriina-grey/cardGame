using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardBagGenerate : Singleton<CardBagGenerate>
{
    
    public GameObject parentPanel;
    public GameObject cardPrefab;
    public Sprite cardsBg;
    

    public GameObject nowPanel;
    public GameObject linkPanel;

    public GameObject textLable;


    public CardGroupItem CardGroup;

    public GameObject EventCatchArea; 
    public GameObject selectPanel;
    public GameObject selectCardPrefab;
    GameObject cardInsItem;
    //卡牌加载顺序
    int index;

    int[] cardsIndex;

    

    int subNum;

    Transform tempParent;
     SelectCardsDate[] date;


    private void Start()
    {
        index = 0;
        tempParent = GameObject.Find("Canvas").transform;
        CardsGenerate();
        cardsIndex = new int[20];
        
        date = new SelectCardsDate[20];
        for(int i=0; i < date.Length; i++)
        {
            date[i] = new SelectCardsDate();
        }
        subNum = 0;



    }
    void CardsGenerate()
    { 

        foreach(CardItem cardItem in CardGroup.itemList)
        {
            
            cardInsItem = Instantiate(cardPrefab);
            Transform[] gameObjects= cardInsItem.transform.GetComponentsInChildren<Transform>();
            gameObjects[1].GetComponent<Image>().sprite = cardsBg;
            gameObjects[2].GetComponent<Image>().sprite = cardItem.sprite;
            gameObjects[3].GetComponent<Text>().text = cardItem.cardInfo;
            gameObjects[4].GetComponent<Text>().text = cardItem.spend+"";
            cardInsItem.transform.GetComponent<DragTest>().name = cardItem.name;
            cardInsItem.transform.GetComponent<DragTest>().index = index;
            cardInsItem.transform.GetComponent<DragTest>().tempParent = tempParent;
            
            cardInsItem.transform.SetParent(parentPanel.transform);
            
            index++;
        }
    }


   
    private class SelectCardsDate
    {
        public int index = 0;
        public int num = 0;
        public Transform transform = null;
        public SelectCardsDate()
        {
            index = 0;
            num = 0;
            transform = null;
        }
    }
    SelectCardsDate isSimilar(int index)
    {
        foreach(SelectCardsDate dateItem in date)
        {
            if (dateItem.num != 0)
            {
                if (index == dateItem.index)
                    return dateItem;
            }
                
        }
        return null;
    }
    int FoundEnmpty()
    {
        int i=0;
        foreach (SelectCardsDate dateItem in date)
        {
            if (dateItem.num == 0)
                return i;
            i++;
        }
        return 0;
    }
  
   public void SelectCardsDataControl(string name,int index)
    {
        SelectCardsDate dateItem;
        if (subNum<20)//！
        {
            if((dateItem=isSimilar(index))==null)
            {
                //生成新的实例
                GameObject selectCardItem = Instantiate(selectCardPrefab);
                Transform textChild_name = selectCardItem.transform.GetChild(0);
                
                int i=FoundEnmpty();


                textChild_name.transform.GetComponent<Text>().text = name;
                selectCardItem.transform.GetComponent<SlescrCardController>().index = index;
                selectCardItem.transform.SetParent(selectPanel.transform);
                date[i].num = 1;
                date[i].index = index;
                date[i].transform = selectCardItem.transform;
                textLable.transform.GetComponent<Text>().text = (subNum+1) + "/20";
                subNum++;
            }
            else
            {
                if (dateItem.num == 1)
                {
                    dateItem.num = 2;
                    dateItem.transform.GetChild(1).transform.GetComponent<Text>().text = "x" + 2;
                    textLable.transform.GetComponent<Text>().text = (subNum + 1) + "/20";
                    subNum++;
                }
                else
                {

                    Text text= textLable.transform.Find("Text").transform.GetComponent<Text>() ;
                    text.text = "已达到数量限制，请选择其他卡牌";
                    LableAlphaControler.Instance.StarToClear(text);
                }
            }

            //没有
            //则生成新实例
            //保存信息
            
            //有
            //修改信息和实例属性
            
        }
        
    }
    void SwichPanel()
    {
        nowPanel.SetActive(false);
        linkPanel.SetActive(true);
    }
   
    public bool CardNumReduce(int index_F)
    {
        SelectCardsDate dateItem;
        dateItem=isSimilar(index_F);
        dateItem.num--;
        subNum--;
        if (dateItem.num == 0)
        {
            return true;
        }
        else
        {
            dateItem.transform.GetChild(1).transform.GetComponent<Text>().text = "";
            return false;
        }
    }

    public void EnterSelect_OnClick()
    {
        int i = 0;
        
        if (subNum == 20)
        {
            foreach (SelectCardsDate dateItem in date)
            {

                for(int j = dateItem.num; j > 0; j--)
                {
                    cardsIndex[i] = dateItem.index;
                    i++;
                }
                
                
            }
            GameDateManger.Instance.cardsInsIndex = cardsIndex;
            SwichPanel();
        }
        else
        {
            //提示
            Text text = textLable.transform.Find("Text").transform.GetComponent<Text>();
            text.text = "数量不足20张！";
            LableAlphaControler.Instance.StarToClear(text);
        }
    }


    

}
