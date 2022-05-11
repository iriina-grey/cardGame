using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class text : MonoBehaviour
{
public void OnClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
