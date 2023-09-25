using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Aim : MonoBehaviour
{
    [SerializeField]
    Player_Control playerControl;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
            playerControl.AddEnemyList(other.transform);
    }
}
