using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPlayer : Pawn
{
    public override string pawnName { get; set; } = "Player";
    public override int health { get; set; } = 80;
    public override int maxHealth { get; set; } = 80;
    public override int sanity { get; set; } = 80;
    public override int maxSanity { get; set; } = 80;
}
