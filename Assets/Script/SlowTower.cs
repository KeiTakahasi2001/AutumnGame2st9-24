using UnityEngine;

public class SlowTower : MonoBehaviour
{
    [SerializeField] private float slowMultiplier = 0.5f; // 速度を何倍にするか（0.5なら半分のスピードに！）

    // 範囲内に敵が「入っている間ずっと」毎フレーム呼ばれる
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyMover enemy = collision.GetComponent<EnemyMover>();
            if (enemy != null)
            {
                // 範囲内にいる間は、ずっとスロー倍率を適用する！
                enemy.SetSpeedMultiplier(slowMultiplier);
            }
        }
    }

    // 範囲から敵が「出ていった瞬間」に呼ばれる
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyMover enemy = collision.GetComponent<EnemyMover>();
            if (enemy != null)
            {
                // 範囲を出たら、スピードの倍率を「1.0（通常速度）」に戻す！
                enemy.SetSpeedMultiplier(1.0f);
            }
        }
    }
}