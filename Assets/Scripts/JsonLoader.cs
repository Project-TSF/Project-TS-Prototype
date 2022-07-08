// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class JsonLoader
// {
//     public JsonData LoadJson(string path)
//     {
//         string json = Resources.Load<TextAsset>(path).text;
//         return JsonUtility.FromJson<JsonData>(json);
//     }

//     // export to json file
//     public void SaveJson(JsonData obj)
//     {
//         string json = JsonUtility.ToJson(obj);
//         Debug.Log(json);
//     }

//     #region CheckCondition

//     public bool CheckCondition(Condition condition)
//     {
//         switch (condition.conditionName)
//         {
//             case "OR":
//                 return checkOR(condition);
//             // case "AND":
//             //     return checkAND(condition);
//             case "LessThan":
//                 return checkLessThan(condition);
//             // case "GreaterThan":
//             //     return checkGreaterThan(condition);
//             // case "Equal":
//             //     return checkEqual(condition);
//             // case "NotEqual":
//             //     return checkNotEqual(condition);
//             default:
//                 return false;
//         }

//     }

//     private bool checkOR(Condition conditions)
//     {
//         foreach (Condition condition in conditions.conditions)
//         {
//             if (!CheckCondition(condition))
//             {
//                 return false;
//             }
//         }

//         return true;
//     }
//     private bool checkLessThan(Condition condition)
//     {
//         try
//         {
//             int targetValue = (int)BattleManager.Inst.GetVariable(condition.targetName, condition.targetVariable);

//             if (targetValue < int.Parse(condition.value))
//             {
//                 Debug.Log("Condition met");

//                 return true;
//             }
//         }
//         catch (System.Exception)
//         {
//             Debug.Log("Variable not found");

//             return false;
//         }

//         return false;
//     }

//     #endregion


//     #region GetAction

//     public ActionT GetAction(Action action)
//     {
//         switch (action.actionName)
//         {
//             case "Damage":
//                 return getDamage(action);
//             case "Power":
//                 return getPower(action);
//             // case "Heal":
//             //     return getHeal(action);
//             // case "Sanity":
//             //     return getSanity(action);
//             // case "Health":
//             //     return getHealth(action);
//             // case "Move":
//             //     return getMove(action);
//             // case "Attack":
//             //     return getAttack(action);
//             // case "Defend":
//             //     return getDefend(action);
//             // case "Wait":
//             //     return getWait(action);
//             // case "EndTurn":
//             //     return getEndTurn(action);
//             // case "EndBattle":
//             //     return getEndBattle(action);
//             default:
//                 return null;
//         }
//     }

//     private ActionT getPower(Action action)
//     {
//         throw new System.NotImplementedException();
//     }

//     private ActionT getDamage(Action action)
//     {
//         return new BehaviorAttack()
//         {
//             targetName = action.targetName,
//             damage = int.Parse(action.value)
//         };
//     }

//     #endregion

// }


// [System.Serializable]
// public class JsonData
// {
//     public string name;
//     public int health;
//     public int sanity;

//     public List<Pattern> patterns;

//     public List<Trigger> triggers;
// }


// #region Trigger

// [System.Serializable]
// public class Trigger
// {
//     public List<Condition> conditions;
//     public Action action;
// }

// [System.Serializable]
// public class Condition
// {
//     public string conditionName;
//     public string targetName;
//     public string targetVariable;
//     public string value;

//     public List<Condition> conditions;
// }

// #endregion

// #region Pattern

// [System.Serializable]
// public class Pattern
// {
//     public string name;
//     public List<Act> acts;
// }

// [System.Serializable]
// public class Act
// {
//     public string name;
//     public List<Action> actions;
// }

// [System.Serializable]
// public class Action
// {
//     public string actionName;
//     public string targetName;
//     public string value;
// }

// [System.Serializable]
// public class ActionT
// {
//     public virtual void Execute() { }
// }

// [System.Serializable]
// public class BehaviorAttack : ActionT   // TODO: 가능한 Action들 다 만들기 효과 발동쪽임
// {
//     public string targetName;
//     public int damage;

//     public override void Execute()
//     {
//         GameObject target = GameObject.Find(targetName);
//         Debug.Log("Attacking " + targetName + " for " + damage + " damage");
//     }
// }

// #endregion