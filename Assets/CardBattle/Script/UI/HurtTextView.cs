using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtTextView : MonoBehaviour
{
    /// <summary>
    /// 滚动速度
    /// </summary>
    private float speed = 8f;

    /// <summary>
    /// 计时器
    /// </summary>
    private float timer = 0f;

    /// <summary>
    /// 销毁时间
    /// </summary>
    private float time = 2.5f;

    private void Update()
    {
        Scroll();
    }

    /// <summary>
    /// 冒泡效果
    /// </summary>
    private void Scroll()
    {
        Color color = this.GetComponent<Text>().color;

        this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        timer += Time.deltaTime;

        

        //字体渐变透明
        if (timer > 1.25f)
        {
            color.a = 1 - timer + 1;
            this.GetComponent<Text>().color = color;
        }
        else
        {
            this.GetComponent<Text>().color = color;
        }
        
        Destroy(gameObject, time);
    }
}
