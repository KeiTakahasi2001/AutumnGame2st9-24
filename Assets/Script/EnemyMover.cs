using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform goalTransform;// ゴール（右側）の位置をInspectorから指定できるようにする
    [SerializeField] private float moveSpeed = 3f;    // 敵の移動スピード

    void Update()
    {
        if (goalTransform != null)// ゴールが設定されているか確認
        {
            // 現在地からゴールに向かってまっすぐ移動する
            transform.position = Vector3.MoveTowards(transform.position, goalTransform.position, moveSpeed * Time.deltaTime);

            // ゴールにたどり着いたら、自分自身を消滅させる（仮の処理）
            if (Vector3.Distance(transform.position, goalTransform.position) < 0.1f)
            {
                Destroy(gameObject);
                Debug.Log("ゴールに到達された！");
            }
        }
    }
}
