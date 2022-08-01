using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProp_Blueberries : AbstractProp
{
    public override string propName {get;set;} = "Blueberries";
    public override string propDescription {get;set;} = "A handful of blueberries. Add 10 HP to Max HP";
    public override string ImgPath {get;set;} = "";

    public override void OnEquip()
    {
        PawnManager.Inst.player.maxHealth += 10;
        PawnManager.Inst.player.health += 10;
    }
}

public class TestProp_TimeBomb : AbstractProp
{
    public override string propName { get;set; } = "Time Bomb";
    public override string propDescription {get;set;} = "Give 10 Damage to all enemies after 3 Turn passes";
    public override string ImgPath { get; set; } = "";

    private int bombCount = 1;



    public TestProp_TimeBomb()
    {
        propText = bombCount.ToString();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        propText = bombCount.ToString();

        if (bombCount >= 3)
        {
            PawnManager.Inst.enemyList.ForEach(enemy =>
            {
                PawnBehaviorList.Inst.Behavior_Action_NormalAttack(PawnManager.Inst.player, enemy, 10);
            });
            bombCount = 1;
        }
        else
        {
            bombCount++;
        }

        UpdateText();
    }
}
