using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LableAlphaControler : Singleton<LableAlphaControler>
{
    

    

    #region 字段和属性的定义

    //渐变的速率
    public float floatColorChangeSpeed = 0.6f;
    //RawImage对象
    
    //RawImage组件
    private Text goRawText;
    private Color textColor;

    private bool _isSceneToClear;
    


    #endregion

    void Awake()
    {
        

    _isSceneToClear=false;
}
      void Update()
    {
        if (_isSceneToClear)
        {
            SceneToClear();
        }
    }
   

    /// <summary>
    /// 屏幕逐渐清晰(淡出)
    /// </summary>
    private void FadeToClear()
    {
        //插值运算
       goRawText.color = Color.Lerp(goRawText.color, Color.clear, floatColorChangeSpeed * Time.deltaTime);
    }

    public void StarToClear(Text text)
    {
        goRawText = text;

        goRawText.enabled = true;
        goRawText.color = Color.white;
        _isSceneToClear = true;

    }

    /// <summary>
    /// 屏幕的淡出
    /// </summary>
    private void SceneToClear()
    {
        FadeToClear();
        //当我们的a值小于等于0.05f的时候 就相当于完全透明了
        if (goRawText.color.a <= 0.05f)
        {
            //设置为完全透明
            goRawText.color = Color.clear;
            //组件的开关设置为关闭的状态
            goRawText.enabled = false;
           
            //布尔条件设置为false
            _isSceneToClear = false;
        }
    }

}
