using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int maxHp = 3;
    private int currentHp;

    private float originalSpeed;
    private float speedMultiplier = 1.0f;

    private Transform[] waypoints; // 経由地のリスト
    private int targetIndex = 0;   // いま目指している経由地の番号

    void Start()
    {
        currentHp = maxHp;
        originalSpeed = moveSpeed;

        WaveManager.aliveEnemyCount++;
    }

    private void OnDestroy()
    {
        WaveManager.aliveEnemyCount--;
    }

    // 外部（WaveManagerなど）からコース情報を受け取る
    public void SetPath(WaypointPath path)
    {
        if (path != null && path.points.Length > 0)
        {
            waypoints = path.points;
            targetIndex = 0;
            // 最初の出現位置をPoint 0に合わせる
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (waypoints == null || targetIndex >= waypoints.Length) return;

        // 次の経由地を目指して移動
        Transform targetPoint = waypoints[targetIndex];
        float currentMoveSpeed = originalSpeed * speedMultiplier;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, currentMoveSpeed * Time.deltaTime);

        // 経由地に十分近づいたら次の経由地へターゲットを変更！
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
        {
            targetIndex++;

            // 最後の経由地（ゴール）を通り過ぎたら消滅
            if (targetIndex >= waypoints.Length)
            {
                Destroy(gameObject);
                Debug.Log("ゴールに到達された！");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}