using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;     // ナイフが飛ぶスピード
    [SerializeField] private int damage = 1;        // ナイフの攻撃力
    private Transform target;                       // 狙う敵の場所

    // 撃たれたときに、どの敵に向かうかをセットする関数
    public void SetTarget(Transform enemyTransform)
    {
        target = enemyTransform;
    }

    void Update()
    {
        // ターゲット（敵）がいなくなったら、ナイフ自身も消える
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 敵の方向に向かってまっすぐ進む
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // ※おまけ：ナイフの向きを敵の方向にスッと向かせる場合（2D用）
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f)); // ナイフの画像の向きによって -90f は調整してね！
    }

    // 敵にぶつかった瞬間！
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 敵のHP（EnemyMoverなど）にダメージを与える
            EnemyMover enemy = collision.GetComponent<EnemyMover>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 敵に当たったらナイフは消滅する
            Destroy(gameObject);
        }
    }
}