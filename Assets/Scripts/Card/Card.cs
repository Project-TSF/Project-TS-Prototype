using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

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
        //TODO: 이거 카드 모션 유형별로 따로 움직이는 코드 만들기 ex) 원래 위치로 돌아가는 메서드, 슬롯 안착 메서드, 덱으로 이동 메서드 등등
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

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
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

    async public Task UseEffect()
    {
        Sequence seq = DOTween.Sequence()
            .Append(transform.DOMove(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.OutQuad))
            .AppendCallback(() => cardData.UseEffect())
            .AppendInterval(0.5f)
            .Append(transform.DOMove(GameObject.Find("DiscardDeckPos").transform.position, 0.5f).SetEase(Ease.InQuad))
            .OnComplete(() => CardManager.Inst.DiscardCard(this) );

        await seq.AsyncWaitForCompletion();
        BattleManager.Inst.UpdateUI();
    }

    #endregion
}
