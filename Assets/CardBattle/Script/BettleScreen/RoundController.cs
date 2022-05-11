using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundController : Singleton<RoundController>
{
    public GameObject EventControlArea;
    public Transform prepreaArea_Enm;
    public Transform showCardPosition;
    public Transform GraveyardArea_Enm;
    // Start is called before the first frame update
    RoundState roundState = RoundState.Start;
    //回合开始效果卡牌列表
    List<Transform> hand = new List<Transform>();
    List<Transform> hand_Enemy = new List<Transform>();
    List<Transform> cardsPosition = new List<Transform>();
    List<Transform> enmCardsPosition = new List<Transform>();

    Transform graveyardArea;
    Transform prepreaArea;

    

    
    enum RoundState
    {
        Start,
        End,
        Play,
        EnemAI
    }
    private void Awake()
    {
        EventControlArea.SetActive(false);
        cardsPosition = GameDateManger.Instance.cardPositions;
        graveyardArea = GameDateManger.Instance.CardsGraveyard;
        prepreaArea = GameDateManger.Instance.CardsPrepreaArea;
        enmCardsPosition = GameDateManger.Instance.enemyPositions;
    }
    // Update is called once per frame
    void Update()
    {       
        switch (roundState)
        {
            case RoundState.Play:
                 break;
            case RoundState.Start:
                roundState = RoundState.Play;
                StartCoroutine(RoundStart());
                break;
            
            case RoundState.End:
                roundState = RoundState.Play;
                StartCoroutine(RoundEnd());
                break;
            case RoundState.EnemAI:
                StartCoroutine(EnmAI());
                roundState = RoundState.Play;
                break;
        }
        
    }
    IEnumerator RoundStart()
    {   
        
        
       
        string startStr="回合开始！";
        //回合开始提示
        yield return StartCoroutine(RoundChangeAnim(startStr));
        
        //执行回合开始效果
        yield return new WaitForSeconds(1f);
        EventManger.Instance.RoundEventControl("Start", false);
        yield return new WaitForSeconds(1f);
        //回合减少ap
        EventManger.Instance.RoudAPReduce(false);
        EventManger.Instance.SetPowerValue();
        

        //抽牌
        hand =CardsGenerate.Instance.GetCards(false);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(DrawCards(hand, cardsPosition,false));

        EventControlArea.SetActive(false);

        yield return new WaitForSeconds(1f);
               
        yield return new WaitForSeconds(1f);
        
    }
   
    IEnumerator RoundEnd()
    {

        //执行回合开始效果
        //回合开始提示
        EventControlArea.SetActive(true);
        Debug.Log("回合jieshu");
        //执行卡牌操作
        EventManger.Instance.RoundEventControl("End",false);
        
        //弃牌
        Debug.Log("qipai");
        yield return StartCoroutine(Discard(hand, graveyardArea, prepreaArea));
        
        
        EventControlArea.SetActive(true);
        yield return new WaitForSeconds(1f);
        roundState = RoundState.EnemAI;
        



        yield return new WaitForSeconds(1f);
        
    }

  
    IEnumerator EnmAI()
    {
        int num = 10;
        EventControlArea.SetActive(true);
        string str = "敌方回合";
        //对方回合开始动画
        yield return  StartCoroutine(RoundChangeAnim(str));
        yield return new WaitForSeconds(1f);
        
        hand_Enemy = CardsGenerate.Instance.GetCards(true);
        
        yield return StartCoroutine(DrawCards(hand_Enemy, enmCardsPosition,true));
        yield return new WaitForSeconds(0.5f);
        //回合开始效果执行
        
        EventManger.Instance.RoundEventControl("Start", true);
        GameDateManger.Instance.enmPowerValueText.transform.GetComponent<Text>().text = "" + num;
        EventManger.Instance.RoudAPReduce(true);
        yield return new WaitForSeconds(2f);
        //使用卡牌，判断能量☞
        foreach(Transform card in hand_Enemy)
        {
            DragONpic cardMessage = card.GetComponent<DragONpic>();
            if (num - cardMessage.Spend >= 0)
            {
                GameDateManger.Instance.enmPowerValueText.transform.GetComponent<Text>().text = "" + (num=num - cardMessage.Spend);
                yield return StartCoroutine(ShowCardAnim_Enemy(card)) ;
                    if (cardMessage.isNeedTarget)
                    {
                        if (cardMessage.enemyUsing)
                        {
                            Transform target = GameDateManger.Instance.playerShipsPanel.transform.GetChild(0);
                            EventManger.Instance.CardEvent_NeedTarget(cardMessage.id,target, !cardMessage.enemyUsing);
                        }
                        else
                        {
                            Transform target = GameDateManger.Instance.enmShipsPanel.transform.GetChild(0);
                            EventManger.Instance.CardEvent_NeedTarget(cardMessage.id, target, !cardMessage.enemyUsing);
                        }
                    }
                    else
                    {
                    
                        EventManger.Instance.CardEvent_NoTarget(cardMessage.id, !cardMessage.enemyUsing);
                   
                    }
            }
            else
            {
                continue;
            }
            
        }
        //攻击敌方单位，优先母舰
        foreach(Transform ship in GameDateManger.Instance.enmShipsPanel.transform)
        {
            foreach(Transform ship_Player in GameDateManger.Instance.playerShipsPanel.transform)
            {
                if (ship_Player.GetComponent<Ship>().isAllowBeAttack )
                {
                    EventManger.Instance.ATKControl(ship, ship_Player);
                    yield return new WaitForSeconds(3f);
                    break;
                }
            }
        }
        //回合结束效果执行
        yield return new WaitForSeconds(1f);
        EventManger.Instance.RoundEventControl("End", true);
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(Discard(hand_Enemy,GraveyardArea_Enm,prepreaArea_Enm));
        yield return new WaitForSeconds(1.0f);
        EventControlArea.SetActive(false);
        
        yield return new WaitForSeconds(3f);
        EventControlArea.SetActive(false);
        roundState = RoundState.Start;

        
    }

    IEnumerator DrawCards(List<Transform> hand,List<Transform> position,bool isEnemy)
    {
        int i = 0;
        foreach(Transform handCard in hand)
        {
            if (i == 3)
            {
                yield return StartCoroutine(DrawCardsAnim(handCard, position[i], handCard.transform.GetComponent<Animation>(),isEnemy));
                
            }
            else
            {
                StartCoroutine(DrawCardsAnim(handCard, position[i], handCard.transform.GetComponent<Animation>(),isEnemy));
            }

            i++;
        }
        i = 0;
        if (isEnemy)
        {
            foreach (Transform handCard in hand)
            {
                handCard.transform.SetParent(GameDateManger.Instance.enmCarsPanel.transform);
                handCard.transform.SetSiblingIndex(i);
                i++;
            }
        }
        else
        {
            foreach (Transform handCard in hand)
            {
                handCard.transform.SetParent(GameDateManger.Instance.playerCardsPanel.transform);
                handCard.transform.SetSiblingIndex(i);
                i++;
            }
        }
        
        yield return 0;
    }
    IEnumerator DrawCardsAnim(Transform cardPos,Transform TragetPos,Animation animation,bool isEnemy)
    {
        float speed = 4f;
        while (cardPos.position != TragetPos.position)
        {
            
            cardPos.position = Vector3.MoveTowards(cardPos.position, TragetPos.position, speed * Time.deltaTime*100f);
            yield return new WaitForEndOfFrame();
            if(speed>0.5f)
                  speed -= (0.25f*Time.deltaTime);
            
        }
        if(!isEnemy)
        animation.Play("DrawCardsAnim");
        yield return new WaitForSeconds(2f);

        //cardPos.SetParent(GameDateManger.Instance.playerCardsPanel.transform);
    }
    IEnumerator Discard(List<Transform> hand,Transform Graveyard,Transform PrepreaArea)
    {
        foreach(Transform handCard in hand)
        {
            handCard.transform.SetParent(PrepreaArea);
            StartCoroutine(DiscardAnim(handCard, Graveyard, PrepreaArea ,handCard.transform.GetComponent<Animation>()));

        }

        yield return new WaitForSeconds(1f);
    }
    IEnumerator DiscardAnim(Transform handCard, Transform Graveyard,Transform PrepreaArea, Animation animation)
    {
        float speed = 4.5f;
        
        //animation.Play("Discard");
        while (handCard.position != Graveyard.position)
        {
            
            handCard.position = Vector3.MoveTowards(handCard.position, Graveyard.position, speed * Time.deltaTime * 100f);
            yield return new WaitForEndOfFrame();

            if (speed < 6f)
                speed += 0.25f * Time.deltaTime;

        }
        yield return new WaitForSeconds(1.5f);
        
            handCard.transform.position = PrepreaArea.transform.position;
               
    }
    IEnumerator ShowCardAnim_Enemy(Transform hand)
    {
        float speed = 4f;
        Animation handAnim = hand.GetComponent<Animation>();
        hand.SetParent(prepreaArea_Enm);
        
        handAnim.Play("ShowCard");
        handAnim["ShowCard"].speed = 1;
        
        while (hand.position != showCardPosition.position)
        {

            hand.position = Vector3.MoveTowards(hand.position, showCardPosition.position, speed * Time.deltaTime * 100f);
            yield return new WaitForEndOfFrame();
            if (speed > 0.5f)
                speed -= (0.25f * Time.deltaTime);

        }
        yield return new WaitForSeconds(1f);
        handAnim.Play("ShowCard");
        handAnim["ShowCard"].time = handAnim["ShowCard"].length;
        handAnim["ShowCard"].speed = -1f;
        yield return new WaitForSeconds(0.5f);
        speed = 3f;
        while (hand.position != GraveyardArea_Enm.position)
        {

            hand.position = Vector3.MoveTowards(hand.position, GraveyardArea_Enm.position, speed * Time.deltaTime * 100f);
            yield return new WaitForEndOfFrame();
            if (speed > 0.5f)
                speed -= (0.25f * Time.deltaTime);

        }
        
        yield return new WaitForSeconds(0.1f);
        
        yield return 0;
    }
    IEnumerator RoundChangeAnim(string str)
    {
        EventControlArea.transform.Find("Text").transform.GetComponent<Text>().text = str;
        EventControlArea.SetActive(true);
        EventControlArea.transform.GetComponent<Animation>().Play("RoundChangeAnim");
        yield return new WaitForSeconds(1.5f);
    }

    
    

    public void RoundFinallButton_OnClick()
    {
        roundState = RoundState.End;

    }
}
