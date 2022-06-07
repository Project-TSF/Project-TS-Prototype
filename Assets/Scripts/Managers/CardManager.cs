using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] int handDeckCountLimit;

    [Space]

    [SerializeField] GameObject cardPrefab;

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

    GameObject targetSlotable;

    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;


    private void Start()
    {
        SetupSampleDeck();
        SetupAvailableDeck();
        SetupNGCard();

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
        availableDeck = allCardDeck;
        foreach (Card card in availableDeck)
        {
            card.transform.parent = availableDeckPos;
        }
    }

    void SetupSampleDeck()
    {
        allCardDeck = new List<Card>() {
            MakeCard(new CardData()), // TODO: 
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
            MakeCard(new CardData()),
        };
    }

    public Card MakeCard(CardData cardData) // 카드 Instantiate
    {
        var cardObj = Instantiate(cardPrefab, availableDeckPos.position, Utils.QI);
        var card = cardObj.GetComponent<Card>();
        card.Setup(cardData);
        card.setVisible(false);

        cardObj.name = ("Card " + card.cardData.cardName + UnityEngine.Random.Range(0, 1000).ToString());
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

    void addTohandDeck() // 카드뽑기
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
        for (int i = 0; i < pickNum; i++)
        {
            addTohandDeck();
        }
    }

    void DiscardCard(Card card)
    {
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
        Debug.Log("Return || Card: " + card.name);

        card.transform.parent = handDeckCollider.transform;

        card.slot.GetComponent<Slot>().slotedCard = null;
        card.slot = handDeckCollider;

        handDeck.Add(card);
    }

    void replaceCard(Slot slot, Card originCard, Card replaceCard)
    {
        Debug.Log("Replace || OriginCard: " + originCard.name + " | " + "ReplaceCard: " + replaceCard.name + " | " + "Slot: " + slot.name);
        returnHandDeck(originCard);
        fitSlotable(replaceCard, slot);
        handDeck.Remove(replaceCard);
    }

    void GetTargetEntity()
    {
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
        Debug.Log("==================MOUSE UP===================");
        GetTargetEntity();
        Debug.Log(card.slot?.name + " -> " + targetSlotable?.name);

        if (card.slot.GetComponent<Slot>()?.isMoveable == false)
        {
            return;
        }

        if (!targetSlotable)
        {
            if (card.cardData.isNGCard)
            {
                Debug.Log("Destroy NG Card");
                Destroy(card.gameObject);

                if (card.slot.GetComponent<Slot>())
                    return;
                
                SetupNGCard();
                return;
            }
            Debug.Log("Not Target");
            card.MoveTransform(card.originPRS, true, 0.1f);

            return;
        }

        if (card.slot == targetSlotable)
        {
            Debug.Log("Same Target");
            card.MoveTransform(card.originPRS, true, 0.1f);

            return;
        }

        Slot targetSlot = targetSlotable.GetComponent<Slot>();

        if (targetSlot?.isMoveable != true && targetSlotable != handDeckCollider)
        {
            Debug.Log("Not Moveable");
            card.MoveTransform(card.originPRS, true, 0.1f);

            return;
        }


        if (card.slot == handDeckCollider && targetSlot)
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

        else if (card.slot.GetComponent<Slot>() && targetSlot)
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

        else if (card.slot.GetComponent<Slot>() && targetSlotable == handDeckCollider)
        {
            if (card.cardData.isNGCard)
            {
                card.MoveTransform(card.originPRS, true, 0.1f);
            }
            else
            {
                // 슬롯 -> 손패
                returnHandDeck(card);
                CardAlignment();
            }
        }

        else if (card.slot == NGCardSlot && targetSlotable == handDeckCollider)
        {
            Debug.Log("Destroy NG Card");
            Destroy(card.gameObject);
            SetupNGCard();
        }

        else if (card.slot == NGCardSlot && targetSlot)
        {
            if (targetSlot.slotedCard)
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

    internal void CardMouseDrag(Card card)
    {
        if (card.slot.GetComponent<Slot>())
        {
            if (!card.slot.GetComponent<Slot>().isMoveable) return;
        }

        card.MoveTransform(new PRS(Utils.MousePos, card.transform.rotation, card.transform.localScale), false);
    }

    #endregion

    #endregion


    #region Effect



    #endregion
}
