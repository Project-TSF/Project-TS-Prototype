using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Pawn : MonoBehaviour
{
    
    // 이 Pawn의 ID입니다. 다른 ID와 겹치지 않게 항상 유니크해야합니다.
    public string ID;

    // 이름
    public string pawnName;


    // 체력
    public int health;
    public int maxHealth;


    // 정신력
    public int sanity;
    public int maxSanity;

    // 쉴드
    public int shield;

    [Header("Modifiers(De/Buffs)")]

    public int modifier_normal_attack; // 이 Pawn이 공격할 때 고정 수치를 변화시킵니다. Power/Weaken
    public int modifier_defend;   // 이 Pawn이 방어를 할 때 고정 수치를 변화시킵니다. 


    [Header("UI Attributes")]
    
    public TMP_Text healthTMP;    //TODO: 이거 플레이어 클래스로 옮겨야 할지도?
    public TMP_Text sanityTMP;



    internal void CallDeath()
    {
        throw new NotImplementedException();
    }

    internal void CallSanityDeath()
    {
        throw new NotImplementedException();
    }
}
