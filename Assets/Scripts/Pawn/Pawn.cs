using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Pawn : MonoBehaviour
{
    // public Hashtable variables = new Hashtable();
    
    // 이 Pawn의 ID입니다. 다른 ID와 겹치지 않게 항상 유니크해야합니다.

    public string ID = "";

    // 이름
    public string pawnName = "";


    // 체력
    public int health = 0;
    public int maxHealth = 0;


    // 정신력
    public int sanity = 0;
    public int maxSanity = 0;

    // 쉴드
    public int shield = 0;

    [Header("Modifiers(De/Buffs)")]

    public int modifier_normal_attack = 0; // 이 Pawn이 공격할 때 고정 수치를 변화시킵니다. Power/Weaken
    public int modifier_defend = 0;   // 이 Pawn이 방어를 할 때 고정 수치를 변화시킵니다. 

    [Header("UI Attributes")]
    
    public TMP_Text healthTMP;
    public TMP_Text sanityTMP;
    public TMP_Text shieldTMP;

    [SerializeField] GameObject buffIndicatorPrefab;
    [SerializeField] List<PawnBuffIndicator> buffIndicatorList = new List<PawnBuffIndicator>();


    public void UpdateUI()
    {
        healthTMP.text = health + " / " + maxHealth;
        sanityTMP.text = sanity + " / " + maxSanity;
        shieldTMP.text = shield.ToString();

        //TODO: modifier나 아예 다른 변수들 전부 딕셔너리 같은걸로 관리해버리고, foreach문으로 관리해버리면 편할꺼같아오
        if (modifier_normal_attack != 0)
        {
            // find if there is already a power buff indicator
            if (buffIndicatorList.Find(x => x.name == "Power") == null)
                AddBuffIndicator("Power", modifier_normal_attack.ToString(), Color.white);
            else 
            {
                buffIndicatorList.Find(x => x.name == "Power").buffText = modifier_normal_attack.ToString();
                buffIndicatorList.Find(x => x.name == "Power").UpdateUI();
            }
        }
        else
        {
            RemoveBuffIndicator("Power");
        }
        if (modifier_defend != 0)
        {
            if (buffIndicatorList.Find(x => x.name == "Defend") == null)
                AddBuffIndicator("Defend", modifier_defend.ToString(), Color.white);
            else 
            {
                buffIndicatorList.Find(x => x.name == "Defend").buffText = modifier_normal_attack.ToString();
                buffIndicatorList.Find(x => x.name == "Defend").UpdateUI();
            }
        }
        else
        {
            RemoveBuffIndicator("Defend");
        }
    }

    #region PawnBuffIndicator

    public void AddBuffIndicator(string name, string value, Color color)
    { //TODO: 모션 넣기
        PawnBuffIndicator buffIndicator = Instantiate(buffIndicatorPrefab).GetComponent<PawnBuffIndicator>();
        buffIndicator.transform.SetParent(transform);
        buffIndicator.name = name;
        buffIndicator.buffText = value;
        buffIndicator.UpdateUI();
        
        buffIndicatorList.Add(buffIndicator);
        AlignBuffIndicator();
    }

    public void RemoveBuffIndicator(string name)
    {
        foreach (PawnBuffIndicator buffIndicator in buffIndicatorList)
        {
            if (buffIndicator.name == name)
            {
                Destroy(buffIndicator.gameObject);
                buffIndicatorList.Remove(buffIndicator);
                break;
            }
        }
    }

    public void AlignBuffIndicator()
    {
        var i = 0;
        foreach (PawnBuffIndicator buffIndicator in buffIndicatorList)
        {
            buffIndicator.transform.localPosition = new Vector3(6, 4.5f - i*1.5f, 0);
            i++;
        }
    }

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
