using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text speedTMP;

    public GameObject slot;
    public CardData cardData;
    public PRS originPRS;

    public void Setup(CardData cardData)
    {
        this.cardData = cardData;

        this.cardData.speed = Random.Range(0, 10);

        UpdateUI();
    }

    #region Movement/Visual

    public void UpdateUI()
    {
        nameTMP.text = this.cardData.cardName;
        speedTMP.text = this.cardData.speed.ToString();
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    public void setVisible(bool visibility)
    {
        transform.gameObject.SetActive(visibility);
    }


    #region CardMouseControl
    private void OnMouseOver()
    {
        CardManager.Inst.CardMouseOver(this);
    }

    private void OnMouseExit()
    {
        CardManager.Inst.CardMouseExit(this);
    }

    private void OnMouseDown()
    {
        CardManager.Inst.CardMouseDown(this);
    }

    private void OnMouseUp()
    {
        CardManager.Inst.CardMouseUp(this);
    }

    private void OnMouseDrag() {
        CardManager.Inst.CardMouseDrag(this);
    }

    #endregion

    #endregion


    #region Effect

    public void UseEffect()
    {
        cardData.UseEffect();
        BattleManager.Inst.UpdateUI();
    }

    #endregion
}
