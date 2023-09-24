using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animation_Controller : MonoBehaviour
{
    public bool isBusy = false;
    public void Busy(string isBusy)
    {
        this.isBusy = Convert.ToBoolean(isBusy);
    }
}
