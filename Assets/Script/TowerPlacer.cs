using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab; // 配置するタワーのプレハブ
    [SerializeField] private int maxTowerCount = 3;  // 置けるタワーの最大数
    private GameObject previewTower;                 // マウスについてくるプレビュー用タワー
    private int placedTowerCount = 0;                // 【重要】自分でいま何個置いたかを正確に覚える変数！

    void Start()
    {
        // 最初にプレビューを1個作る
        CreateNewPreview();
    }

    void Update()
    {
        // 1. すでに上限数に達している場合
        if (placedTowerCount >= maxTowerCount)
        {
            // プレビューが存在していれば、非表示にして完全にストップ
            if (previewTower != null && previewTower.activeSelf)
            {
                previewTower.SetActive(false);
            }
            return;
        }

        // 2. 上限未満のとき、プレビューが非表示なら再表示する
        if (previewTower != null && !previewTower.activeSelf)
        {
            previewTower.SetActive(true);
        }

        if (previewTower == null)
        {
            CreateNewPreview();
        }

        // 3. マウスの位置にプレビューを追従させる
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        previewTower.transform.position = worldPos;

        // 4. 左クリックを離した瞬間に配置する
        if (Input.GetMouseButtonUp(0))
        {
            if (placedTowerCount >= maxTowerCount) return;

            // プレビューを本物として昇格させる
            previewTower.tag = "Tower";
            SetTowerAlpha(previewTower, 1.0f);

            // 当たり判定を有効にする
            Collider2D[] colliders = previewTower.GetComponentsInChildren<Collider2D>();
            foreach (var col in colliders)
            {
                col.enabled = true;
            }

            // 置いた数を1個増やす！
            placedTowerCount++;
            Debug.Log("ケーキスタンドを置いたよ！ 現在の数: " + placedTowerCount + " / " + maxTowerCount);

            // プレビューの役目を終えたので空にする
            previewTower = null;

            // まだ上限に達していなければ、次のプレビューを作る
            if (placedTowerCount < maxTowerCount)
            {
                CreateNewPreview();
            }
        }
    }

    private void CreateNewPreview()
    {
        previewTower = Instantiate(towerPrefab);
        SetTowerAlpha(previewTower, 0.5f);

        Collider2D[] colliders = previewTower.GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
    }

    private void SetTowerAlpha(GameObject target, float alpha)
    {
        SpriteRenderer[] renderers = target.GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            if (r.gameObject.name == "AttackRange")
            {
                continue;
            }

            Color col = r.color;
            col.a = alpha;
            r.color = col;
        }
    }
}