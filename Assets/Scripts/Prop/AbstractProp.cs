using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AbstractProp : MonoBehaviour
{
    public TMP_Text propStringTMP;
    public string propText = "0";

    public void UpdateUI()
    {
        propStringTMP.text = propText;
    }
}
