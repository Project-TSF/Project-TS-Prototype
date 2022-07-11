using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy
{
    public Enemy Get_TempEnemy_Spade(Enemy enemy)
    {
        // ID = "Spade"; 
        enemy.health = 100;
        enemy.maxHealth = 100;
        enemy.sanity = 100;
        enemy.maxSanity = 100;
        enemy.shield = 0;
        enemy.modifier_normal_attack = 0;
        enemy.modifier_defend = 0;

        enemy.pattern = new Pattern()
        {
            acts = new List<Act>()
                {
                    new Act()
                    {
                        name = "Act_1",
                        actions = new List<Effect>()
                        {
                            new Effect(){name="Normal_Attack", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Action_NormalAttack",args = new PawnArguments(){pawnName = "&Player",value = "Function_RandomInt(4, 5)" }}}},
                            new Effect(){name="Shield", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Action_GetShield",args = new PawnArguments(){pawnName = "&Self",value = "4"}}}},
                            new Effect(){name="Power", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Buff_Power",args = new PawnArguments(){pawnName = "&Self",value = "1"}}}},
                        }
                    },
                    new Act()
                    {
                        name = "Act_2",
                        actions = new List<Effect>()
                        {
                            new Effect(){name="Normal_Attack", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Action_NormalAttack",args = new PawnArguments(){pawnName = "&Player",value = "Function_RandomInt(4, 5)"}}}},
                            new Effect(){name="Power", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Buff_Power",args = new PawnArguments(){pawnName = "&Self",value = "1"}}}},
                            new Effect(){name="Shield", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Action_GetShield",args = new PawnArguments(){pawnName = "&Self",value = "4"}}}},
                        }
                    },
                    new Act()
                    {
                        name = "Act_3",
                        actions = new List<Effect>()
                        {
                            new Effect(){name="Shield", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Action_GetShield",args = new PawnArguments(){pawnName = "&Self",value = "4"}}}},
                            new Effect(){name="Power", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Buff_Power",args = new PawnArguments(){pawnName = "&Self",value = "1"}}}},
                            new Effect(){name="Normal_Attack", behaviors = new List<Behavior>(){new Behavior(){name = "Behavior_Action_NormalAttack",args = new PawnArguments(){pawnName = "&Player",value = "Function_RandomInt(4, 5)"}}}},
                        }
                    }
                }
        };

        enemy.trigger = new Trigger()
        {
            name = "TestTrigger",
            conditions = new List<Condition>() {
                new Condition() {
                    name = "LessThan",
                    args = new PawnArguments() {
                        pawnName = "&Player",
                        varName = "health",
                        value = "10"
                    }
                }
            }
        };


        return enemy;
    }




}
