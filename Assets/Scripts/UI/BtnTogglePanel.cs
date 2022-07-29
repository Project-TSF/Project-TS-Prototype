using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class BtnTogglePanel : MonoBehaviour
{
    [SerializeField] TMP_Text toggleBtnText;
    [SerializeField] GameObject slotPanel;
    bool isOpenPanel = true;
    public bool isAvailable = true;

    async public void TogglePanel()
    {
        if (isAvailable)
        {
            if (isOpenPanel)
            {
                await ClosePanel();
            }
            else
            {
                await OpenPanel();
            }
        }
    }

    public async Task OpenPanel()
    {
        CardManager.Inst.isCardSelectable = false;

        Debug.Log("<<Open Slot Panel>>");
        Tween moveY = slotPanel.transform.DOMoveY(0, 0.2f);

        await moveY.AsyncWaitForCompletion();

        isOpenPanel = true;
        toggleBtnText.text = "Close Panel";
        
        CardManager.Inst.isCardSelectable = true;
    }

    async public Task ClosePanel()
    {
        CardManager.Inst.isCardSelectable = false;

        Debug.Log("<<Close Slot Panel>>");
        Tween moveY = slotPanel.transform.DOMoveY(-30, 0.2f);

        await moveY.AsyncWaitForCompletion();

        isOpenPanel = false;
        toggleBtnText.text = "Open Panel";
        
        CardManager.Inst.isCardSelectable = true;
    }
}
