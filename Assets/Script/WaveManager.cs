using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public struct WaveData
    {
        public string waveName;
        public GameObject enemyPrefab;
        public int enemyCount;
        public float spawnInterval;
    }

    [Header("ウェーブの設定")]
    [SerializeField] private WaveData[] waves;
    [SerializeField] private WaypointPath targetPath;
    [SerializeField] private float waveInterval = 5f;

    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject stageClearUI;

    public static int aliveEnemyCount = 0;
    private int currentWaveIndex = 0;

    void Start()
    {
        aliveEnemyCount = 0;

        if (stageClearUI != null)
        {
            stageClearUI.SetActive(false);
        }

        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            yield break;
        }

        WaveData currentWave = waves[currentWaveIndex];

        if (waveText != null)
        {
            waveText.text = currentWave.waveName + " (" + (currentWaveIndex + 1) + "/" + waves.Length + ")";
        }

        Debug.Log(currentWave.waveName + " スタート！");

        // 【新機能！】Waveが始まる直前に、3秒間だけ予告ルートを点灯表示する！
        if (targetPath != null)
        {
            targetPath.ShowPathLineForSeconds(3f);
        }

        // 予告線が見えるように少しだけ待ってから敵を出し始める（1.5秒待機）
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            if (currentWave.enemyPrefab != null && targetPath != null)
            {
                GameObject newEnemy = Instantiate(currentWave.enemyPrefab);
                EnemyMover enemyScript = newEnemy.GetComponent<EnemyMover>();

                if (enemyScript != null)
                {
                    enemyScript.SetPath(targetPath);
                }
            }

            yield return new WaitForSeconds(currentWave.spawnInterval);
        }

        while (aliveEnemyCount > 0)
        {
            yield return null;
        }

        Debug.Log(currentWave.waveName + " クリア！");

        currentWaveIndex++;

        if (currentWaveIndex < waves.Length)
        {
            Debug.Log("次のウェーブまであと " + waveInterval + " 秒…");
            yield return new WaitForSeconds(waveInterval);
            StartCoroutine(StartNextWave());
        }
        else
        {
            StageClear();
        }
    }

    private void StageClear()
    {
        Debug.Log("【全ウェーブクリア！！】おめでとう！");
        if (stageClearUI != null)
        {
            stageClearUI.SetActive(true);
        }
    }
}