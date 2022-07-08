using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Transform slotStartPos;

    [Space]

    [SerializeField] GameObject slotPanel;
    [SerializeField] Slot slotPrefab;
    [SerializeField] Transform slotGenPos;
    [SerializeField] List<SlotSet> slotsets;
    public int slotAmount;

    [Space]

    [SerializeField] TMP_Text healthTMP;    //TODO: 이거 플레이어 클래스로 옮겨야 할지도?
    [SerializeField] TMP_Text sanityTMP;

    public int maxHealth;
    public int maxSanity;
    public int currentHealth;
    public int currentSanity;

    [Space]

    [SerializeField] Transform PlayerPos;   // 플레이어가 평소 서 있는 위치. 현재 위치와 헷갈릴 수 있다.
    [SerializeField] Transform EnemyPos;

    [SerializeField] Player player;
    [SerializeField] List<Enemy> enemyList;
    [SerializeField] Enemy enemyPrefab; 


    public static BattleManager Inst { get; private set; }
    void Awake() => Inst = this;

    private void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {

        // TODO: 디버그

        var tempEnemyGen = new TempEnemy();

            // ID = "Player",
        player.maxHealth = 80;
        player.health = 80;
        player.maxSanity = 80;
        player.sanity = 80;

        player.modifier_normal_attack = 0;
        player.modifier_defend = 0;


        enemyList = new List<Enemy>();
        
        for (var i = 0; i < 1; ++i)
        {
            var tempEnemy = tempEnemyGen.Get_TempEnemy_Spade(MakeEnemy());
            enemyList.Add(tempEnemy);
        }

        // 여기까지


        UpdateUI();
        StartTurn();
    }

    public void UpdateUI()
    {
        healthTMP.text = player.health + " / " + player.maxHealth;
        sanityTMP.text = player.sanity + " / " + player.maxSanity;

        for (var i = 0; i < enemyList.Count; ++i)
        {
            var enemy = enemyList[i];
            enemy.healthTMP.text = enemy.health + " / " + enemy.maxHealth;
            enemy.sanityTMP.text = enemy.sanity + " / " + enemy.maxSanity;
        }

        EnemyAlignment();
    }

    #region Turn

    public void StartTurn() // 턴 시작
    {
        MakeSlots(slotAmount);
    }

    public void EndTurn() // Turn End 버튼이 눌렸을 때
    {
        Debug.Log("<<END TURN>>");

        List<Slot> timedSlotList = new List<Slot>();
        foreach (SlotSet slotSet in slotsets)
        {
            List<Slot> tempSlotSet = GetTimedSlots(slotSet);

            if (tempSlotSet == null)
            {
                Debug.Log("Slots are not full");

                return;
            }

            timedSlotList.AddRange(tempSlotSet);
        }

        foreach (Slot s in timedSlotList)
        {
            s.slotedCard.UseEffect();
        }

        GameObject.Find("BtnTogglePanel").GetComponent<BtnTogglePanel>().TogglePanel();
    }

    List<Slot> GetTimedSlots(SlotSet slotset) //slotset에서 slot에 있는 카드가 빠른 순으로 정렬해서 리턴
    {
        if (slotset.mySlot.slotedCard == null || slotset.enemySlot.slotedCard == null)
        {
            return null;
        }


        if (slotset.mySlot.slotedCard.cardData.speed < slotset.enemySlot.slotedCard.cardData.speed)
        {
            return new List<Slot>() { slotset.mySlot, slotset.enemySlot };
        }
        else if (slotset.mySlot.slotedCard.cardData.speed > slotset.enemySlot.slotedCard.cardData.speed)
        {
            return new List<Slot>() { slotset.enemySlot, slotset.mySlot };
        }
        else
        {
            return new List<Slot>() { slotset.mySlot, slotset.enemySlot }; // TODO: 방어/공격 서순 정하기
        }
    }

    #endregion

    #region Slot

    void SlotMoveTransform(Transform obj, PRS prs, float dotweenTime) // 슬롯 움직이는 함수
    {
        obj.DOMove(prs.pos, dotweenTime);
        obj.DORotateQuaternion(prs.rot, dotweenTime);
        obj.DOScale(prs.scale, dotweenTime);
    }

    void SlotSetsAlignment() // 슬롯 정렬하는 함수
    {
        var targetSlots = slotsets;
        for (int i = 0; i < targetSlots.Count; i++)
        {
            var targetSlotSet = targetSlots[i].mySlot.transform.parent;
            var newPosition = new Vector3(targetSlotSet.position.x + 5 * i, targetSlotSet.position.y, targetSlotSet.position.z);
            var newPRS = new PRS(newPosition, targetSlotSet.transform.rotation, targetSlotSet.transform.localScale);
            SlotMoveTransform(targetSlotSet, newPRS, 0.3f);
        }
    }

    Slot MakeSlot() // 슬롯 1개 Instantiate하고 return하는 함수
    {
        var slotObj = Instantiate(slotPrefab, slotGenPos.position, Utils.QI);
        var slot = slotObj.GetComponent<Slot>();

        slotObj.name = ("Slot " + UnityEngine.Random.Range(0, 1000).ToString());
        return slot;
    }

    SlotSet MakeSlotSets() // SlotSet(슬롯 2개 내꺼 적꺼) Instantiate하고 return하는 함수
    {
        GameObject SlotSetObj = new GameObject("SlotSet " + UnityEngine.Random.Range(0, 1000).ToString());

        Slot slotMy = MakeSlot();
        Slot slotEnemy = MakeSlot();

        slotMy.isMoveable = true;
        slotEnemy.isMoveable = false;

        SlotSetObj.transform.parent = slotStartPos.transform;

        slotMy.transform.parent = SlotSetObj.transform;
        slotEnemy.transform.parent = SlotSetObj.transform;

        slotMy.transform.localPosition = new Vector3(0, 0, 0);
        slotEnemy.transform.localPosition = new Vector3(0, 8, 0);


        slotEnemy.slotedCard = GetEnemyCard(slotEnemy);

        slotEnemy.slotedCard.slot = slotEnemy.gameObject;
        slotEnemy.slotedCard.transform.parent = slotEnemy.transform;

        slotEnemy.slotedCard.transform.localPosition = new Vector3(0, 0, 0);
        slotEnemy.slotedCard.originPRS = new PRS(
            slotEnemy.slotedCard.transform.position,
            Utils.QI,
            slotEnemy.slotedCard.transform.localScale);
        slotEnemy.slotedCard.setVisible(true);

        return new SlotSet() { mySlot = slotMy, enemySlot = slotEnemy };
    }

    Card GetEnemyCard(Slot slotEnemy) // 적 카드를 Enemy에서 불러와 return하는 함수
    {
        Card tempCard = CardManager.Inst.MakeCard(new CardData()); // TODO: 적 카드 받는 함수 구현

        return tempCard;
    }

    void MakeSlots(int amount) // slotset 여러개를 스폰하는 함수
    {
        for (int i = 0; i < amount; i++)
        {
            SlotSet slotSet = MakeSlotSets();
            slotsets.Add(slotSet);
            SlotSetsAlignment();
        }
    }

    #endregion

    #region Pawn

    public void PawnMove(Pawn pawn, PRS prs, float dotweenTime) // 폰 움직이는 함수
    {
        pawn.transform.DOMove(prs.pos, dotweenTime);
        pawn.transform.DORotateQuaternion(prs.rot, dotweenTime);
        pawn.transform.DOScale(prs.scale, dotweenTime);
    }

    public void EnemyAlignment() // 폰 정렬하는 함수
    {
        var targetPawns = enemyList;
        for (int i = 0; i < targetPawns.Count; i++)
        {
            var targetPawn = targetPawns[i].transform;
            var newPosition = new Vector3(targetPawn.position.x, targetPawn.position.y, targetPawn.position.z);
            var newPRS = new PRS(newPosition, targetPawn.transform.rotation, targetPawn.transform.localScale);
            PawnMove(targetPawn.GetComponent<Pawn>(), newPRS, 0.3f);
        }
    }

    public Enemy MakeEnemy() // Enemy 1개 Instantiate하고 return하는 함수
    {
        var enemyObj = Instantiate(enemyPrefab, EnemyPos.position, Utils.QI);
        var enemy = enemyObj.GetComponent<Enemy>();
        enemy.name = ("Enemy " + UnityEngine.Random.Range(0, 1000).ToString());
        return enemy;
    }

    #endregion


    // [SerializeField] JsonData jsondata;

    // public void test1()
    // {
    //     JsonLoader jsonLoader = new JsonLoader();
    //     jsondata = jsonLoader.LoadJson("testjson copy");

        
    // }

    // public void test()
    // {

    //     JsonLoader jsonLoader = new JsonLoader();

    //     JsonData jsonData = new JsonData()
    //     {
    //         name = "Spade",
    //         health = 10,
    //         sanity = 10,
    //         patterns = new List<Pattern>() {
    //             new Pattern() {
    //                 name = "Pattern 1",
    //                 acts = new List<Act>() {
    //                     new Act() {
    //                         name = "Act 1",
    //                         actions = new List<Action>() {
    //                             new Action() {
    //                                 actionName = "Damage",
    //                                 targetName = "Player",
    //                                 value = "2"
    //                             },
    //                             new Action() {
    //                                 actionName = "Damage",
    //                                 targetName = "Player",
    //                                 value = "4"
    //                             },
    //                             new Action() {
    //                                 actionName = "Power",
    //                                 targetName = "This",
    //                                 value = "2"
    //                             }
    //                         }
    //                     }
    //                 }
    //             },
    //         },
    //         triggers = new List<Trigger>() {
    //             new Trigger() {
    //                 conditions = new List<Condition>() {
    //                     new Condition() {
    //                         conditionName = "OR",
    //                         conditions = new List<Condition>() {
    //                             new Condition() {
    //                                 conditionName = "LessThan",
    //                                 targetName = "This",
    //                                 targetVariable = "Health",
    //                                 value = "5"
    //                             },

    //                             new Condition() {
    //                                 conditionName = "LessThan",
    //                                 targetName = "This",
    //                                 targetVariable = "Sanity",
    //                                 value = "5"
    //                             }
    //                         }
    //                     },
    //                 },
    //                 action = new Action() {
    //                         actionName = "Damage",
    //                         targetName = "Player",
    //                         value = "2"
    //                 }
    //             }
    //         }
    //     };


    //     jsonLoader.SaveJson(jsonData);
    // }


    public object GetVariable(string targetName, string targetVariable)
    {
        GameObject obj = GameObject.Find(targetName);
        object value = obj.GetType().GetField(targetVariable).GetValue(obj);
        Debug.Log(value);
        return value;
    }

    public object GetMethod(string targetName, string targetMethod)
    {
        GameObject obj = GameObject.Find(targetName);
        object value = obj.GetType().GetMethod(targetMethod).Invoke(obj, null);
        Debug.Log(value);
        return value;
    }
}
