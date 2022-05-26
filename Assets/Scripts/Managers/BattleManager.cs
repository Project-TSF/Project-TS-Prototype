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
    [SerializeField] List<SlotSet> slots;
    public int slotAmount;

    [Space]

    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text sanityTMP;
    
    public int maxHealth;
    public int maxSanity;
    public int currentHealth;
    public int currentSanity;


    public static BattleManager Inst { get; private set; }
    void Awake() => Inst = this;
    
    private void Start() {
        StartGame();
    }

    public void StartGame()
    {
        healthTMP.text = "80 / 80";
        sanityTMP.text = "80 / 80";

        StartTurn();
    }

    #region Turn

    public void StartTurn()
    {
        MakeSlots(slotAmount);
    }

    public void EndTurn()
    {
        Debug.Log("<<END TURN>>");
        
        List<Slot> timedSlotList = new List<Slot>();
        foreach (SlotSet slotSet in slots)
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

    List<Slot> GetTimedSlots(SlotSet slotset)
    {
        if (slotset.mySlot.slotedCard == null || slotset.enemySlot.slotedCard == null)
        {
            return null;
        }


        if (slotset.mySlot.slotedCard.cardData.speed < slotset.enemySlot.slotedCard.cardData.speed)
        {
            return new List<Slot>() {slotset.mySlot, slotset.enemySlot};
        }
        else if (slotset.mySlot.slotedCard.cardData.speed > slotset.enemySlot.slotedCard.cardData.speed)
        {
            return new List<Slot>() {slotset.enemySlot, slotset.mySlot};
        }
        else
        {
            return new List<Slot>() {slotset.mySlot, slotset.enemySlot}; // TODO: 방어/공격 서순 정하기
        }
    }

    #endregion

    #region Slot

    void SlotMoveTransform(Transform obj, PRS prs, float dotweenTime)
    {
        obj.DOMove(prs.pos, dotweenTime);
        obj.DORotateQuaternion(prs.rot, dotweenTime);
        obj.DOScale(prs.scale, dotweenTime);
    }

    void SlotSetsAlignment()
    {
        var targetSlots = slots;
        for (int i = 0; i < targetSlots.Count; i++)
        {
            var targetSlotSet = targetSlots[i].mySlot.transform.parent;
            var newPosition = new Vector3(targetSlotSet.position.x + 5 * i, targetSlotSet.position.y, targetSlotSet.position.z);
            var newPRS = new PRS(newPosition, targetSlotSet.transform.rotation, targetSlotSet.transform.localScale);
            SlotMoveTransform(targetSlotSet, newPRS, 0.3f);
        }
    }

    Slot MakeSlot()
    {
        var slotObj = Instantiate(slotPrefab, slotGenPos.position, Utils.QI);
        var slot = slotObj.GetComponent<Slot>();

        slotObj.name = ("Slot " + UnityEngine.Random.Range(0, 1000).ToString());
        return slot;
    }

    SlotSet MakeSlotSets()
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

        return new SlotSet() {mySlot = slotMy, enemySlot = slotEnemy};
    }

    Card GetEnemyCard(Slot slotEnemy)
    {
        Card tempCard = CardManager.Inst.MakeCard(new CardData());

        return tempCard;
    }

    void MakeSlots(int amount)
    {
        for (int i = 0; i<amount; i++)
        {
            SlotSet slotSet = MakeSlotSets();
            slots.Add(slotSet);
            SlotSetsAlignment();
        }
    }

    #endregion
}
