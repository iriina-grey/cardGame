using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour,IPointerClickHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public bool isCardEventNeed;
    Animator animator;
    GameObject ATKimage;
    public GameObject imagePrefab;
    public Transform tempParent;
    public bool dragAllow;

    public int HP;
    public int nowHP;
    public int AP;
    public int AC;
    public int ATK;

    public bool isAllowToAttack;
    public bool isAllowBeAttack;

    public AudioSource AttackVoice;
    CanvasGroup cg;
    RectTransform rt;
    Vector3 newPosition;
    private void Awake()
    {
        isCardEventNeed = false;
        tempParent = GameObject.Find("Canvas").transform;
        animator = this.gameObject.transform.GetComponent<Animator>();
        isAllowToAttack = true;
        isAllowBeAttack = false;
       // StartCoroutine(AnimatorTest());


    }
    void Start()
    {
        nowHP = HP;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCardEventNeed)
        {
            EventManger.Instance.TargetShipData(this.transform.GetSiblingIndex());
            isCardEventNeed = false;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragAllow&&isAllowToAttack)
        {
            ApearEnemy_AllowAttcke(true);
            ATKImageInstantiate();
            AttackVoice.Play();
        }
            

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragAllow& isAllowToAttack)
        {

            RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, null, out newPosition);
            ATKimage.transform.position = newPosition;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject endObj;
        endObj = eventData.pointerEnter;
        ApearEnemy_AllowAttcke(false);
        if (dragAllow)
        {
            if (endObj.transform.tag == "EnemyShip")
            {
                if(isAllowToAttack&& endObj.transform.GetComponentInParent<Ship>().isAllowBeAttack)
                {
                    EventManger.Instance.ATKControl(this.gameObject.transform, endObj.transform.parent);
                    isAllowToAttack = false;
                }
                     
            }

            GameObject.Destroy(ATKimage);

        }
            
    }
    void ATKImageInstantiate()
    {
        ATKimage = GameObject.Instantiate(imagePrefab, this.transform.position, Quaternion.identity, tempParent);
        //监测忽略
        cg = ATKimage.gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        rt = ATKimage.transform.GetComponent<RectTransform>();

    }
    void ApearEnemy_AllowAttcke(bool key)
    {
        GameObject gameObject = GameDateManger.Instance.enmShipsPanel;
        foreach (Transform child in gameObject.transform)
        {
            if(key)
            {
if (child.transform.GetComponent<Ship>().isAllowBeAttack)
            {
                child.Find("SelectImage").transform.gameObject.SetActive(true);
            }
            }
            else
            {
                child.Find("SelectImage").transform.gameObject.SetActive(false);
            }
            
                
        }
    }
    
    /*IEnumerator AnimatorTest()
    {

        animator.SetBool("isUnderATK", true);
        animator.SetBool("isUnderATK", false);
        yield return new WaitForSeconds(2f);
        Debug.Log("1");
        

        yield return new WaitForSeconds(2f);
        Debug.Log("2");
       animator.SetBool("isUnderATK", true);
    }
    */
}
