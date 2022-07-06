using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : Pawn
{
    // pattern of the pawn
    public Pattern pattern;

    // trigger
    public Trigger trigger;


    public TMPro.TMP_Text healthTMP; //TODO: 카드를 바꾸던 얘를 바꾸던 하나는 바꿔서 똑같이 만들어야 할듯. 보이는 부분이랑 Data부분이랑 분리시킬지 말지
    public TMPro.TMP_Text sanityTMP;
}