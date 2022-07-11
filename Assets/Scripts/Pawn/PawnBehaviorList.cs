using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnBehaviorList
{

    public static PawnBehaviorList Inst { get; private set; }
    void Awake() => Inst = this;

    public void PawnBehaviorTranslator(Behavior behavior)
    {
        try
        {
            this.GetType().GetMethod(behavior.name).Invoke(this, new object[] {behavior.args});
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Condition name is not found");
        }
    }

    //  Buff나 Debuff를 적용하는 것은 이 함수들 안에서 처리한다. 

    #region Action
    // 행동들

    public void Behavior_Action_NormalAttack(Pawn fromPawn, Pawn toPawn, int amount)
    {
        /* fromPawn이 toPawn에게 damage만큼 공격한다.
        만약 적이 죽었다면, toPawn의 Death! 함수를 호출하고 true를 리턴한다.

        TODO: %연산을 할 때 계산을 할 때 마다 정수로 만들어야 할까 아님 계산을 다 끝내고 정수로 만들어야 할까?
        */
        int totalDamageAmount = amount;

        // modifier_normal_attack을 계산한다.
        totalDamageAmount += fromPawn.modifier_normal_attack;



        // toPawn의 방어막
        if (toPawn.shield > 0)
        { //TODO: Floating message 띄우기
            if (totalDamageAmount < toPawn.shield)
            {
                // 데미지보다 쉴드량이 더 많을 때
                toPawn.shield -= totalDamageAmount;
            }
            else if (totalDamageAmount > toPawn.shield)
            {
                // 데미지보다 쉴드량이 더 적을 때
                toPawn.shield = 0;
                toPawn.health -= (totalDamageAmount - toPawn.shield);
            }
            else
            {
                // 데미지와 쉴드량이 같을 때
                toPawn.shield = 0;
            }
        }

        // toPawn의 health를 감소시킨다.
        toPawn.health = totalDamageAmount > toPawn.health ? 0 : toPawn.health - totalDamageAmount;

        BattleManager.Inst.UpdateUI();

        // 적이 죽었는지 확인한다.
        if (toPawn.health <= 0)
        {
            toPawn.CallDeath();
        }
    }

    public void Behavior_Action_SanityAttack(Pawn fromPawn, Pawn toPawn, int amount)
    {
        /* fromPawn이 toPawn에게 damage만큼 정신공격한다.*/
        int totalDamageAmount = amount;

        toPawn.sanity -= totalDamageAmount;  // 적의 정신력을 정수만큼 감소시킨다.

        BattleManager.Inst.UpdateUI();

        // 적의 정신력이 죽었는지 확인한다.
        if (toPawn.sanity < 0)
        {
            toPawn.CallSanityDeath();
        }
    }

    public void Behavior_Action_Heal(Pawn fromPawn, Pawn toPawn, int amount)
    {
        /* fromPawn이 toPawn에게 heal만큼 치료한다.*/
        double totalHealAmount = amount;

        toPawn.health = +int.Parse(totalHealAmount.ToString());  // 적의 체력을 정수만큼 회복시킨다.

        // 적의 체력이 최대치를 넘지 않았는지 확인한다.
        if (toPawn.health > toPawn.maxHealth) toPawn.health = toPawn.maxHealth;

        BattleManager.Inst.UpdateUI();

    }

    public void Behavior_Action_GetShield(Pawn fromPawn, Pawn toPawn, int amount)
    {
        /* fromPawn이 toPawn에게 shield만큼 보호막을 부여한다.*/
        int totalShieldAmount = amount;

        // modifier_defend를 계산한다.
        totalShieldAmount += fromPawn.modifier_defend;

        toPawn.shield = totalShieldAmount;  // toPawn의 방어막을 정수만큼 부여시킨다.

        BattleManager.Inst.UpdateUI();

    }

    #endregion

    #region Buff
    // 버프를 거는 함수들

    public void Behavior_Buff_Power(Pawn fromPawn, Pawn toPawn, int amount)
    {
        toPawn.modifier_normal_attack += amount;
        BattleManager.Inst.UpdateUI();

    }

    #endregion

    #region DeBuff
    // 디버프를 거는 함수들

    public void Behavior_Debuff_Weaken(Pawn fromPawn, Pawn toPawn, int amount)
    {
        toPawn.modifier_normal_attack -= amount;
        BattleManager.Inst.UpdateUI();

    }

    #endregion

}
