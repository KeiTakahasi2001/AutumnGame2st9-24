using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;  // 敵のプレハブ
    [SerializeField] private Transform spawnPoint;    // 【新機能】ここから敵が発生するよ！という場所
    [SerializeField] private Transform goalTransform; // 敵が目指すゴール
    [SerializeField] private float spawnInterval = 3f; // 何秒おきに敵を出すか

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // spawnPointがちゃんと設定されているか確認する
            if (spawnPoint != null && enemyPrefab != null)
            {
                // 1. 「spawnPointの位置」に敵を生み出す！
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                // 2. 生まれたての敵にゴールを教えてあげる
                EnemyMover enemyScript = newEnemy.GetComponent<EnemyMover>();
                if (enemyScript != null && goalTransform != null)
                {
                    enemyScript.SetGoal(goalTransform);
                }
            }
            else
            {
                Debug.LogWarning("SpawnPointかEnemyPrefabが設定されていないよ！");
            }

            // 指定した秒数だけ待つ
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}