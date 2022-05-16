using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    [SerializeField] Transform SlotStartPos;

    [Space]

    [SerializeField] Slot slotPrefab;
    [SerializeField] Transform slotGenPos;
    [SerializeField] List<List<Slot>> slots;
    public int slotAmount;
    int slotGenCursor = 0;

    [Space]

    [SerializeField] TMP_Text healthTMP;
    [SerializeField] TMP_Text sanityTMP;


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
            var targetSlotSet = targetSlots[i][0].transform.parent;
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

    List<Slot> MakeSlotSets()
    {
        GameObject SlotSetObj = new GameObject("SlotSet " + UnityEngine.Random.Range(0, 1000).ToString());

        Slot slotMy = MakeSlot();
        Slot slotEnemy = MakeSlot();

        SlotSetObj.transform.parent = SlotStartPos.transform;

        slotMy.transform.parent = SlotSetObj.transform;
        slotEnemy.transform.parent = SlotSetObj.transform;

        slotMy.transform.localPosition = new Vector3(0, 0, 0);
        slotEnemy.transform.localPosition = new Vector3(0, 8, 0);

        return new List<Slot>() {slotMy, slotEnemy};
    }

    void MakeSlots(int amount)
    {
        for (int i = 0; i<amount; i++)
        {
            slots.Add(MakeSlotSets());
            SlotSetsAlignment();
        }
    }

    #endregion
}
