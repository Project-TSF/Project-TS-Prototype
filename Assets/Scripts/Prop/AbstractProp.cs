using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public abstract class AbstractProp : MonoBehaviour
{
    public string ID;
    public abstract string propName {get;set;}
    public abstract string propDescription {get;set;}
    public abstract string ImgPath {get;set;}

    [Header("UI Elements")]
    public Sprite Img;
    public SpriteRenderer ImgRenderer;

    private TMP_Text propStringTMP;
    public string propText = "";

    private void Awake() {
        propStringTMP = transform.Find("PropStringTMP").GetComponent<TMP_Text>();
    }

    public void UpdateText()
    {
        propStringTMP.text = propText;
    }


    // Events

    private void OnEnable() {
        BattleManager.OnTurnStartEvent += OnTurnStart;
        BattleManager.OnTurnEndEvent += OnTurnEnd;
    }

    private void OnDisable() {
        BattleManager.OnTurnStartEvent += OnTurnStart;
        BattleManager.OnTurnEndEvent += OnTurnEnd;
    }

    public virtual void OnEquip() {}
    public virtual void OnUnequip() {}
    public virtual void OnTurnStart() {}
    public virtual void OnTurnEnd() {}
}
