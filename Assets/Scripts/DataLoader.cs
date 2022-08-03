using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DataLoader
{
    public Dictionary<string, PawnEnemyGroups> poolEnemyGroup = new Dictionary<string, PawnEnemyGroups>();
    public Dictionary<string, Encounter> poolEncounter = new Dictionary<string, Encounter>();
    public Dictionary<string, Action> poolProp = new Dictionary<string, Action>();



    public void AddEnemy<T>(string enemyGroupID) where T: AbstractPawnEnemy
    {
        poolEnemyGroup.Add(enemyGroupID, new PawnEnemyGroups(enemyGroupID, new List<Action>{() => PawnManager.Inst.InstEnemy<T>()} ));;
    }
    public void AddEnemy<T>() where T: AbstractPawnEnemy
    {
        string id = typeof(T).ToString();
        poolEnemyGroup.Add(id, new PawnEnemyGroups(id, new List<Action>{() => PawnManager.Inst.InstEnemy<T>()} ));;
    }

    public void AddEnemyGroup(string encounterID, List<Type> enemies)
    {
        List<Action> actions = new List<Action>();

        foreach (Type enemyType in enemies)
        {
            actions.Add(() => PawnManager.Inst.InstEnemy(enemyType));
        }

        poolEnemyGroup.Add(encounterID, new PawnEnemyGroups(encounterID, actions));
    }



    public void AddBattleEncounter(string encounterID, Encounter encounter)
    {
        poolEncounter.Add(encounterID, encounter);
    }

    public void AddBattleEncounter(string encounterID, PawnEnemyGroups enemyGroup, Pattern pattern, int encounterLevel=1, int encounterProb=1)
    {
        poolEncounter.Add(encounterID, new Encounter(encounterID, enemyGroup, encounterLevel, encounterProb, pattern));
    }
    public void AddBattleEncounter(string encounterID, string enemyGroupID, Pattern pattern, int encounterLevel=1, int encounterProb=1)
    {
        poolEncounter.Add(encounterID, new Encounter(encounterID, poolEnemyGroup[enemyGroupID], encounterLevel, encounterProb, pattern));
    }
    



    public void AddPropToPool<T>(string propID) where T: AbstractProp
    {
        poolProp.Add(propID, () => BattleManager.Inst.InstProps<T>());
    }
    public void AddPropToPool<T>() where T: AbstractProp
    {
        string propID = typeof(T).ToString();
        poolProp.Add(propID, () => BattleManager.Inst.InstProps<T>());
    }
}

public class PawnEnemyGroups
{
    public string enemyGroupID;
    public List<Action> pawnEnemies;

    public PawnEnemyGroups(string enemyGroupID, List<Action> enemies)
    {
        this.enemyGroupID = enemyGroupID;
        this.pawnEnemies = enemies;
    }
}

public class Encounter
{
    public string encounterID;
    public PawnEnemyGroups pawnEnemyGroups;

    public int encounterLevel;
    public int encounterProbability;

    public Pattern pattern;

    public Encounter(
        string encounterID,
        PawnEnemyGroups pawnEnemyGroups,
        int encounterLevel,
        int encounterProbability,
        Pattern pattern)
    {
        this.encounterID = encounterID;
        this.pawnEnemyGroups = pawnEnemyGroups;
        this.encounterLevel = encounterLevel;
        this.encounterProbability = encounterProbability;   
        this.pattern = pattern;
    }
}
