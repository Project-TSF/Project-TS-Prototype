using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class PawnManager : MonoBehaviour
{


    [SerializeField] Transform playerPos;   // 플레이어가 평소 서 있는 위치. 현재 위치와 헷갈릴 수 있다.
    [SerializeField] Transform enemyPos;

    public PawnPlayer player;
    public List<AbstractPawnEnemy> enemyList = new List<AbstractPawnEnemy>();
    [SerializeField] AbstractPawnEnemy enemyPrefab;

    [Space]

    [SerializeField] GameObject floatingTextPrefab;

    public static PawnManager Inst { get; private set; }
    void Awake() => Inst = this;


    void Start()
    {

    }

    public void UpdateUI()
    {
        player.UpdateUI();

        for (var i = 0; i < enemyList.Count; ++i)
        {
            AbstractPawnEnemy enemy = enemyList[i];
            enemy.UpdateUI();
        }

        AlignEnemy();
    }

    public void PawnMove(Pawn pawn, PRS prs, float dotweenTime) // 폰 움직이는 함수
    {
        pawn.transform.DOMove(prs.pos, dotweenTime);
        pawn.transform.DORotateQuaternion(prs.rot, dotweenTime);
        pawn.transform.DOScale(prs.scale, dotweenTime);
    }
    
    public void InstFloatingText(Pawn pawn, string text, Color color, float moveSpeed=1.5f)
    {
        var floatingText = Instantiate(floatingTextPrefab);
        floatingText.GetComponent<FloatingText>().StartFloating(pawn, text, color, moveSpeed);
    }

    #region Enemy

    public void GetTestEnemy()
    {
        // enemyList.Add(ReadEnemyFromJson());
        var enemy = Instantiate(enemyPrefab, enemyPos);
        var enemyData = enemy.GetComponent<AbstractPawnEnemy>();
        var tempEnemy = new TempEnemy();
        enemyList.Add(tempEnemy.GetTempEnemySpade(enemyData));
        UpdateUI();
    }

    public AbstractPawnEnemy ReadEnemyFromJson()
    {
        // Instantiate Enemy from Enemy Prefab and Json Enemy Data
        // 테스트용으로 만들어진 json에서 1개 불러오도록 만든거니까 나중에 json구조 여러명 넣어지게 바뀌면 얘도 바꿔야할듯
        var enemyObj = Instantiate(enemyPrefab, enemyPos);
        var enemy = enemyObj.GetComponent<AbstractPawnEnemy>();
        var jsonfile = Resources.Load<TextAsset>("testEnemyjson");
        JsonUtility.FromJsonOverwrite(jsonfile.text, enemy);

        enemy.healthTMP.text = enemy.health + " / " + enemy.maxHealth;
        enemy.sanityTMP.text = enemy.sanity + " / " + enemy.maxSanity;
        enemy.shieldTMP.text = enemy.shield.ToString();

        return enemy;
    }

    public void AlignEnemy() // 폰 정렬하는 함수
    {
        var targetPawns = enemyList;
        for (int i = 0; i < targetPawns.Count; i++)
        {
            var targetPawn = targetPawns[i].transform;
            var newPosition = new Vector3(enemyPos.position.x + i * 5, targetPawn.position.y, targetPawn.position.z);
            var newPRS = new PRS(newPosition, targetPawn.transform.rotation, targetPawn.transform.localScale);
            PawnMove(targetPawn.GetComponent<Pawn>(), newPRS, 0);
        }
    }


    #endregion

}