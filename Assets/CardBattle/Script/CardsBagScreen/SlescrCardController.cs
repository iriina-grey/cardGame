using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlescrCardController : MonoBehaviour,IPointerClickHandler
{
    
    public int index;

  

    

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (CardBagGenerate.Instance.CardNumReduce(index))
        Destroy(this.gameObject);

        //destroyControler.GetComponent<DestroyControler>().isDestroy = true;
        
    }

    // Start is called before the first frame update

}
