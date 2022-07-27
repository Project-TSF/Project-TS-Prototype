using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System.Threading.Tasks;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Transform slotStartPos;

    [Space]

    [SerializeField] GameObject slotPanel;
    [SerializeField] Slot slotPrefab;
    [SerializeField] Transform slotGenPos;
    [SerializeField] List<SlotSet> slotsets;
    public int slotAmount;


    public static BattleManager Inst { get; private set; }
    void Awake() => Inst = this;

    private void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        // 디버그용

        PawnManager.Inst.player.ID = "Player";

        PawnManager.Inst.player.modifier_normal_attack = 0;
        PawnManager.Inst.player.modifier_defend = 0;
        

        PawnManager.Inst.GetTestEnemy();

        // 여기까지 디버그용

        StartTurn();
        UpdateUI();
    }

    public void UpdateUI()
    {
        PawnManager.Inst.UpdateUI();
    }

    #region Turn

    public void StartTurn() // 턴 시작
    {
        MakeSlots(slotAmount);
        for (int i = 0; i < slotsets.Count; i++)
        {
            CardData card = slotsets[i].enemySlot.slotedCard.cardData;
            CardData carddata = PawnManager.Inst.enemyList[0].pattern.acts[0].cardDatas[i];

            // copying carddata to card
            card.cardName = carddata.cardName;
            card.speed = carddata.speed;
            card.cardEffect = carddata.cardEffect;

            slotsets[i].enemySlot.slotedCard.UpdateUI();
        }
    }

    public async void EndTurn() // Turn End 버튼이 눌렸을 때
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

        GameObject.Find("BtnTogglePanel").GetComponent<BtnTogglePanel>().TogglePanel();
        
        foreach (Slot slot in timedSlotList)
        {
            if (!Application.isPlaying) break;

            Debug.Log($"Use {slot.slotedCard.cardData.cardName}");
            slot.slotedCard.UseEffect();
            await Task.Delay(1500);
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

        // 슬롯을 적꺼 하나 내꺼 하나 만듬
        Slot slotMy = MakeSlot();
        Slot slotEnemy = MakeSlot();

        // 적의 카드는 움직일 수 없게, 내 카드는 움직일 수 있게 하기
        slotMy.isMoveable = true;
        slotEnemy.isMoveable = false;

        // Slotset의 위치를 옮기고 hierarchy 정리를 위해서 parent를 지정해주기
        SlotSetObj.transform.parent = slotStartPos.transform;

        // Slot들을 SlotSet 안에 넣어주기
        slotMy.transform.parent = SlotSetObj.transform;
        slotEnemy.transform.parent = SlotSetObj.transform;

        // SlotSet 안에서 Slot들의 위치 조절
        slotMy.transform.localPosition = new Vector3(0, 0, 0);
        slotEnemy.transform.localPosition = new Vector3(0, 8, 0);

        // 적 카드 받아오기인데 나중에 위에 디버그쪽에 짜둔거 GetEnemyCard로 옮기기
        slotEnemy.slotedCard = GetEnemyCard(slotEnemy);

        // 카드 내부 데이터 초기화
        slotEnemy.slotedCard.slot = slotEnemy.gameObject;
        
        // 적 카드를 적 슬롯 위에 놓기
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

}
