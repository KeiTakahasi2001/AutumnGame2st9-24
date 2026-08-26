using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)// センサー（Circle Collider 2D）の範囲内に何かが入ってきた瞬間
    {
        if (collision.CompareTag("Enemy"))// ぶつかってきた相手が「Enemy」というタグ（目印）を持っているか確認
        {
            // 仮の攻撃：範囲内に入ったら一撃で敵を消滅させる！（あとでHPシステムに改造できます）
            Destroy(collision.gameObject);
            Debug.Log("タワーが敵を攻撃して倒した！");
        }
    }
}