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

    public override void onEquip()
    {
        PawnManager.Inst.player.maxHealth += 10;
        PawnManager.Inst.player.health += 10;
    }
}
