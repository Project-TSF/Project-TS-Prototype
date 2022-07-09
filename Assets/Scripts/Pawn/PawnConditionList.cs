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
            return (bool)this.GetType().GetMethod(condition.name).Invoke(this, condition.args);
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Condition name is not found");
            return false;
        }

    }

    public int GetVariableFromPawn(Pawn pawn, string varName)
    {
        try
        {
            return int.Parse(pawn.GetType().GetField(varName).GetValue(pawn).ToString());
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Variable Name is not found");
            return 0;
        }
    }


    #region ValueCompare
    /* 넘겨 받은 숫자와 Pawn의 변수의 크기를 비교하는 함수들 */
        public bool LessThan(Pawn pawn, string varName, int compareValue) => GetVariableFromPawn(pawn, varName) < compareValue;

        public bool Equal(Pawn pawn, string varName, int compareValue) => GetVariableFromPawn(pawn, varName) == compareValue;

        public bool GreaterThan(Pawn pawn, string varName, int compareValue) => GetVariableFromPawn(pawn, varName) > compareValue;

        public bool LessThanOrEqual(Pawn pawn, string varName, int compareValue) => GetVariableFromPawn(pawn, varName) <= compareValue;

        public bool GreaterThanOrEqual(Pawn pawn, string varName, int compareValue) => GetVariableFromPawn(pawn, varName) >= compareValue;

        public bool NotEqual(Pawn pawn, string varName, int compareValue) => GetVariableFromPawn(pawn, varName) != compareValue;

    #endregion
}