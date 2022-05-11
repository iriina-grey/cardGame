using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtomGroupOnClick : MonoBehaviour
{
    
    public Boolean isFull=false;
    public int cardFullNum=4;
    public int Num;
    public AudioSource buttonDown;
    public void OnClickPlus()
    {
        buttonDown.Play();
        if (!isFull)
        {
            string str = this.transform.parent.transform.Find("NumText").GetComponent<Text>().text;
            
            Num = str[0] - '0' + 1;
            str = "" + Num;
            this.transform.parent.transform.Find("NumText").GetComponent<Text>().text = str;
            isFullController();
        }
    }
    public void OnClickSub()
    {
        buttonDown.Play();
        string str = this.transform.parent.transform.Find("NumText").GetComponent<Text>().text;
        
        Num = str[0] - '0' - 1;
        if (Num < 0)
            Num = 0;
        str = "" + Num;
        this.transform.parent.transform.Find("NumText").GetComponent<Text>().text = str;
        isFullController();


    }
    

    void isFullController()
    {
        int sum=0;
        string str;
        
        GameObject[] buttonObj = GameObject.FindGameObjectsWithTag("EditorNum");
        
        foreach (GameObject Find in buttonObj)
        {
           str=Find.transform.Find("NumText").GetComponent<Text>().text;
            sum += str[0] - '0';
            if(sum>=cardFullNum)
            {
                
                foreach(GameObject sons in buttonObj)
                {
                    sons.transform.Find("Event").GetComponent<ButtomGroupOnClick>().isFull = true;
                }
                break;
            }else
            {
                foreach (GameObject sons in buttonObj)
                {
                    sons.transform.Find("Event").GetComponent<ButtomGroupOnClick>().isFull = false;
                }
            }
        }


    }
    


}
