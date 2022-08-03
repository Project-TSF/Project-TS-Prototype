using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGCard : AbstractCard
{
    public override CardType cardType {get;set;} = CardType.NG;
    public override string cardName {get;set;} = "NG";
    public override int speed {get;set;} = 0;

    public override void onUse()
    {
        Debug.Log("NG Card");
    }
}
