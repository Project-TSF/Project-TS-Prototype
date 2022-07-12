using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Pawn
{
    // pattern of the pawn
    public Pattern pattern;

    // trigger
    public Trigger trigger;

    
    internal TMPro.TMP_Text healthTMP;
    internal TMPro.TMP_Text sanityTMP;
}