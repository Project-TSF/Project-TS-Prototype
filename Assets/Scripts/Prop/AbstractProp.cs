using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public abstract class AbstractProp : MonoBehaviour
{
    public string ID;
    public string propName;
    public string propDescription;

    public string ImgPath;
    public Sprite Img;
    public SpriteRenderer ImgRenderer;
    

    [Header("UI Elements")]
    public TMP_Text propStringTMP;
    public string propText;

    public void UpdateUI()
    {
        propStringTMP.text = propText;
    }


    // Events

    public virtual void onEquip() {}
}
