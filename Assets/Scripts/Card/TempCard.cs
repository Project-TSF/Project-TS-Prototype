using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCard_AttackCard : AbstractCard
{
    public override CardType cardType { get; set; } = CardType.Action;
    public override string cardName { get; set; } = "Attack";
    public override int speed { get; set; }

    int attackValue = 4;

    public override void onUse()
    {
        PawnBehaviorList.Inst.Behavior_Action_NormalAttack(PawnManager.Inst.player, PawnManager.Inst.enemyList[0], attackValue);
    }
}

public class TempCard_GetShieldCard : AbstractCard
{
    public override CardType cardType { get; set; } = CardType.Action;
    public override string cardName { get; set; } = "Get Shield";
    public override int speed { get; set; }

    int shieldValue = 5;

    public override void onUse()
    {
        PawnBehaviorList.Inst.Behavior_Action_GetShield(PawnManager.Inst.player, PawnManager.Inst.player, shieldValue);
    }
}

public class TempCard_PowerCard : AbstractCard
{
    public override CardType cardType { get; set; } = CardType.Skill;
    public override string cardName { get; set; } = "Power";
    public override int speed { get; set; }

    int powerValue = 1;

    public override void onUse()
    {
        PawnBehaviorList.Inst.Behavior_Buff_Power(PawnManager.Inst.player, PawnManager.Inst.player, powerValue);
    }
}
