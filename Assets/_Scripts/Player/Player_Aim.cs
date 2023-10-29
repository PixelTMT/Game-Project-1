using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Aim : MonoBehaviour
{
    [SerializeField]
    Player_Control playerControl;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            playerControl.AddEnemyList(collision.collider.transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            playerControl.RemoveEnemyList(collision.collider.transform);
    }
    private void OnTriggerStay(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {

    }
}
