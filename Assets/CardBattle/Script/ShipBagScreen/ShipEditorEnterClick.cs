using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipEditorEnterClick : MonoBehaviour
{
    public ShipItemGroup itemGroup;
    public GameObject parentPanel;
    public GameObject linkPanel;
    public GameObject TagLable;
    
    private string NumNotEnoughStr = "舰队数量不足，需要4支舰队";

    public void OnClick()
    {
        
        ShipDataEditor();

        //ShipDataEditor();
    }
    void ShipDataEditor()
    {
        int sum = 0;
        int type = 0;
        string str;
        
        CardData.shipList = new List<ShipItem>();

        GameObject[] buttonObj = GameObject.FindGameObjectsWithTag("EditorNum");
        
        int[] indexItem = new int[4];
        int num = 0;
        foreach (GameObject Find in buttonObj)
        {


            str = Find.transform.Find("NumText").GetComponent<Text>().text;
            for (int i = str[0] - '0'; i > 0; i--)
            {
                indexItem[num] = type;
                
                num++;
            }
            
            sum += str[0] - '0';
            type++;

        }

        if (sum < 4)
        {
            TagLable.GetComponent<Text>().text= NumNotEnoughStr;
            //提醒Lable
            LableAlphaControler.Instance.StarToClear(TagLable.GetComponent<Text>());
        }
        else
        {

            GameDateManger.Instance.shipsInsIndex = indexItem;
            SwichPanel();
        }


        //foreach (ShipItem shipItem in CardData.shipList)
        //{
        //    Debug.Log(shipItem.name);
        //}
    }
    void SwichPanel()
    {
        parentPanel.SetActive(false);
        linkPanel.SetActive(true);
    }

}

