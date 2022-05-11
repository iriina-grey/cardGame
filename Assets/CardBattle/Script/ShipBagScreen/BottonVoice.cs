using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottonVoice : MonoBehaviour
{
    public AudioSource buttonDown;

    public void OnClick()
    {
        buttonDown.Play();
    }
}
