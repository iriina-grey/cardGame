using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragTest : MonoBehaviour, IDragHandler, IEndDragHandler,IBeginDragHandler
{
 
    //属性输入
    
    public int index;
    public string name;
    public AudioSource selectCardCreate;
    public AudioSource move_Voice;
    //卡片容器
    GameObject objectItem;
    
    public Transform tempParent;//画布

    CanvasGroup cg;//用于射线监测忽略

    //拖动卡牌跟随
    RectTransform rt;
    Vector3 newPosition;

    GameObject EventArea;

    void Awake()
    {
        EventArea = CardBagGenerate.Instance.EventCatchArea;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {


        DragCardInstantiate();
    }

    public void OnDrag(PointerEventData eventData)
    {
        CardItemFollow();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SelectBagEdior(eventData);
    }
    void CardItemFollow()
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, null, out newPosition);
        objectItem.transform.position = newPosition;
    }
    void DragCardInstantiate()
    {
        move_Voice.Play();
        objectItem = GameObject.Instantiate(this.gameObject, this.transform.position, Quaternion.identity, tempParent);
        //监测忽略
        cg = objectItem.gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        rt = objectItem.transform.GetComponent<RectTransform>();

        EventArea.SetActive(true);

        
    }
    void SelectBagEdior(PointerEventData eventData)
    {
        GameObject endObj;

        EventArea.SetActive(false);

        endObj = eventData.pointerEnter;
        
        if (endObj.transform.tag == "CardEditorBag")
        {
            selectCardCreate.Play();
                CardBagGenerate.Instance.SelectCardsDataControl(name, index);
            
            
        }
        GameObject.Destroy(objectItem);

    }
}
