using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public bool isCardEventNeed;
    Animator animator;
    GameObject ATKimage;
    public GameObject imagePrefab;
    public Transform tempParent;

    public int HP;
    public int nowHP;
    public int AP;
    public int AC;
    public int ATK;
}
