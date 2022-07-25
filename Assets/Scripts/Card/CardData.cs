using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CardData
{
    [System.NonSerialized] public bool isNGCard;
    public string cardName;
    public int speed;

    public List<string> cardEffect;

    public void UseEffect()
    {
        
    }
}
