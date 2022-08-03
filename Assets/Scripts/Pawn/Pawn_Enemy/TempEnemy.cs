using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEnemy_Spade : AbstractPawnEnemy
{
    public override string pawnName { get; set; } = "Spade";
    public override int health { get; set; } = 20;
    public override int maxHealth { get; set; } = 20;
    public override int sanity { get; set; } = 20;
    public override int maxSanity { get; set; } = 20;

    public class PawnEnemy_Spade_Card_Attack : AbstractCard
    {
        public Pawn fromPawn;
        public override string cardName { get; set; } = "Attack";
        public override CardType cardType { get; set; } = CardType.Action;
        public override int speed { get; set; } = Random.Range(0, 10);

        private int damage = 5;

        public PawnEnemy_Spade_Card_Attack(Pawn fromPawn)
        {
            this.fromPawn = fromPawn;
        }

        public override void onUse()
        {
            PawnBehaviorList.Inst.Behavior_Action_NormalAttack(fromPawn, PawnManager.Inst.player, damage);
        }
    }   

    public class PawnEnemy_Spade_Card_GetShield: AbstractCard
    {
        public Pawn fromPawn;

        public override string cardName { get; set; } = "GetShield";
        public override CardType cardType { get; set; } = CardType.Action;
        public override int speed { get; set; } = Random.Range(0, 10);

        private int shield = 5;

        public PawnEnemy_Spade_Card_GetShield(Pawn fromPawn)
        {
            this.fromPawn = fromPawn;
        }

        public override void onUse()
        {
            PawnBehaviorList.Inst.Behavior_Action_GetShield(fromPawn, PawnManager.Inst.player, shield);
        }
    }

    public class PawnEnemy_Spade_Card_Power: AbstractCard
    {
        public Pawn fromPawn;

        public override string cardName { get; set; } = "Power";
        public override CardType cardType { get; set; } = CardType.Skill;
        public override int speed { get; set; } = Random.Range(0, 10);

        private int sanity = 5;

        public PawnEnemy_Spade_Card_Power(Pawn fromPawn)
        {
            this.fromPawn = fromPawn;
        }

        public override void onUse()
        {
            PawnBehaviorList.Inst.Behavior_Buff_Power(fromPawn, PawnManager.Inst.player, sanity);
        }
    }
}
