using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EventManger : Singleton<EventManger>
{
    int cardIndex;
    string cardId;
    int targetIndex;
    int powerValue;
    Boolean isUsingEvent;
    Boolean isSureToDo;
    Boolean isTargetNeed;
    Boolean isEnemyUsing;

    Transform  nowCard;
    Transform target;

    List<EventBag> startEventBag;
    List<EventBag> startEventBag_Enemy;
    List<EventBag> endEventBag;
    List<EventBag> endEventBag_Enemy;

    Transform tempyform;
    GameObject cardItem;
    Transform canvas;
    private void Awake()
    {
         isUsingEvent = false;

         isSureToDo = false;
         isTargetNeed = false;
        startEventBag = new List<EventBag>();
        endEventBag= new List<EventBag>();
        endEventBag_Enemy = new List<EventBag>();
        startEventBag_Enemy = new List<EventBag>();
        tempyform = GameDateManger.Instance.playerShipsPanel.transform.parent;
        canvas = GameObject.Find("Canvas").transform;

    }
    void Update()
    {
         CardEventControl();
    }
    //事件判断主程序
    void CardEventControl()
    {
        //判断是否在使用卡牌
        if (isUsingEvent)
        {
            //遮罩UI，禁止对其他卡牌进行操作
            EnableControler();
            int value;
            if ((value=powerValue - nowCard.transform.GetComponent<DragONpic>().Spend) < 0)
            {
                Debug.Log(value);
                RebootManerge();
                

            }
            else
            {

                //判断卡牌是否是单个目标类型
                if (!isTargetNeed)
                {

                    //状态复原
                    isUsingEvent = false;
                    //卡牌处理
                    //墓地移动
                    MovetoCardGraveyard();
                    //卡牌效果
                    CardEvent_NoTarget(cardId,isEnemyUsing);
                    //状态复原

                    isTargetNeed = false;
                    powerValue = value;
                    GameDateManger.Instance.playerPowerValueText.transform.GetComponent<Text>().text = "" + powerValue;
                    //去除遮罩允许操作
                    AbleContrle();



                }
                //需要选择某个单位的情况
                else
                {
                    //移入墓地
                    MovetoCardGraveyard();
                    //判断是否取消
                    if (Input.GetMouseButtonDown(1))
                    {
                        //取消并还原卡牌
                        RebootManerge();

                    }
                    if (isEnemyUsing)
                    {
                        foreach (Transform child in GameDateManger.Instance.enmCarsPanel.transform)
                        {
                            //允许ship实例进行交互
                            child.transform.GetComponent<Ship>().isCardEventNeed = true;
                        }
                        GameDateManger.Instance.EventControlArea.SetActive(true);
                        GameDateManger.Instance.EventControlArea.transform.GetComponent<Image>().color=new Color(0f,0f,0f,0.5f);
                        GameDateManger.Instance.enmShipsPanel.transform.SetParent(GameDateManger.Instance.EventControlArea.transform);
                    }
                    else
                    {
                        foreach (Transform child in GameDateManger.Instance.playerShipsPanel.transform)
                        {
                            //允许ship实例进行交互
                            child.transform.GetComponent<Ship>().isCardEventNeed = true;
                        }
                        GameDateManger.Instance.EventControlArea.SetActive(true);
                        GameDateManger.Instance.EventControlArea.transform.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
                        GameDateManger.Instance.playerShipsPanel.transform.SetParent(GameDateManger.Instance.EventControlArea.transform);
                    }
                    AbleContrle();

                    //设置遮罩
                    //卡牌处理

                    //确定单位
                    if (isSureToDo)
                    {   //还原状态
                        isUsingEvent = false;
                        isSureToDo = false;

                        foreach (Transform child in GameDateManger.Instance.playerShipsPanel.transform)
                        {
                            child.transform.GetComponent<Ship>().isCardEventNeed = false;
                        }
                        GameDateManger.Instance.EventControlArea.SetActive(false);
                        GameDateManger.Instance.playerShipsPanel.transform.SetParent(tempyform);
                        foreach (Transform child in GameDateManger.Instance.enmShipsPanel.transform)
                        {
                            //允许ship实例进行交互
                            child.transform.GetComponent<Ship>().isCardEventNeed = false;
                        }
                        GameDateManger.Instance.EventControlArea.SetActive(false);
                        GameDateManger.Instance.enmShipsPanel.transform.SetParent(tempyform);

                        powerValue = value;
                        //卡牌处理——触发效果
                        GameDateManger.Instance.playerPowerValueText.transform.GetComponent<Text>().text = "" + powerValue;
                        CardEvent_NeedTarget(cardId, target, isEnemyUsing);
                        //遮罩去除

                    }
                
            }
        }

        }
    }

   //获得卡牌讯息
    public void GetEventCardMassage(string idStr,int index,bool type,GameObject gameObject)
    {
        this.cardId = idStr;
        this.cardIndex = index;
        this.isUsingEvent = true;
        isTargetNeed = type;
        nowCard = gameObject.transform;
        isEnemyUsing = gameObject.transform.GetComponent<DragONpic>().enemyUsing;
    }
    //遮罩生成
    void EnableControler()
    {
        GameDateManger.Instance.roundButton.transform.GetComponent<Button>().enabled = false;
        GameDateManger.Instance.CardsCancelPanel.SetActive(true);
        GameDateManger.Instance.CardsCancelPanel.GetComponent<Image>().color = new Color(1, 1,1, 0); 
        //更换遮罩的图片
        //GameDateManger.Instance.CardsCancelPanel.transform.GetComponent<Image>().sprite =;

    }
    //遮罩去除
    void AbleContrle()
    {
        GameDateManger.Instance.roundButton.transform.GetComponent<Button>().enabled = true;
        GameDateManger.Instance.CardsCancelPanel.SetActive(false);
        GameDateManger.Instance.CardsCancelPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        //更换遮罩的图片
        //GameDateManger.Instance.CardsCancelPanel.transform.GetComponent<Image>().sprite =;
    }
    //单体卡牌效果
    void CardsEvent(string idStr,int cardIndex,Transform target)
    {
        

    }


    void ValueChange_NoTarget(char type,int num,Transform panel)
    {
        foreach(Transform child in panel)
        {
            ValueChange_TargetNeed(child, type, num);
        }
    }
    void ValueChange_NoTarget(char type, int num, bool usingWithEnemy)
    {
        if (usingWithEnemy)
        {
            foreach (Transform child in GameDateManger.Instance.enmShipsPanel.transform)
            {
                Transform transform = child;
                ValueChange_TargetNeed(transform, type, num);
            }
        }
        else
        {
            foreach (Transform child in GameDateManger.Instance.playerShipsPanel.transform)
            {
                ValueChange_TargetNeed(child, type, num);
            }

        }
    }

    void ValueChange_TargetNeed(Transform target_F,char type,int num)
    {
        switch (type)
        {
            case 'H':
                
                
                    if ((target_F.transform.GetComponent<Ship>().nowHP += num) > target_F.transform.GetComponent<Ship>().HP)
                    target_F.transform.GetComponent<Ship>().nowHP = target_F.transform.GetComponent<Ship>().HP;
                target_F.transform.Find("HPText").GetComponent<Text>().text = "" + target_F.transform.GetComponent<Ship>().nowHP;
                HurtText(target_F.transform.Find("HPText").transform.position, num, target_F.transform.Find("HPText").GetComponent<Text>().color);
                break;
            case 'P':
                
                target_F.transform.GetComponent<Ship>().AP += num;
                if (target_F.transform.GetComponent<Ship>().AP <= 0)
                {
                    target_F.transform.GetComponent<Ship>().AP = 0;
                    target_F.transform.GetComponent<Ship>().isAllowBeAttack = true;

                }else
                {
                    target_F.transform.GetComponent<Ship>().isAllowBeAttack = false;
                }
                target_F.transform.Find("APText").GetComponent<Text>().text = "" + target_F.transform.GetComponent<Ship>().AP;
                HurtText(target_F.transform.Find("APText").transform.position, num, target_F.transform.Find("APText").GetComponent<Text>().color);
                break;

            case 'A':
                target_F.transform.GetComponent<Ship>().ATK += num;
                target_F.transform.Find("ATKText").GetComponent<Text>().text = "" + target_F.transform.GetComponent<Ship>().ATK;
                HurtText(target_F.transform.Find("ATKText").transform.position, num, target_F.transform.Find("ATKText").GetComponent<Text>().color);
                break;

        }

    }
    //回合效果事件存储
    void SetRoundEventBag(bool enemyUsing, string str,char type,int value,bool usingWithEnemy,Transform target)
    {
        EventBag eventBag = new EventBag(type, value, usingWithEnemy, target);
        if (enemyUsing)
        {
            if (str == "Start")
                startEventBag_Enemy.Add(eventBag);
            else
                endEventBag_Enemy.Add(eventBag);
        }
        else
        {
            if (str == "Start")
                startEventBag.Add(eventBag);
            else
                endEventBag.Add(eventBag);
        }

        

    }

    //回合效果事件触发
   public void RoundEventControl(string roundState,bool isEnemyDo)
    {
        if (roundState == "Start")
        {
            if (isEnemyDo)
            {
                RoundEventDo(startEventBag_Enemy);
                startEventBag_Enemy = new List<EventBag>();
            }
            else
            {
                RoundEventDo(startEventBag);
                startEventBag = new List<EventBag>();
            }

            
        }
        else
        {
            if (isEnemyDo)
            {
                RoundEventDo(endEventBag_Enemy);
                endEventBag_Enemy = new List<EventBag>();
            }
            else
            {
                RoundEventDo(endEventBag);
                endEventBag = new List<EventBag>();
            }
        }
    }
    void RoundEventDo(List<EventBag> list_F)
    {
        foreach (EventBag bag in list_F)
        {
            if (bag.target == null)
            {
                ValueChange_NoTarget(bag.type, bag.value, bag.usingWithEnemy);
            }
            else
            {
                ValueChange_TargetNeed(bag.target, bag.type, bag.value);
            }
        }
        list_F = new List<EventBag>();
    }
    //攻击效果
    public void ATKControl(Transform thisTransform,Transform target_F)
    {
        Ship thisShip = new Ship();
        Ship targetShip = new Ship();
        thisShip = thisTransform.transform.GetComponent<Ship>();
        
        targetShip = target_F.transform.GetComponent<Ship>();
        
        if ((targetShip.nowHP-=thisShip.ATK) > 0)
        {
            
            StartCoroutine(ATKAnim(thisTransform, target_F,false));//播放攻击动画和受伤动画
        }
        else
        {
            
            target_F.Find("Back").transform.GetComponent<Image>().raycastTarget = false;
            target_F.GetComponent<Ship>().isAllowBeAttack = false;
            StartCoroutine(ATKAnim(thisTransform, target_F,true));//播放攻击动画和受伤动画并判断胜利
            
        }
    }
    //恢复攻击能力
    void ATKAllowReboot()
    {
        foreach(Transform ship in GameDateManger.Instance.playerShipsPanel.transform)
        {
            ship.GetComponent<Ship>().isAllowToAttack = true;
        }
    }
    IEnumerator IsWinOrFail()
    {
        int num_E = GameDateManger.Instance.enmShipsPanel.transform.GetChildCount();
        int num_P = GameDateManger.Instance.playerShipsPanel.transform.GetChildCount();
        
        GameObject EventControlArea = GameDateManger.Instance.EventControlArea;
        if (num_E == 0)
        {
            yield return WinOrFailAnim(true);


            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        else if (num_P==0)
        {
            yield return WinOrFailAnim(false);

            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
    IEnumerator WinOrFailAnim(bool IsWinOrFail)
    {
        if (IsWinOrFail)
        {
            
            GameDateManger.Instance.winPanle.transform.Find("Text").transform.GetComponent<Text>().text = "游戏胜利";
            GameDateManger.Instance.winPanle.SetActive(true);
            
        }
        else
        {

            GameDateManger.Instance.winPanle.transform.Find("Text").transform.GetComponent<Text>().text = "游戏失败";
            GameDateManger.Instance.winPanle.SetActive(true);
            
        }
        yield return new WaitForSeconds(2.5f);

    }
    IEnumerator ATKAnim(Transform ATKObject,Transform target_F,bool isBoom)
    {
        GameObject gameObject = Instantiate(GameDateManger.Instance.boltPrefab, ATKObject.position, Quaternion.identity,canvas);
        TargetRotate(ATKObject, target_F, gameObject.transform);
        while (gameObject.transform.position != target_F.position)
        {
            gameObject.transform.position= Vector3.MoveTowards(gameObject.transform.position, target_F.position, 5f * Time.deltaTime * 50f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        target_F.transform.GetComponent<Animation>().Play("UnderATK");
        target_F.transform.Find("HPText").transform.GetComponent<Text>().text = "" + target_F.transform.GetComponent<Ship>().nowHP;


        HurtText(target_F.transform.Find("HPText").transform.position, -ATKObject.transform.GetComponent<Ship>().ATK, target_F.transform.Find("HPText").transform.GetComponent<Text>().color);
        Destroy(gameObject);
        
        if (isBoom)
        {
            GameObject AnimObject = Instantiate(GameDateManger.Instance.BoomAnim, target_F.position, Quaternion.identity, canvas);
            yield return new WaitForSeconds(3f);
            Destroy(AnimObject);
            
            Destroy(target_F.gameObject);
            yield return new WaitForSeconds(1f);
            StartCoroutine(IsWinOrFail());
            
           
        }
        GameDateManger.Instance.EventControlArea.SetActive(false);
    }
    //伤害数值
    void HurtText(Vector3 position_F,int num,Color color)
    {
        GameObject text = Instantiate(GameDateManger.Instance.hurtText, position_F + new Vector3(0, 5, 0), Quaternion.identity, canvas);
        text.transform.GetComponent<Text>().color = color;
        if (num>0)
        {
            text.transform.GetComponent<Text>().text = "+" + num;
        }
        else
        {
            text.transform.GetComponent<Text>().text = "" + num;
        }
        
        

    }
    //子弹方向旋转
    public void TargetRotate(Transform gameObject1, Transform gameObject2, Transform boltTarget)
    {
        float num;
        float x1;
        float x2;
        float y1;
        float y2;
        x1 = gameObject1.position.x;
        x2 = gameObject2.position.x;
        y1 = gameObject1.position.y;
        y2 = gameObject2.position.y;
        if (y1 < y2)
        {
            num = (x2 - x1) / (y2 - y1);
            boltTarget.transform.Rotate(0, 0, 90f - Mathf.Atan(num) * Mathf.Rad2Deg);
        }
        else if (y1 > y2)
        {
            num = (x2 - x1) / (y2 - y1);
            boltTarget.transform.Rotate(0, 0, 270f-Mathf.Atan(num) * Mathf.Rad2Deg );
        }
        else
        {
            boltTarget.transform.Rotate(0, 0, 90f);
        }

    }
    //移入墓地
    void MovetoCardGraveyard()
    {
        
        nowCard.position = GameDateManger.Instance.CardsGraveyard.transform.position;
        
    }
    //抽牌
   
   
    //取消后恢复卡牌原貌
    void RebootManerge()
    {
        isUsingEvent = false;

        isSureToDo = false;
        isTargetNeed = false;
        nowCard.transform.SetParent(GameDateManger.Instance.playerCardsPanel.transform);
        nowCard.SetSiblingIndex(cardIndex);
        GameDateManger.Instance.EventControlArea.SetActive(false);
        GameDateManger.Instance.playerShipsPanel.transform.SetParent(tempyform);
        GameDateManger.Instance.EventControlArea.SetActive(false);
        GameDateManger.Instance.playerShipsPanel.transform.SetParent(tempyform);
        AbleContrle();
        //Destroy(cardItem.gameObject);
    }
    //选取的目标信息
    public void TargetShipData(int index)
    {
        isSureToDo = true;
        targetIndex = index;
        target = GameDateManger.Instance.playerShipsPanel.transform.GetChild(index);
    }
    //回复能量费用
    public void SetPowerValue()
    {
        powerValue = 10;
        GameDateManger.Instance.playerPowerValueText.transform.GetComponent<Text>().text = "" + powerValue;
    }
    //回合减少ap
    public void RoudAPReduce(bool isEnemy_F)
    {
        if (isEnemy_F)
        {
            SetRoundEventBag(isEnemy_F,"Start", 'P', -2, isEnemy_F, null);
        }
        else
        {
            SetRoundEventBag(isEnemy_F, "Start", 'P', -2, isEnemy_F, null);
            ATKAllowReboot();
        }
        
    }
    //
    class EventBag
    {
        public char type;
        public int value;
        public bool usingWithEnemy;
        public Transform target=null;
        public EventBag(char type,int value,bool usingWithEnemy,Transform target)
        {
            this.type = type;
            this.value = value;
            this.target = target;
            this.usingWithEnemy = usingWithEnemy;
        }
    }

    public void CardEvent_NoTarget(string id,bool enemyUsing)
    {
        switch (id)
        {
            case "A1":
                ValueChange_NoTarget('P', -2, enemyUsing);
                break;
        }
    }
    public void CardEvent_NeedTarget(string id,Transform target, bool enemyUsing)
    {
        switch (id)
        {
            case "B1":
                ValueChange_TargetNeed(target, 'A', 5);
                SetRoundEventBag(enemyUsing,"End", 'A', -5, false, target);
                break;
        }
    }
}
