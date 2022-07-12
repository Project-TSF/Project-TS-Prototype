using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class PawnManager : MonoBehaviour
{


    [SerializeField] Transform playerPos;   // 플레이어가 평소 서 있는 위치. 현재 위치와 헷갈릴 수 있다.
    [SerializeField] Transform enemyPos;

    public Player player;
    public List<Enemy> enemyList;
    [SerializeField] Enemy enemyPrefab;

    public static PawnManager Inst { get; private set; }
    void Awake() => Inst = this;


    void Start()
    {
        // TODO: 디버그

        // var tempEnemyGen = new TempEnemy();

        player.ID = "Player";

        player.modifier_normal_attack = 0;
        player.modifier_defend = 0;


        enemyList = new List<Enemy>();
        
        // for (var i = 0; i < 1; ++i)
        // {
        //     var tempEnemy = tempEnemyGen.Get_TempEnemy_Spade(MakeEnemy());
        //     enemyList.Add(tempEnemy);
        // }

        // TOJSON
        // string json = JsonUtility.ToJson(enemyList[0]);
        // Debug.Log(json);

        // FROMJSON
        // enemyList.Add(ReadEnemyFromJson());

        UpdateUI();

        // 여기까지
    }

    public void UpdateUI()
    {
        player.healthTMP.text = player.health + " / " + player.maxHealth;
        player.sanityTMP.text = player.sanity + " / " + player.maxSanity;

        for (var i = 0; i < enemyList.Count; ++i)
        {
            var enemy = enemyList[i];
            enemy.healthTMP.text = enemy.health + " / " + enemy.maxHealth;
            enemy.sanityTMP.text = enemy.sanity + " / " + enemy.maxSanity;
        }

        EnemyAlignment();
    }


    public void PawnMove(Pawn pawn, PRS prs, float dotweenTime) // 폰 움직이는 함수
    {
        pawn.transform.DOMove(prs.pos, dotweenTime);
        pawn.transform.DORotateQuaternion(prs.rot, dotweenTime);
        pawn.transform.DOScale(prs.scale, dotweenTime);
    }

    #region Enemy

    public Enemy ReadEnemyFromJson()
    {
        // Instantiate Enemy from Enemy Prefab and Json Enemy Data
        var enemyObj = Instantiate(enemyPrefab);
        var enemy = enemyObj.GetComponent<Enemy>();
        var jsonfile = Resources.Load<TextAsset>("testEnemyjson");
        JsonUtility.FromJsonOverwrite(jsonfile.text, enemy);

        enemy.healthTMP.text = enemy.health + " / " + enemy.maxHealth;
        enemy.sanityTMP.text = enemy.sanity + " / " + enemy.maxSanity;

        return enemy;
    }

    public void EnemyAlignment() // 폰 정렬하는 함수
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