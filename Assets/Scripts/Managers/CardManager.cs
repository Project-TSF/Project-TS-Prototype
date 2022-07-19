using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] int handDeckCountLimit; // handdeck에 저장할 카드의 최대 개수

    [Space]

    [SerializeField] GameObject cardPrefab; // 카드 프리펩

    [Space]

    [SerializeField] Transform availableDeckPos;
    [SerializeField] Transform handDeckPos;
    [SerializeField] Transform discardDeckPos;
    [SerializeField] GameObject NGCardSlot;
    [SerializeField] GameObject handDeckCollider;

    [Space]

    [SerializeField] List<Card> allCardDeck;
    [SerializeField] List<Card> availableDeck;
    [SerializeField] List<Card> handDeck;
    [SerializeField] List<Card> discardDeck;

    [Space]

    [SerializeField] GameObject targetSlotable; // 카드를 드래그 하다 마우스를 놓았을 때 그 밑에 있던 오브젝트중 Slotable한 오브젝트

    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;


    private void Start()
    {
        // 카드 덱 초기화
        SetupSampleDeck();
        SetupAvailableDeck();
        SetupNGCard();

        // 디버그용으로 카드 생성하는 코드
        addTohandDeck();
        addTohandDeck();
        addTohandDeck();
        addTohandDeck();
        addTohandDeck();
        addTohandDeck();

        CardAlignment();
    }

    private void SetupNGCard()
    {
        // NG카드를 오른쪽 밑에 생성하는 코드
        Card NGCard = MakeCard(new CardData() { isNGCard = true });
        NGCard.name = ("NGCard " + NGCard.cardData.cardName + UnityEngine.Random.Range(0, 1000).ToString());
        NGCard.transform.parent = NGCardSlot.transform;
        NGCard.transform.position = NGCardSlot.transform.position;

        NGCard.slot = NGCardSlot;
        NGCard.originPRS = new PRS(NGCard.transform.position, Utils.QI, NGCard.transform.localScale);
        NGCard.setVisible(true);
    }


    #region Deck

    void SetupAvailableDeck()
    {
        // 현재 가지고 있는 카드들을 불러와 이번 전투에서 사용할 카드덱에 채워넣음
        availableDeck = allCardDeck;
        foreach (Card card in availableDeck)
        {
            card.transform.parent = availableDeckPos;
        }
    }

    void SetupSampleDeck()
    {
        TempCard tempcard = new TempCard();
        // 디버그용 샘플덱 생성 코드
        allCardDeck = new List<Card>() {
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
            MakeCard(tempcard.GetTempCard()),
        };
    }

    public Card MakeCard(CardData cardData) // 카드 Instantiate하는 코드
    {
        var cardObj = Instantiate(cardPrefab, availableDeckPos.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(cardData);
        card.setVisible(false);

        cardObj.name = card.cardData.cardName;
        return card;
    }

    List<Card> ShuffleDeck(List<Card> deck) // 덱 셔플
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[rand];
            deck[rand] = temp;
        }
        return deck;
    }

    void ResetAvailableDeck() // discardDeck 에서 AvailableDeck으로 넘기기
    {
        int leftCardNum = discardDeck.Count;
        for (int i = 0; i < leftCardNum; i++)
        {
            Card card = discardDeck[0];
            availableDeck.Add(card);
            discardDeck.RemoveAt(0);
            card.MoveTransform(new PRS(availableDeckPos.position, Utils.QI, card.originPRS.scale), false);
        }

        ShuffleDeck(availableDeck);
    }

    void addTohandDeck() // 카드 1개 뽑기
    {
        if (availableDeck.Count == 0) // 덱에 아무 카드도 없을시 덱 재생성
        {
            if (discardDeck.Count == 0)
                return;

            ResetAvailableDeck();
        }

        Card card = availableDeck[0];
        handDeck.Add(card);
        card.setVisible(true);
        card.transform.parent = handDeckCollider.transform;
        card.slot = handDeckCollider;
        availableDeck.RemoveAt(0);

        // SetOriginOrder();
        CardAlignment();
    }

    void PickupCards(int pickNum)
    {
        // pickupNum개의 카드를 handDeck로 보내는 코드
        for (int i = 0; i < pickNum; i++)
        {
            addTohandDeck();
        }
    }

    void DiscardCard(Card card)
    {
        // 카드를 discardDeck로 보내는 코드
        discardDeck.Add(card);
        handDeck.Remove(card);

        card.MoveTransform(new PRS(discardDeckPos.position, Utils.QI, card.originPRS.scale), true, 0.1f);

        card.setVisible(false);

        // SetOriginOrder();
        CardAlignment();
    }

    #endregion


    #region CardAlignnment

    void CardAlignment()
    {
        // handdeck의 카드를 정렬시켜주는 코드
        var targetCards = handDeck;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];
            var newPosition = new Vector3(handDeckPos.position.x + 5 * i, handDeckPos.position.y, handDeckPos.position.z);
            var newPRS = new PRS(newPosition, targetCard.transform.rotation, targetCard.transform.localScale);
            targetCard.originPRS = newPRS;
            targetCard.MoveTransform(newPRS, true, 0.3f);
        }
    }

    #endregion


    #region CardMovementControl

    void fitSlotable(Card card, Slot slot)
    {
        /* HandDeck에 있던 카드를 Slot에 꽂는 코드
        Card card: HandDeck에 있던 카드
        Slot slot: 카드를 꽂을 대상이 되는 slot */
        Debug.Log(card.name);
        Debug.Log(slot.name);
        Debug.Log("FIT || Card: " + card.name + " | " + "Slot: " + slot.name);

        var newPRS = new PRS(slot.transform.position, card.transform.rotation, card.transform.localScale);
        card.originPRS = newPRS;
        card.MoveTransform(newPRS, true, 0.1f);
        card.transform.parent = slot.transform;

        card.slot = slot.gameObject;
        slot.slotedCard = card;
    }

    void returnHandDeck(Card card)
    {
        /* Slot에 있던 카드를 HandDeck으로 내리는 코드
        Card card: 내리는 대상인 카드 */
        Debug.Log("Return || Card: " + card.name);

        card.transform.parent = handDeckCollider.transform;

        card.slot.GetComponent<Slot>().slotedCard = null;
        card.slot = handDeckCollider;

        handDeck.Add(card);
    }

    void replaceCard(Slot slot, Card originCard, Card replaceCard)
    {
        /* handdeck에 있던 replaceCard와 slot에 꽂혀있던 originCard를 서로 바꾸는 코드.
        Slot slot: 타겟 슬롯
        Card originCard: 타겟 슬롯에 있던 카드
        Card replaceCard: handdeck에 있던 카드
         */
        Debug.Log("Replace || OriginCard: " + originCard.name + " | " + "ReplaceCard: " + replaceCard.name + " | " + "Slot: " + slot.name);
        returnHandDeck(originCard);
        fitSlotable(replaceCard, slot);
        handDeck.Remove(replaceCard);
    }

    void GetTargetEntity()
    {
        // 카드를 놓았을 때 마우스가 어느 오브젝트 위에서 놓아졌는지 리턴하는 코드
        bool existTarget = false;

        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward, Mathf.Infinity);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == 7)
            {
                GameObject cEntity = hit.collider?.gameObject;
                if (cEntity != null)
                {
                    targetSlotable = cEntity;
                    existTarget = true;
                    break;
                }
            }
        }
        if (!existTarget)
            targetSlotable = null;
    }

    #region CardMouse

    internal void CardMouseOver(Card card)
    {

    }

    internal void CardMouseExit(Card card)
    {

    }

    internal void CardMouseDown(Card card)
    {
        // card.originPRS = new PRS(card.transform.position, card.transform.rotation, card.transform.localScale);
    }

    internal void CardMouseUp(Card card)
    {
        // TODO: 이거 가끔 카드가 안잡힐 때가 있는거 같은데 애초에 카드를 클릭했다고 인식도 안됨. 아마 TargetSlotable 변수의 업데이트 문제같아오
        // 카드를 움직였을때 어떤 위치로 보내는가 판별하는 코드 TODO: 이거 다른 곳으로 정리해서 옮기기
        Debug.Log("==================MOUSE UP===================");
        GetTargetEntity(); // 카드를 놓았을 때 마우스가 어느 오브젝트 위에서 놓아졌는지 리턴하는 코드
        Debug.Log(card.slot?.name + " -> " + targetSlotable?.name);

        if (card.slot.GetComponent<Slot>()?.isMoveable == false)
        {
            // 만약 들었던 카드의 슬롯이 이 카드는 움직일 수 없다고 하면 무시하고 종료.
            return;
        }

        if (!targetSlotable)  // 밑에 Slotable한 오브젝트가 없을 때
        {
            if (card.cardData.isNGCard)  // 만약 카드가 NG카드라면 그 카드를 파괴하고 원래 NG카드 위치에 재생성함
            {
                Debug.Log("Destroy NG Card");
                Destroy(card.gameObject);

                if (card.slot.GetComponent<Slot>())
                    return;

                SetupNGCard();
                return;
            }
            // NG카드가 아니라면 원래 위치로 복귀시킴
            Debug.Log("Not Target");
            card.MoveTransform(card.originPRS, true, 0.1f);

            return;
        }

        if (card.slot == targetSlotable) // 밑에 Slotable한 오브젝트가 자기 자신일 때 카드를 원래 위치로 돌림
        {
            Debug.Log("Same Target");
            card.MoveTransform(card.originPRS, true, 0.1f);

            return;
        }

        Slot targetSlot = targetSlotable.GetComponent<Slot>();

        if (targetSlot?.isMoveable == false && targetSlotable != handDeckCollider) // Not Moveable한 슬롯에 카드를 꽂으려 할 때 원래 위치로 돌림
        {
            Debug.Log("Not Moveable");
            card.MoveTransform(card.originPRS, true, 0.1f);

            return;
        }


        if (card.slot == handDeckCollider && targetSlot) // 카드를 손패에서 ->
        {
            if (targetSlot.slotedCard)
            {
                // 손패 <-> 슬롯 바꾸기
                replaceCard(targetSlot, targetSlot.slotedCard, card);
            }
            else
            {
                // 손패 -> 슬롯
                fitSlotable(card, targetSlot);
                handDeck.Remove(card);
            }

            CardAlignment(); // TODO: replace/fit 하고 CardAlign하기 카드 제대로 안 돌아 온 상태에서 카드 갈아 끼우면 제대로 안 움직여짐
            // TODO: 이거 처리하는거 큐 쓰면 해결 될지도?
        }

        else if (card.slot.GetComponent<Slot>() && targetSlot) // 카드를 슬롯에서 ->
        {
            // 슬롯 <-> 슬롯 바꾸기
            Card cardA = card;
            Slot slotA = card.slot.GetComponent<Slot>();
            Card cardB = targetSlot.slotedCard;
            Slot slotB = targetSlot;

            if (cardB)
            {   // targetSlot에 카드가 있으면 -> 교체
                fitSlotable(cardA, slotB);
                fitSlotable(cardB, slotA);
            }
            else
            {   // targetSlot에 카드가 없으면 -> 옮기기
                fitSlotable(cardA, slotB);
                slotA.slotedCard = null;
            }
        }

        else if (card.slot.GetComponent<Slot>() && targetSlotable == handDeckCollider) // 카드를 슬롯에서 -> handDeck으로
        {
            if (card.cardData.isNGCard) // 카드가 NG카드라면 원래 위치(슬롯)으로 복귀시킴: TODO: 파괴시키는게 나을까요?
            {
                card.MoveTransform(card.originPRS, true, 0.1f);
            }
            else // 슬롯 -> 손패
            {
                
                returnHandDeck(card);
                CardAlignment();
            }
        }

        else if (card.slot == NGCardSlot && targetSlotable == handDeckCollider) // NG카드를 NG카드 덱에서 handdeck으로 옮기려고 함 -> 파괴시킴
        {
            Debug.Log("Destroy NG Card");
            Destroy(card.gameObject);
            SetupNGCard();
        }

        else if (card.slot == NGCardSlot && targetSlot) // NG카드를 NG카드 덱에서 슬롯으로 옮기려고 할 때
        {
            if (targetSlot.slotedCard) // 슬롯된 카드가 있다면 파괴시킴: TODO: 교체하는게 나을지도?
            {
                Debug.Log("Destroy NG Card");
                Destroy(card.gameObject);
                SetupNGCard();
            }
            else
            {
                // NG슬롯 NG카드 -> 슬롯
                fitSlotable(card, targetSlot);
                SetupNGCard();
            }
        }


    }

    internal void CardMouseDrag(Card card) // 카드 드래그
    {
        if (card.slot.GetComponent<Slot>())
        {
            if (!card.slot.GetComponent<Slot>().isMoveable) return; // 카드의 슬롯이 있고, Moveable하지 않으면 드래그 하지 않음
        }

        card.MoveTransform(new PRS(Utils.MousePos, card.transform.rotation, card.transform.localScale), false);
    }

    #endregion

    #endregion


    #region Loading

    public Enemy ReadCardFromJson()
    {
        // Instantiate Enemy from Enemy Prefab and Json Enemy Data
        var enemyObj = Instantiate(cardPrefab);
        var enemy = enemyObj.GetComponent<Enemy>();
        var jsonfile = Resources.Load<TextAsset>("testCardjson");
        JsonUtility.FromJsonOverwrite(jsonfile.text, enemy);

        return enemy;
    }

    #endregion
}
