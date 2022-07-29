using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCard
{
    public CardData GetTempAttackCard()
    {
        CardData cardData = new CardData();

        cardData.cardName = "Attack";
        cardData.speed = Random.Range(0, 10);

        var attackValue = 4;
        // Pawn target = null;

        cardData.cardEffect = new List<System.Action> { () => PawnBehaviorList.Inst.Behavior_Action_NormalAttack(PawnManager.Inst.player, PawnManager.Inst.enemyList[0], attackValue) };

        return cardData;
    }

    public CardData GetTempGetShieldCard()
    {
        CardData cardData = new CardData();

        cardData.cardName = "Shield";
        cardData.speed = Random.Range(0, 10);

        var shieldValue = 4;
        // Pawn target = null;

        cardData.cardEffect = new List<System.Action> { () => PawnBehaviorList.Inst.Behavior_Action_GetShield(PawnManager.Inst.player, PawnManager.Inst.player, shieldValue) };

        return cardData;
    }

    public CardData GetTempPowerCard()
    {
        CardData cardData = new CardData();

        cardData.cardName = "Power";
        cardData.speed = Random.Range(0, 10);

        var powerValue = 1;
        // Pawn target = null;

        cardData.cardEffect = new List<System.Action> { () => PawnBehaviorList.Inst.Behavior_Buff_Power(PawnManager.Inst.player, PawnManager.Inst.player, powerValue) };

        return cardData;
    }
}
