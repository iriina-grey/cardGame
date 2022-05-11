using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragONpic : MonoBehaviour,IDragHandler,IEndDragHandler,IPointerClickHandler,IBeginDragHandler
{

    GameObject cancelArea;
    Transform myParent;
    Transform tempParent;
    CanvasGroup cg;
    RectTransform rt;
    Vector3 newPosition;

    public string id;    
    public  bool isNeedTarget;
    public int Spend;
    public bool enemyUsing;

    public AudioSource DragOnVoice;
    

    int index;
    // Start is called before the first frame update
    void Awake()
    {
        cg = this.gameObject.AddComponent<CanvasGroup>();
        rt = this.GetComponent<RectTransform>();
        tempParent = GameObject.Find("Canvas").transform;
        myParent = GameDateManger.Instance.playerCardsPanel.transform;
        cancelArea = GameDateManger.Instance.CardsCancelPanel;       
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void OnBeginDrag(PointerEventData eventData)
    {
        index =this.transform.GetSiblingIndex();
        cancelArea.SetActive(true);
        cg.blocksRaycasts = false;
        DragOnVoice.Play();
        this.transform.SetParent(tempParent);
        
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, eventData.enterEventCamera, out newPosition);
        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CardEvent(eventData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }
    void CardEvent(PointerEventData eventData)
    {
        GameObject target = eventData.pointerEnter;


        if (target.name == "CardCancelAreaPanel")
        {
            this.transform.SetParent(myParent);
            this.transform.SetSiblingIndex(index);
            cancelArea.SetActive(false);
        }
        else
        {
            EventManger.Instance.GetEventCardMassage(id,0,isNeedTarget, this.gameObject);
            

        }
        cg.blocksRaycasts = true;
    }
    
}
