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
            cardName = "Attack",
            cardEffect = new List<string> { "Behavior_Action_NormalAttack(SPawn_Player, Function_RandomInt(4, 5))" }
        };
        var shieldCard = new CardData()
        {
            cardName = "Shield",
            cardEffect = new List<string> { "Behaviour_Action_GetShield(SPawn_Self, 4)" }
        };
        var powerCard = new CardData()
        {
            cardName = "Power",
            cardEffect = new List<string> { "Behavior_Buff_Power(SPawn_Self, 1)" }
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


        return enemy;
    }




}
