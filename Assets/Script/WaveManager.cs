using UnityEngine;
using System.Collections;
using TMPro; // UIでWave数やクリアを表示するため

public class WaveManager : MonoBehaviour
{
    // 1つのウェーブの設定データ
    [System.Serializable]
    public struct WaveData
    {
        public string waveName;          // 例：「Wave 1」
        public GameObject enemyPrefab;   // 出現させる敵の種類
        public int enemyCount;           // そのウェーブで出す敵の数
        public float spawnInterval;      // 敵が出る間隔（秒）
    }

    [Header("ウェーブの設定")]
    [SerializeField] private WaveData[] waves;          // ウェーブのリストデータ
    [SerializeField] private Transform spawnPoint;       // 敵の出現場所
    [SerializeField] private Transform goalTransform;    // 敵の目指すゴール
    [SerializeField] private float waveInterval = 5f;    // 次のウェーブまでの待ち時間

    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI waveText;   // 画面の「Wave 1/3」等の表示
    [SerializeField] private GameObject stageClearUI;    // 「STAGE CLEAR」の画像やパネル

    public static int aliveEnemyCount = 0;               // 【重要】いま画面に生き残っている敵の数
    private int currentWaveIndex = 0;                    // いま何番目のウェーブか

    void Start()
    {
        aliveEnemyCount = 0;

        if (stageClearUI != null)
        {
            stageClearUI.SetActive(false); // 最初はクリア表示を隠しておく
        }

        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        // すべてのウェーブが終わったかチェック
        if (currentWaveIndex >= waves.Length)
        {
            yield break;
        }

        WaveData currentWave = waves[currentWaveIndex];

        // UI表示を更新（例: "Wave 1 / 3"）
        if (waveText != null)
        {
            waveText.text = currentWave.waveName + " (" + (currentWaveIndex + 1) + "/" + waves.Length + ")";
        }

        Debug.Log(currentWave.waveName + " スタート！");

        // 指定された数だけ敵をスポーンさせるループ
        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            if (spawnPoint != null && currentWave.enemyPrefab != null)
            {
                GameObject newEnemy = Instantiate(currentWave.enemyPrefab, spawnPoint.position, Quaternion.identity);
                EnemyMover enemyScript = newEnemy.GetComponent<EnemyMover>();
                if (enemyScript != null && goalTransform != null)
                {
                    enemyScript.SetGoal(goalTransform);
                }
            }

            // 次の敵が出るまで指定秒数待つ
            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        // --- このウェーブの全敵を出し切った後の処理 ---

        // 画面上の敵が0匹になるまで待機する！
        while (aliveEnemyCount > 0)
        {
            yield return null; // 1フレーム待って再チェック
        }

        Debug.Log(currentWave.waveName + " クリア！");

        currentWaveIndex++; // 次のウェーブ番号へ進める

        // まだ次のウェーブがある場合
        if (currentWaveIndex < waves.Length)
        {
            Debug.Log("次のウェーブまであと " + waveInterval + " 秒…");
            yield return new WaitForSeconds(waveInterval); // ウェーブ間の休憩時間
            StartCoroutine(StartNextWave());               // 次のウェーブを開始！
        }
        else
        {
            // 全てのウェーブを撃破！！
            StageClear();
        }
    }

    private void StageClear()
    {
        Debug.Log("【全ウェーブクリア！！】おめでとう！舞台成功！");
        if (stageClearUI != null)
        {
            stageClearUI.SetActive(true); // クリア画面を表示！
        }
    }
}