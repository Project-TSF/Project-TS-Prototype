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
    public List<CardData> cardDatas;
}
