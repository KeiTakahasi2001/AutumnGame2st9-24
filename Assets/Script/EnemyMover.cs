using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Transform goalTransform;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int maxHp = 3;
    private int currentHp;

    private float originalSpeed;
    private float speedMultiplier = 1.0f;

    void Start()
    {
        currentHp = maxHp;
        originalSpeed = moveSpeed;

        // 生み出されたらカウントを +1 する
        WaveManager.aliveEnemyCount++;
    }

    // 誰に消されようと、オブジェクトが消滅する瞬間に絶対に呼ばれるメソッド
    private void OnDestroy()
    {
        // どんな理由で消えたとしても、必ず全体の敵カウントを -1 する！
        WaveManager.aliveEnemyCount--;
    }

    public void SetGoal(Transform newGoal)
    {
        goalTransform = newGoal;
    }

    void Update()
    {
        if (goalTransform != null)
        {
            float currentMoveSpeed = originalSpeed * speedMultiplier;
            transform.position = Vector3.MoveTowards(transform.position, goalTransform.position, currentMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, goalTransform.position) < 0.1f)
            {
                // もうカウント減らす処理は呼ばなくてOK！ただDestroyするだけで OnDestroy() が自動で働く！
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log("敵にダメージ！ 残りHP: " + currentHp);

        if (currentHp <= 0)
        {
            // こちらもただDestroyするだけ！
            Destroy(gameObject);
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}