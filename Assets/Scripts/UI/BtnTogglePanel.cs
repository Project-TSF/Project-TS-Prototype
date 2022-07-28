using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class BtnTogglePanel : MonoBehaviour
{
    [SerializeField] TMP_Text toggleBtnText;
    [SerializeField] GameObject slotPanel;
    bool isOpenPanel = true;
    public bool isAvailable = true;

    public void TogglePanel()
    {
        if (isAvailable)
        {
            if (isOpenPanel)
            {
                ClosePanel();
            }
            else
            {
                OpenPanel();
            }
        }
    }

    public void OpenPanel()
    {
        Debug.Log("<<Open Slot Panel>>");
        slotPanel.transform.DOMoveY(0, 0.2f);

        isOpenPanel = true;
        toggleBtnText.text = "Close Panel";
    }

    public void ClosePanel()
    {
        Debug.Log("<<Close Slot Panel>>");
        slotPanel.transform.DOMoveY(-30, 0.2f);

        isOpenPanel = false;
        toggleBtnText.text = "Open Panel";
        
    }
}
