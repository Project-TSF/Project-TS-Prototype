using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System.Threading.Tasks;
using System;

public class BattleManager : MonoBehaviour
{
    public Encounter currentEncounter;

    [Header("Slot Panel")]

    [SerializeField] GameObject slotPanel;
    [SerializeField] Slot slotPrefab;
    [SerializeField] Transform slotGenPos;
    [SerializeField] Transform slotStartPos;
    [SerializeField] List<SlotSet> slotsets;
    public int slotAmount;


    [Header("Props")]

    [SerializeField] GameObject propPrefab;
    [SerializeField] Transform propStartPos;
    public List<AbstractProp> propList = new List<AbstractProp>();


    [Header("Current Game Status")]

    public int maxActNum;
    public int currentActNum;


    //Events
    public delegate void onPostEncounterInitialize();
    public static event onPostEncounterInitialize OnPostEncounterInitializeEvent;

    public delegate void onTurnStart();
    public static event onTurnStart OnTurnStartEvent;
    public delegate void onTurnEnd();
    public static event onTurnEnd OnTurnEndEvent;


    public static BattleManager Inst { get; private set; }
    void Awake() => Inst = this;


    // DEBUG
    DataLoader dataLoader = new DataLoader();

    private void Start()
    {
        InitializeBattleEncounter();
    }

    public void InitializeBattleEncounter()
    {
        this.currentEncounter = GameManager.Inst.currentEncounter;

        // 디버그용
        dataLoader.AddPropToPool<TestProp_Blueberries>();
        dataLoader.AddPropToPool<TestProp_TimeBomb>();


        PawnManager.Inst.InstEnemy<PawnEnemy_Spade>();


        dataLoader.poolProp[typeof(TestProp_Blueberries).ToString()].Invoke();
        

        // 여기까지 디버그용

        maxActNum = currentEncounter.pattern.acts.Count - 1;
        currentActNum = 0;

        OnPostEncounterInitializeEvent?.Invoke();

        StartTurn();
        UpdateUI();
    }

    public void UpdateUI()
    {
        PawnManager.Inst.UpdateUI();
        propList.ForEach(prop => prop.UpdateText());
        AlignProps();
    }

    #region Turn

    public void StartTurn() // 턴 시작
    {
        Debug.Log("<<StartTurn>>");

        OnTurnStartEvent?.Invoke();

        MakeSlotsetAsAmount(slotAmount);

        if (currentActNum > maxActNum) // 모든 Act가 지나면 다시 0부터 시작
        {
            currentActNum = 0;
        }

        for (int i = 0; i < slotsets.Count; i++)
        {
            GetEnemyCard(i);
        }

        CardManager.Inst.PickupCards(6);

        CardManager.Inst.isCardSelectable = true;
    }

    public async void EndTurn() // Turn End 버튼이 눌렸을 때
    {
        Debug.Log("<<END TURN>>");

        Queue<Slot> timedQueue = GetTimedQueue();
        if (timedQueue == null)
        {
            Debug.Log("Slots are not full");

            return;
        }

        OnTurnEndEvent?.Invoke();

        await GameObject.Find("BtnTogglePanel").GetComponent<UI_BtnTogglePanel>().ClosePanel();
        GameObject.Find("BtnTogglePanel").GetComponent<UI_BtnTogglePanel>().isAvailable = false;
        CardManager.Inst.isCardSelectable = false;

        foreach (Slot slot in timedQueue)
        {
            if (!Application.isPlaying) break;

            Debug.Log($"Use {slot.slotedCard.cardName}");
            await slot.slotedCard.UseEffect();
        }

        Debug.Log("<<END TURN>>");
        GameObject.Find("BtnTogglePanel").GetComponent<UI_BtnTogglePanel>().isAvailable = true;
        await GameObject.Find("BtnTogglePanel").GetComponent<UI_BtnTogglePanel>().OpenPanel();

        await CardManager.Inst.ClearHandDeck();

        currentActNum += 1;
        StartTurn();
    }

    #endregion

    #region Slot

    void SlotMoveTransform(Transform obj, PRS prs, float dotweenTime) // 슬롯 움직이는 함수
    {
        obj.DOMove(prs.pos, dotweenTime);
        obj.DORotateQuaternion(prs.rot, dotweenTime);
        obj.DOScale(prs.scale, dotweenTime);
    }

    void AlignSlotSets() // 슬롯 정렬하는 함수
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
        SlotSetObj.transform.SetParent(slotStartPos.transform);

        // Slot들을 SlotSet 안에 넣어주기
        slotMy.transform.SetParent(SlotSetObj.transform);
        slotEnemy.transform.SetParent(SlotSetObj.transform);

