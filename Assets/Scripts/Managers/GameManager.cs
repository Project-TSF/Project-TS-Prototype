using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    public DataLoader dataLoader = new DataLoader();

    public delegate void onPostInitialize();
    public static event onPostInitialize OnPostInitializeEvent;

    private void Awake() {
        if (Inst != null) {
            Destroy(gameObject);
            return;
        }
        else if (Inst == null) {
            Inst = this;
        }
        DontDestroyOnLoad(gameObject);

        // 로딩
        // dataLoader.AddMonster("testMonsterGroup", );
        // dataLoader.AddBattleEncounter("testBattleEncounter", );
        dataLoader.AddPropToPool<TestProp_Blueberries>(typeof(TestProp_Blueberries).ToString());
        dataLoader.AddPropToPool<TestProp_TimeBomb>("TestProp_Timebomb");

        OnPostInitializeEvent?.Invoke();
    }

    public void LoadTestBattleEncounter() {
        SceneManager.LoadScene("BattleScene");
    }


}
