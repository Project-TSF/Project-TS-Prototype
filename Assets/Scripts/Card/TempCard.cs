using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCard
{
    public CardData GetTempCard()
    {
        CardData cardData = new CardData();

        cardData.cardName = "Attack";
        cardData.speed = 1;

        var attackValue = 4;

        cardData.cardEffect = new List<string> { $"Behavior_Action_NormalAttack({attackValue})" };

        return cardData;
    }
}
