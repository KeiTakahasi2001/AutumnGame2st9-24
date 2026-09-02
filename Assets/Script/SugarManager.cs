using UnityEngine;
using TMPro; // 画面に文字（UI）で表示するために使います！


public class SugarManager : MonoBehaviour
{
    // 他のスクリプトから「何個持ってるか」「減らせるか」を簡単に確認できるようにする仕組み
    public static SugarManager Instance { get; private set; }

    [SerializeField] private int currentSugar = 50;     // 最初から持っている金平糖の数
    [SerializeField] private int sugarIncreaseRate = 10; // 1秒ごとに増える量
    [SerializeField] private float increaseInterval = 1f; // 何秒ごとに増やすか

    [SerializeField] private TextMeshProUGUI sugarText; // 画面上の文字（UI）への連動用

    private float timer = 0f;

    void Awake()
    {
        // マネージャーの定番の仕組み（どこからでもアクセスできるようにする）
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateSugarUI();
    }

    void Update()
    {
        // 時間経過で金平糖を増やすタイマー
        timer += Time.deltaTime;
        if (timer >= increaseInterval)
        {
            currentSugar += sugarIncreaseRate;
            timer = 0f;
            UpdateSugarUI();
            // Debug.Log("金平糖が増えたよ！ 現在の数: " + currentSugar);
        }
    }

    // 金平糖が足りるか確認して、足りたら減らすメソッド（タワーを置くときに使う！）
    public bool TryConsumeSugar(int amount)
    {
        if (currentSugar >= amount)
        {
            currentSugar -= amount;
            UpdateSugarUI();
            return true; // 消費成功！
        }
        else
        {
            Debug.Log("金平糖が足りないよ……！");
            return false; // 消費失敗（お金不足）
        }
    }

    // 金平糖を増やすメソッド（将来、足止めキャラが生産したときにも使える！）
    public void AddSugar(int amount)
    {
        currentSugar += amount;
        UpdateSugarUI();
    }

    // 画面のテキストを更新する
    private void UpdateSugarUI()
    {
        if (sugarText != null)
        {
            sugarText.text = "Sugar : " + currentSugar;
        }
    }
}