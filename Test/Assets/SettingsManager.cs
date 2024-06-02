using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    public GameObject SettingsUI;

    private bool _SettingsInput = false;
    public bool SettingsInput
    {
        get
        {
            return _SettingsInput;
        }
        private set
        {
            _SettingsInput = value;
        }
    }

    public void Update()
    {
        S_Input();
        if (SettingsInput)
        {
            OnPress();
        }
        else
        {
            SettingsUI.SetActive(false);

        }

    }

    public void OnPress()
    {
        SettingsUI.SetActive(true);
        if (SettingsInput)
        {
           SettingsInput = false;
        }
        
        
    }

    private void S_Input()
    {
        SettingsInput = Input.GetKeyDown(KeyCode.Escape);
        Debug.Log("Pressed");
        return;
    }
}
