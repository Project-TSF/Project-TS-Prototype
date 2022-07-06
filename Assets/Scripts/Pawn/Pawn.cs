using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{

    // 이 Pawn의 ID입니다. 같은 종류의 Pawn은 모두 같은 ID를 가집니다.
    // EX) 적A, 적B, 적C가 있을 때, 모두 Spade라면 서로 같은 ID를 가집니다. 애옹이
    public string ID { get; }


    // 체력
    public int health;
    public int maxHealth;


    // 정신력
    public int sanity;
    public int maxSanity;

    // 쉴드
    public int shield;


    #region De/Buff Status

    internal int modifier_normal_attack; // 이 Pawn이 공격할 때 고정 수치를 변화시킵니다. Power/Weaken
    internal int modifier_defend;   // 이 Pawn이 방어를 할 때 고정 수치를 변화시킵니다. 

    #endregion

    internal void CallDeath()
    {
        throw new NotImplementedException();
    }

    internal void CallSanityDeath()
    {
        throw new NotImplementedException();
    }
}
