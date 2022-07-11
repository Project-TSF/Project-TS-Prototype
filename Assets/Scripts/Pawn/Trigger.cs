using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trigger
{
    public string name;
    public List<Condition> conditions;

    // TODO: 조건 만족시 발동하는거 만들기
}

[System.Serializable]
public class Condition
{
    public string name;
    public PawnArguments args;
}