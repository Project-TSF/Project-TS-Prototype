using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pattern    // 1개의 적에 들어가는 패턴.
{
    public List<Act> acts;


}

[System.Serializable]
public class Act    // 1개의 Act. 아마 3개의 카드를 가질꺼임
{
    public string name;
    public List<CardEffect> actions;
}


[System.Serializable]
public class CardEffect     // 카드 1개에 들어감. 한 카드에 여러개의 Behavior를 할 수 있음
{
    public string name;
    public List<Behavior> behaviors;
}

[System.Serializable]
public class Behavior   // 효과 발동
{
    public string name;
    public PawnArguments args;
}