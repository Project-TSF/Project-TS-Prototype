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
        cardData.cardEffect = new CardEffect()
        {
            name = "공격",
            behaviors = new List<Behavior>() {
                    new Behavior()
                    {
                        name = "Behavior_Action_NormalAttack",
                        args = new PawnArguments()
                        {
                            pawnName = "&Player",
                            value = "Function_RandomInt(4, 5)"
                        }
                    }
                }
        };

        return cardData;
    }
}
