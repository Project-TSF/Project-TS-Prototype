using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DataLoader
{
    public Dictionary<string, PawnEnemyGroups> poolPawnEnemyGroup = new Dictionary<string, PawnEnemyGroups>();
    public Dictionary<string, Encounter> poolEncounter = new Dictionary<string, Encounter>();
    public Dictionary<string, Action> poolProp = new Dictionary<string, Action>();


    public void AddMonster(string monsterGroupID, System.Action enemy)
    {
        poolPawnEnemyGroup.Add(monsterGroupID, new PawnEnemyGroups(monsterGroupID, new List<AbstractPawnEnemy>() {}));
    }
    public void AddMonster(System.Action enemy)
    {
        string id = enemy.GetType().ToString();
        poolPawnEnemyGroup.Add(id, new PawnEnemyGroups(id, new List<AbstractPawnEnemy>() {}));
    }
    public void AddMonsterGroup(string encounterID, List<AbstractPawnEnemy> enemies)
    {
        poolPawnEnemyGroup.Add(encounterID, new PawnEnemyGroups(encounterID, enemies));
    }

    public void AddBattleEncounter(string encounterID, PawnEnemyGroups enemies)
    {
        poolEncounter.Add(encounterID, new Encounter(encounterID, enemies));
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
    public List<AbstractPawnEnemy> pawnEnemies;

    public PawnEnemyGroups(string enemyGroupID, List<AbstractPawnEnemy> enemies)
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

    public Encounter(string encounterID, PawnEnemyGroups pawnEnemyGroups)
    {
        this.encounterID = encounterID;
        this.pawnEnemyGroups = pawnEnemyGroups;
    }
}
