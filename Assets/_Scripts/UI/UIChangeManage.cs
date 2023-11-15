using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChangeManage : MonoBehaviour
{
    [Serializable]
    public class UIClass
    {
        public Transform UI;
        public bool Awake = true;
        public Transform backTo;
    }
    [SerializeField] UIClass[] _ui;
    [SerializeField] GameObject backUI;

    UIClass currentUI;

    private void Awake()
    {
        for (int i = 0; i < _ui.Length; i++)
        {
            var ui = _ui[i];
            if (ui.Awake)
            {
                ui.UI.gameObject.SetActive(true);
                backUI.SetActive(ui.backTo != null);
                currentUI = ui;
            }
            else ui.UI.gameObject.SetActive(false);
        }
        
    }

    public void OpenUIIndex(int index)
    {
        for (int i = 0; i < _ui.Length; i++)
        {
            var ui = _ui[i];
            if(i == index)
            {
                ui.UI.gameObject.SetActive(true);
                backUI.SetActive(ui.backTo != null);
                currentUI = ui;
            }
            else ui.UI.gameObject.SetActive(false);
        }
    }
    public void OpenUIName(string name)
    {
        for (int i = 0; i < _ui.Length; i++)
        {
            var ui = _ui[i];
            if (ui.UI.name.ToLower() == name.ToLower())
            {
                ui.UI.gameObject.SetActive(true);
                backUI.SetActive(ui.backTo != null);
                currentUI = ui;
            }
            else ui.UI.gameObject.SetActive(false);
        }
    }
    public void BackUI()
    {
        OpenUIName(currentUI.backTo.name);
    }
}
