using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_Animation_Controller : MonoBehaviour
{
    public bool isBusy = false;
    public void Busy(string isBusy)
    {
        this.isBusy = Convert.ToBoolean(isBusy);
    }
    public void ShotRock()
    {
        Debug.Log("ShotRock");
        transform.parent.GetComponent<Enemy2_Controller>().ThrowRock();
    }
}
