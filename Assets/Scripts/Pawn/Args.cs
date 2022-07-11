using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArgsTranslator
{
    public static ArgsTranslator Inst { get; private set; }
    void Awake() => Inst = this;

    public void MainArgsTranslator(PawnArguments originalArgs)
    {
        try
        {
            TranslatedPawnArguments translatedArgs = new TranslatedPawnArguments();

            if (originalArgs.pawnName != null) translatedArgs.pawnName = ArgsTranslator_GetPawnFromString(originalArgs.pawnName);
            if (originalArgs.varName != null && originalArgs.pawnName != null) translatedArgs.varName = ArgsTranslator_GetVariableFromPawn(originalArgs.pawnName, originalArgs.varName);
            if (originalArgs.value != null) translatedArgs.value = ArgsTranslator_Value(originalArgs.value);
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Condition name is not found");
        }
    }

    public int ArgsTranslator_GetVariableFromPawn(string pawn, string varName)
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
        if (pawnName.StartsWith("&"))
        {
            if (pawnName == "&Player")
            {
                return PawnManager.Inst.player;
            }
            else if (pawnName == "&Self")
            {
                // TODO: Implant returnning self pawn
            }
        }

        try
        {
            return GameObject.Find(pawnName).GetComponent<Pawn>();
        }
        catch (System.NullReferenceException)
        {
            Debug.LogError("Pawn Name is not found");
            return null;
        }
    }

    public object ArgsTranslator_Value(string funString)
    {
        if (funString.StartsWith("Function"))
        {
            var funname = funString.Split('(')[0];

            switch (funname)
            {
                case "Function_Pawn_LowestHealthEnemy":
                    Debug.Log("Function_Pawn_LowestHealthEnemy");

                    // get lowest health pawn
                    Pawn lowestHealthPawn = null;
                    int lowestHealth = int.MaxValue;
                    foreach (Pawn pawn in PawnManager.Inst.enemyList)
                    {
                        if (pawn.health < lowestHealth)
                        {
                            lowestHealth = pawn.health;
                            lowestHealthPawn = pawn;
                        }
                    }

                    return lowestHealthPawn;


                case "Function_RandomInt":
                    Debug.Log("Function_RandomInt");
                    // get random int
                    try{
                        // parse args from behind of string (ex: "Function_Random_Int(1,10)" => "1,10")
                        string[] args = funString.Split('(')[1].Split(')')[0].Split(',');
                        int min = int.Parse(args[0]);
                        int max = int.Parse(args[1]);
                        return Random.Range(min, max);
                    }
                    catch(System.NullReferenceException)
                    {
                        Debug.LogError("Function_Random_Int: args is not found");
                        return 0;
                    }


                case "Function_GetPawnFromString":
                    Debug.Log("Function_GetPawnFromString");
                    // get pawn from string
                    try
                    {
                        // parse args from behind of string (ex: "Function_GetPawnFromString(Pawn_Enemy_1)" => "Pawn_Enemy_1")
                        string[] args = funString.Split('(')[1].Split(')')[0].Split(',');
                        return ArgsTranslator_GetPawnFromString(args[0]);
                    }
                    catch (System.NullReferenceException)
                    {
                        Debug.LogError("Function_GetPawnFromString: args is not found");
                        return null;
                    }

                default:
                    return null;
            }
        }
    
        
        try
        {
            return int.Parse(funString);
        }
        catch (System.Exception)
        { }
        
        return funString;
    }
}

[System.Serializable]
public class PawnArguments
{
    public string pawnName;
    public string varName;
    public string value;
}

[System.Serializable]
public class TranslatedPawnArguments
{
    public Pawn pawnName;
    public int varName;
    public object value;
}