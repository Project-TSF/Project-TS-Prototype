using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PawnBuffIndicator : MonoBehaviour
{
    [SerializeField] TMP_Text buffStringTMP;
    public string buffText = "0";

    public void UpdateUI()
    {
        buffStringTMP.text = buffText;
    }
}
