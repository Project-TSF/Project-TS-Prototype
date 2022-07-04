using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonLoader
{
    public static T LoadJson<T>(string path)
    {
        string json = Resources.Load<TextAsset>(path).text;
        return JsonUtility.FromJson<T>(json);
    }

    // export to json file
    public void SaveJson(JsonData obj)
    {
        string json = JsonUtility.ToJson(obj);
        Debug.Log(json);
    }
}


[System.Serializable]
public class JsonData
{
    public string name;
    public int health;  
    public int sanity;

    public List<Pattern> patterns;

    public List<Trigger> triggers;
}


#region Trigger

[System.Serializable]
public class Trigger
{
    public List<Condition> conditions;
    public List<Action> actions;
}

[System.Serializable]
public class Condition
{
    virtual public bool CheckCondition()
    {
        return false;
    }
}

[System.Serializable]
public class OR : Condition
{
    public List<Condition> conditions;
    
    override public bool CheckCondition()
    {
        foreach (Condition condition in conditions)
        {
            if (!condition.CheckCondition()) {
                return false;
            }
        }

        return true;
    }
}


[System.Serializable]
public class LessThan : Condition
{
    public string targetName;
    public string targetVariable;

    public int value;

    override public bool CheckCondition()
    {
        try
        {
            int targetValue = (int)BattleManager.Inst.GetVariable(targetName, targetVariable);

            if (targetValue < value)
            {
                Debug.Log("Condition met");

                return true;
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Variable not found");

            return false;
        }

        return false;
    }
}

#endregion

#region Pattern

[System.Serializable]
public class Pattern
{
    public string name;
    public List<Act> acts;
}

[System.Serializable]
public class Act
{
    public string name;
    public List<Action> actions;
}

[System.Serializable]
public class Action
{
    public virtual void Execute() {}
}

[System.Serializable]
public class BehaviorAttack : Action
{
    public string targetName;
    public int damage;

    public override void Execute()
    {
        GameObject target = GameObject.Find(targetName);
        Debug.Log("Attacking " + targetName + " for " + damage + " damage");
    }
}

#endregion