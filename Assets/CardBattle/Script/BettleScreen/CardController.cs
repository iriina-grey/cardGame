using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCard();
        }
    }
    public void GenerateCard()
    {
        //GameObject.Instantiate(cardPrefab).transform.SetParent(this.transform);
        GameObject obj = GameObject.Instantiate(cardPrefab);
        obj.transform.SetParent(this.transform);
        obj.transform.SetSiblingIndex(0);
        int i=this.transform.childCount;
        
        Debug.Log(i);
       

    }
}
