using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Transform goalTransform; // ゴール（右側）の位置
    [SerializeField] private float moveSpeed = 3f;    // 敵の移動スピード
    [SerializeField] private int maxHp = 3;           // 敵の体力（HP）
    private int currentHp;

    private float originalSpeed;                      // 当初のスピードを記憶しておく変数
    private float speedMultiplier = 1.0f;             // スロー効果などの倍率（1.0なら通常）

    void Start()
    {
        currentHp = maxHp;
        originalSpeed = moveSpeed; // 最初の一歩の速度を保存！
    }

    // 【新追加】スポナーからゴールを受け取るための専用の窓口メソッド！
    public void SetGoal(Transform newGoal)
    {
        goalTransform = newGoal;
    }

    void Update()
    {
        if (goalTransform != null)
        {
            // 【変更】元のスピード × 倍率（スロー等）を掛け合わせて動くようにする！
            float currentMoveSpeed = originalSpeed * speedMultiplier;
            transform.position = Vector3.MoveTowards(transform.position, goalTransform.position, currentMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, goalTransform.position) < 0.1f)
            {
                Destroy(gameObject);
                Debug.Log("ゴールに到達された！");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("敵にダメージ！ 残りHP: " + currentHp);

        if (currentHp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("敵を倒した！やったー！");
        }
    }

    // 【新規追加！】外からスピードの倍率（スロー効果）を変えるためのメソッド
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}