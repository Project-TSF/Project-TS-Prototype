using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;



    #region CardMovementControl

    internal void CardMouseOver(Card card)
    {
        
    }

    internal void CardMouseExit(Card card)
    {
        
    }

    internal void CardMouseDown(Card card)
    {
        
    }

    internal void CardMouseUp(Card card)
    {
        
    }

    internal void CardMouseDrag(Card card)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        card.transform.position = mousePosition;
    }

    #endregion
}
