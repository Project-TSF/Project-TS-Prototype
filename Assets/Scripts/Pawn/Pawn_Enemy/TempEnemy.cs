using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy
{
    public Enemy GetTempEnemySpade(Enemy enemy)
    {
        // ID = "Spade"; 
        enemy.pawnName = "Spade";
        enemy.health = 100;
        enemy.maxHealth = 100;
        enemy.sanity = 100;
        enemy.maxSanity = 100;
        enemy.shield = 0;
        enemy.modifier_normal_attack = 0;
        enemy.modifier_defend = 0;

        var normalAttackCard = new CardData()
        {
            cardName = "Normal_Attack",
            cardEffect = new CardEffect()
            {
                behaviors = new List<Behavior>() {
                    new Behavior()
                    {
                        name = "Behavior_Action_NormalAttack",
                        args = new PawnArguments()
                        {
                            pawnName = "&Player",
                            value = "Function_RandomInt(4, 5)"
                        }
                    }
                }
            }
        };
        var shieldCard = new CardData()
        {
            cardName = "Shield",
            cardEffect = new CardEffect()
            {
                behaviors = new List<Behavior>() {
                    new Behavior() {
                        name = "Behavior_Action_GetShield",
                        args = new PawnArguments()
                        {
                            pawnName = "&Self",
                            value = "4"
                        }
                    }
                }
            }
        };
        var powerCard = new CardData()
        {
            cardName = "Power",
            cardEffect = new CardEffect()
            {
                behaviors = new List<Behavior>() {
                    new Behavior()
                    {
                        name = "Behavior_Buff_Power",
                        args = new PawnArguments()
                        {
                            pawnName = "&Self",
                            value = "1"
                        }
                    }
                }
            }
        };

        enemy.pattern = new Pattern()
        {
            acts = new List<Act>()
                {
                    new Act()
                    {
                        name = "Act_1",
                        cardDatas = new List<CardData>()
                        {
                            normalAttackCard,
                            shieldCard,
                            powerCard
                        }
                    },
                    new Act()
                    {
                        name = "Act_2",
                        cardDatas = new List<CardData>()
                        {
                            normalAttackCard,
                            powerCard,
                            shieldCard
                        }
                    },
                    new Act()
                    {
                        name = "Act_3",
                        cardDatas = new List<CardData>()
                        {
                            shieldCard,
                            powerCard,
                            normalAttackCard
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
