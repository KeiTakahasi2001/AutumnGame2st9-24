using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // 飛んでいくナイフのプレハブ
    [SerializeField] private float attackInterval = 1f; // 何秒おきに撃つか
    private float attackTimer = 0f;

    private Transform currentTarget; // 今狙っている敵

    void Update()
    {
        // 攻撃タイマーを減らす
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        // 狙っている敵がいなくなったらリセット
        if (currentTarget == null)
        {
            return;
        }

        // タイマーが0以下になったら発射！
        if (attackTimer <= 0f)
        {
            Shoot();
            attackTimer = attackInterval; // タイマーをリセット
        }
    }

    // 範囲内に敵が入ってきたとき（ずっと滞在している間）
    private void OnTriggerStay2D(Collider2D collision)
    {
        // まだターゲットがいなくて、かつ相手が「Enemy」タグならターゲットにする！
        if (currentTarget == null && collision.CompareTag("Enemy"))
        {
            currentTarget = collision.transform;
        }
    }

    // 範囲から敵が出ていったとき
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 狙っていた敵が範囲外に出たらターゲットを外す
        if (currentTarget != null && collision.transform == currentTarget)
        {
            currentTarget = null;
        }
    }

    // ナイフを発射する関数
    private void Shoot()
    {
        if (bulletPrefab == null || currentTarget == null) return;

        // 1. タワーの位置にナイフを生成する
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 2. 生成したナイフの「Bullet」スクリプトを取得して、狙う敵を教える！
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetTarget(currentTarget);
        }
    }
}