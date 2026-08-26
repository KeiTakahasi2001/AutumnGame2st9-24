using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private int attackPower = 1;     // 攻撃力
    [SerializeField] private float attackInterval = 1f; // 【重要】何秒おきに攻撃するか（1秒に1回なら1f、0.5秒なら0.5f）
    private float attackTimer = 0f;                   // 攻撃までの残り時間を数えるタイマー

    void Update()
    {
        // 攻撃タイマーを毎フレーム減らしていく
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // タイマーが0以下になっていたら攻撃できる！
            if (attackTimer <= 0f)
            {
                EnemyMover enemy = collision.GetComponent<EnemyMover>();

                if (enemy != null)
                {
                    enemy.TakeDamage(attackPower);

                    // 攻撃したので、タイマーをリセットする（例: 1秒間は次の攻撃ができない）
                    attackTimer = attackInterval;
                }
            }
        }
    }
}