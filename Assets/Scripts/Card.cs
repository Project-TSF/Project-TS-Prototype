using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card;
    [SerializeField] TMP_Text nameTMP;

    public CardData cardData;
    public PRS originPRS;

    public void Setup(CardData cardData)
    {
        this.cardData = cardData;

        UpdateTMP();
    }

    public void UpdateTMP()
    {
        nameTMP.text = this.cardData.cardName;
        
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

    public void makeVisible(bool visibility)
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

}
