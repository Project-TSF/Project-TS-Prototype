using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader
{
    public Dictionary<string, PawnEnemyGroups> pawnEnemyGroups = new Dictionary<string, PawnEnemyGroups>();
    public Dictionary<string, Encounter> encounters = new Dictionary<string, Encounter>();

    public void AddMonster(string monsterGroupID, AbstractPawnEnemy enemy)
    {
        pawnEnemyGroups.Add(monsterGroupID, new PawnEnemyGroups(monsterGroupID, new List<AbstractPawnEnemy>() {enemy}));
    }

    public void AddMonsterGroup(string encounterID, List<AbstractPawnEnemy> enemies)
    {
        pawnEnemyGroups.Add(encounterID, new PawnEnemyGroups(encounterID, enemies));
    }

    public void AddBattleEncounter(string encounterID, PawnEnemyGroups enemies)
    {
        encounters.Add(encounterID, new Encounter(encounterID, enemies));
    }

    public Action GetEnemy()
    {
        return () =>
        {
            Debug.Log("GetEnemy");
        };
    }

    public void AddBoss()
    {

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
