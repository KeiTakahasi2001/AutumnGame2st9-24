using UnityEngine;

public class EnemyMover : MonoBehaviour // ファイル名に合わせてクラス名はそのままでOKです！
{
    [SerializeField] private Transform goalTransform; // ゴール（右側）の位置
    [SerializeField] private float moveSpeed = 3f;    // 敵の移動スピード
    [SerializeField] private int maxHp = 3;           // 【新機能】敵の体力（HP）！インスペクターで変更できます
    private int currentHp;                            // 現在の体力

    void Start()
    {
        currentHp = maxHp; // 生まれたときに体力を満タンにする
    }

    void Update()
    {
        if (goalTransform != null)
        {
            // ゴールに向かってまっすぐ移動
            transform.position = Vector3.MoveTowards(transform.position, goalTransform.position, moveSpeed * Time.deltaTime);

            // ゴールにたどり着いたら消滅
            if (Vector3.Distance(transform.position, goalTransform.position) < 0.1f)
            {
                Destroy(gameObject);
                Debug.Log("ゴールに到達された！");
            }
        }
    }

    // 【超重要】タワーから攻撃を受けたときに呼ばれるメソッド！
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("敵にダメージ！ 残りHP: " + currentHp);

        // HPが0以下になったら消滅（撃破！）
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("敵を倒した！やったー！");
        }
    }
}