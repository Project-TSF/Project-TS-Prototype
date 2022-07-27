using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CardData
{
    [System.NonSerialized] public bool isNGCard;
    public string cardName;
    public int speed;

    public List<System.Action> cardEffect;

    public void UseEffect()
    {
        foreach (System.Action action in cardEffect)
        {
            action.Invoke();
        }
    }
}
