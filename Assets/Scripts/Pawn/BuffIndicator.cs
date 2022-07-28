using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class buffIndicator : MonoBehaviour
{
    [SerializeField] TMP_Text buffStringTMP;
    public string buffText;

    public void UpdateUI()
    {
        buffStringTMP.text = buffText;
    }
}
