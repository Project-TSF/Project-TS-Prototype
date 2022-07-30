using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProp_Blueberries : AbstractProp
{
    public TestProp_Blueberries()
    {
        ID = "TestProp_Blueberries";
        propName = "Blueberries";
        propDescription = "Add 10 HP to Max HP";
    }

    public override void OnEquip()
    {
        PawnManager.Inst.player.maxHealth += 10;
        PawnManager.Inst.player.health += 10;
    }
}

public class TestProp_TimeBomb : AbstractProp
{
    private int bombCount = 1;

    public TestProp_TimeBomb()
    {
        ID = "TestProp_TimeBomb";
        propName = "Time Bomb";
        propDescription = "Give 10 Damage to all enemies after 3 Turn passes";
        propText = bombCount.ToString();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        propText = bombCount.ToString();

        if (bombCount > 3)
        {
            PawnManager.Inst.enemyList.ForEach(enemy =>
            {
                PawnBehaviorList.Inst.Behavior_Action_NormalAttack(PawnManager.Inst.player, enemy, 10);
            });
            bombCount = 0;
        }
        else
        {
            bombCount++;
        }

        UpdateText();
    }
}
