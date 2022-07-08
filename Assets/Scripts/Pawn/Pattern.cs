using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pattern
{
    public List<Act> acts;


}

[System.Serializable]
public class Act
{
    public string name;
    public List<Effect> actions;
}


[System.Serializable]
public class Effect
{
    public string name;
    public List<Behavior> behaviors;
}

[System.Serializable]
public class Behavior
{
    public string behaviorName;
    public string target;
    public string value;
}