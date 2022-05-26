using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class BtnTogglePanel : MonoBehaviour
{
    [SerializeField] TMP_Text toggleBtnText;
    [SerializeField] GameObject slotPanel;
    bool isOpenPanel;

    private void Awake() {
        isOpenPanel = true;
    }

    public void TogglePanel()
    {
        if (isOpenPanel)
        {
            Debug.Log("<<Close Slot Panel>>");
            slotPanel.transform.DOMoveY(-30, 0.2f);

            isOpenPanel = false;
            toggleBtnText.text = "Open Panel";
        }
        else
        {
            Debug.Log("<<Open Slot Panel>>");
            slotPanel.transform.DOMoveY(0, 0.2f);

            isOpenPanel = true;
            toggleBtnText.text = "Close Panel";
        }
    }
}
