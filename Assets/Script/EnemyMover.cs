using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Transform goalTransform; // ゴール（右側）の位置
    [SerializeField] private float moveSpeed = 3f;    // 敵の移動スピード
    [SerializeField] private int maxHp = 3;           // 敵の体力（HP）
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
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
            transform.position = Vector3.MoveTowards(transform.position, goalTransform.position, moveSpeed * Time.deltaTime);

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
}