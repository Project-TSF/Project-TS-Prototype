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

        var normalAttackCard = new CardData()
        {
            cardName = "Attack",
            cardEffect = new List<System.Action> { () => PawnBehaviorList.Inst.Behavior_Action_NormalAttack(enemy, PawnManager.Inst.player, Random.Range(4, 6)) },
            speed = Random.Range(0, 10)
        };

        var shieldCard = new CardData()
        {
            cardName = "Shield",
            cardEffect = new List<System.Action> { () => PawnBehaviorList.Inst.Behavior_Action_GetShield(enemy, enemy, 5) },
            speed = Random.Range(0, 10)
        };
        var powerCard = new CardData()
        {
            cardName = "Power",
            cardEffect = new List<System.Action> { () => PawnBehaviorList.Inst.Behavior_Buff_Power(enemy, enemy, 1) },
            speed = Random.Range(0, 10)
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
