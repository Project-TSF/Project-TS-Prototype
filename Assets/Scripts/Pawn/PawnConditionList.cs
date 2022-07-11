using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PawnConditionList
{
    public static PawnConditionList Inst { get; private set; }
    void Awake() => Inst = this;

    public bool PawnConditionTranslator(Condition condition)
    {
        try
        {
            return (bool)this.GetType().GetMethod(condition.name).Invoke(this, new PawnArguments[] {condition.args});
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Condition name is not found");
            return false;
        }// TODO: int.parse exception 아마 있을꺼같은데 그거도 해야할듯 근데 어떤 오류인지 몰?루
    }


    #region ValueCompare
    /* 넘겨 받은 숫자와 Pawn의 변수의 크기를 비교하는 함수들 */
        public bool LessThan(PawnArguments[] args) => ArgsTranslator.Inst.GetVariableFromPawn(args[0].pawnName, args[0].varName) < int.Parse(args[0].value);

        public bool Equal(PawnArguments[] args) => ArgsTranslator.Inst.GetVariableFromPawn(args[0].pawnName, args[0].varName) == int.Parse(args[0].value);

        public bool GreaterThan(PawnArguments[] args) => ArgsTranslator.Inst.GetVariableFromPawn(args[0].pawnName, args[0].varName) > int.Parse(args[0].value);

        public bool LessThanOrEqual(PawnArguments[] args) => ArgsTranslator.Inst.GetVariableFromPawn(args[0].pawnName, args[0].varName) <= int.Parse(args[0].value);

        public bool GreaterThanOrEqual(PawnArguments[] args) => ArgsTranslator.Inst.GetVariableFromPawn(args[0].pawnName, args[0].varName) >= int.Parse(args[0].value);

        public bool NotEqual(PawnArguments[] args) => ArgsTranslator.Inst.GetVariableFromPawn(args[0].pawnName, args[0].varName) != int.Parse(args[0].value);

    #endregion
}