        // SlotSet 안에서 Slot들의 위치 조절
        slotMy.transform.localPosition = new Vector3(0, 0, 0);
        slotEnemy.transform.localPosition = new Vector3(0, 8, 0);


        return new SlotSet() { mySlot = slotMy, enemySlot = slotEnemy };
    }

    void GetEnemyCard(int enemyCardIndex) // 적 카드를 Enemy에서 불러와 return하는 함수
    {
        Slot slot = slotsets[enemyCardIndex].enemySlot;
        
        Type cardType = currentEncounter.pattern.acts[currentActNum].cards[enemyCardIndex];
        AbstractCard card = CardManager.Inst.InstCard(cardType);

        // 적 카드 받아오기인데 나중에 위에 디버그쪽에 짜둔거 GetEnemyCard로 옮기기
        slot.slotedCard = card;

        // 카드 내부 데이터 초기화
        slot.slotedCard.slot = slot.gameObject;
        
        // 적 카드를 적 슬롯 위에 놓기
        slot.slotedCard.transform.parent = slot.transform;
        slot.slotedCard.transform.localPosition = new Vector3(0, 0, 0);

        slot.slotedCard.originPRS = new PRS(
            slot.slotedCard.transform.position,
            Utils.QI,
            slot.slotedCard.transform.localScale);
        slot.slotedCard.setVisible(true);

        card.UpdateUI();
    }

    void MakeSlotsetAsAmount(int amount) // slotset 여러개를 스폰하는 함수
    {
        int currentSlotCount = slotsets.Count;
        if (currentSlotCount > amount)
        {
            for (int i = 0; i < currentSlotCount - amount; i++)
            {
                Destroy(slotsets[i].mySlot);
                Destroy(slotsets[i].enemySlot);
            }
            slotsets.RemoveRange(0, currentSlotCount - amount);
            AlignSlotSets();
        }
        else if (currentSlotCount < amount)
        {
            for (int i = 0; i < amount - currentSlotCount; i++)
            {
                slotsets.Add(MakeSlotSets());
            }
            AlignSlotSets();
        }

        // Cleanse the data of slots in slotsets
        for (int i = 0; i < slotsets.Count; i++)
        {
            slotsets[i].mySlot.slotedCard = null;
            slotsets[i].enemySlot.slotedCard = null;
        }
    }

    Queue<Slot> GetTimedQueue() //slotset에서 slot에 있는 카드가 빠른 순으로 정렬해서 리턴
    {
        Queue<Slot> timedQueue = new Queue<Slot>();

        foreach (SlotSet slotset in slotsets)
        {
            if (slotset.mySlot.slotedCard == null)
            {
                return null;
            }

            if (slotset.mySlot.slotedCard.speed < slotset.enemySlot.slotedCard.speed)
            {
                timedQueue.Enqueue(slotset.mySlot);
                timedQueue.Enqueue(slotset.enemySlot);
            }
            else if (slotset.mySlot.slotedCard.speed > slotset.enemySlot.slotedCard.speed)
            {
                timedQueue.Enqueue(slotset.enemySlot);
                timedQueue.Enqueue(slotset.mySlot);
            }
            else
            {
                timedQueue.Enqueue(slotset.mySlot);
                timedQueue.Enqueue(slotset.enemySlot);
            }
        }

        return timedQueue;
    }

    #endregion

    #region Prop
    
    public AbstractProp InstProps<T>(string propID = null) where T : AbstractProp
    {
        GameObject propObj = Instantiate(propPrefab);
        AbstractProp prop = propObj.AddComponent<T>() as AbstractProp;

        AfterWorkPropInst(prop, propID);

        return prop;
    }

    public AbstractProp InstProps(string propID, Type propType)
    {
        GameObject propObj = Instantiate(propPrefab);
        AbstractProp prop = propObj.AddComponent(propType) as AbstractProp;

        AfterWorkPropInst(prop, propID);

        return prop;
    }

    private void AfterWorkPropInst(AbstractProp prop, string propID)
    {
        if (propID == null)
        { 
            prop.ID = prop.GetType().ToString();
        }
        else 
        {
            prop.ID = propID;
        } 
        prop.name = prop.propName;

        prop.transform.SetParent(propStartPos.transform, false);
        prop.OnEquip();

        propList.Add(prop);

        AlignProps();
    }

    public void AlignProps()
    {
        var i = 0;
        foreach (AbstractProp prop in propList)
        {
            prop.transform.localPosition = new Vector3(100 * i, 0, 0);
            i++;
        }
    }



    #endregion

}
