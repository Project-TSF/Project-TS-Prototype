using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{

    public void StartFloating(Pawn pawn, string text, Color color, float moveSpeed=1f)
    {
        this.transform.SetParent(pawn.transform);
        this.transform.localPosition = new Vector3(0, 6, 0);

        var textComp = GetComponent<TMP_Text>();
        textComp.text = text;
        textComp.color = color;

        transform.DOLocalMoveY(transform.position.y + 3, moveSpeed).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Destroy(gameObject);
        });
	}
}
