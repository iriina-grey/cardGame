using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsFollowController : MonoBehaviour
{
    
    RectTransform rt;
    Vector3 newPosition;

    private void Awake()
    {
        
        rt = this.GetComponent<RectTransform>();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition,null, out newPosition);
        transform.position = newPosition;

    }

   
}
