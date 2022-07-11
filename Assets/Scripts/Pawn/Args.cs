using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgsTranslator
{
    public static ArgsTranslator Inst { get; private set; }
    void Awake() => Inst = this;

    public ArgsTranslator()
    {
        try
        {
            
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Condition name is not found");
        }
    }

    public int GetVariableFromPawn(string pawn, string varName)
    {
        try
        {
            return int.Parse(ArgsTranslator_GetPawnFromString(pawn).GetType().GetField(varName).GetValue(pawn).ToString());
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Variable Name is not found");
            return 0;
        }
    }

    public Pawn ArgsTranslator_GetPawnFromString(string pawnName)
    {
        try
        {
            
            return null;
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Pawn Name is not found");
            return null;
        }
    }

    public void ArgsTranslator_Function<T>(string funname)
    {
        switch (funname)
        {
            case "Function_LowestHealth":
                Debug.Log("Function_LowestHealth");
                return;
            default:
                return;
        }
    }
}

[System.Serializable]
public class PawnArguments
{
    public string pawnName;
    public string varName;
    public string value;
}