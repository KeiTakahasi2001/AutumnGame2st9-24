using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [System.Serializable]
    public struct TowerData
    {
        public string towerName;         // タワーの名前（例: 「ケーキスタンド」「チョコ床」）
        public GameObject towerPrefab;   // 配置するタワーのプレハブ
        public int towerCost;            // 置くのに必要な金平糖の数
        public int maxCount;             // 【新機能！】このタワーを置ける最大個数
    }

    [SerializeField] private TowerData[] availableTowers; // 配置できるタワーたちのリスト

    private int selectedTowerIndex = 0;                  // いま何番目のタワーを選んでいるか
    private GameObject previewTower;                     // マウスについてくるプレビュー用タワー

    // 【新機能！】各タワーが「いま何個置かれたか」を種類ごとに数えておく配列
    private int[] placedCounts;

    void Start()
    {
        // タワーの種類数に合わせて、カウント用の箱を用意する
        placedCounts = new int[availableTowers.Length];
        CreateNewPreview();
    }

    void Update()
    {
        // キーボードでタワーを切り替える（1キー、2キー、3キー...）
        if (Input.GetKeyDown(KeyCode.Alpha1) && availableTowers.Length > 0) SwitchTower(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && availableTowers.Length > 1) SwitchTower(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && availableTowers.Length > 2) SwitchTower(2);

        // いま選んでいるタワーがすでに上限に達しているかチェック
        bool isLimitReached = placedCounts[selectedTowerIndex] >= availableTowers[selectedTowerIndex].maxCount;

        if (isLimitReached)
        {
            if (previewTower != null && previewTower.activeSelf)
            {
                previewTower.SetActive(false);
            }
            return;
        }

        if (previewTower != null && !previewTower.activeSelf)
        {
            previewTower.SetActive(true);
        }

        if (previewTower == null)
        {
            CreateNewPreview();
        }

        // マウスの位置にプレビューを追従させる
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        if (previewTower != null)
        {
            previewTower.transform.position = worldPos;
        }

        // 左クリックを離した瞬間に配置する
        if (Input.GetMouseButtonUp(0))
        {
            if (isLimitReached) return;

            int currentCost = availableTowers[selectedTowerIndex].towerCost;

            // お金を払えるかチェック
            if (SugarManager.Instance != null && SugarManager.Instance.TryConsumeSugar(currentCost))
            {
                // プレビューを本物として昇格させる
                previewTower.tag = "Tower";
                SetTowerAlpha(previewTower, 1.0f);

                // 当たり判定を有効にする
                Collider2D[] colliders = previewTower.GetComponentsInChildren<Collider2D>();
                foreach (var col in colliders)
                {
                    col.enabled = true;
                }

                // 「選んでいる種類のタワー」の置いた数を1個増やす！
                placedCounts[selectedTowerIndex]++;
                Debug.Log(availableTowers[selectedTowerIndex].towerName + "を置いたよ！ 現在の数: " + placedCounts[selectedTowerIndex] + " / " + availableTowers[selectedTowerIndex].maxCount);

                previewTower = null;

                // 上限に達していなければ次のプレビューを作る
                if (placedCounts[selectedTowerIndex] < availableTowers[selectedTowerIndex].maxCount)
                {
                    CreateNewPreview();
                }
            }
            else
            {
                Debug.Log("金平糖が足りないよ……！置けません！");
            }
        }
    }

    private void SwitchTower(int index)
    {
        if (selectedTowerIndex == index) return;

        selectedTowerIndex = index;
        Debug.Log("タワーを切り替えたよ: " + availableTowers[selectedTowerIndex].towerName);

        if (previewTower != null)
        {
            Destroy(previewTower);
            previewTower = null;
        }
        CreateNewPreview();
    }

    private void CreateNewPreview()
    {
        if (availableTowers.Length == 0) return;

        // すでに上限に達しているならプレビューを作らない
        if (placedCounts[selectedTowerIndex] >= availableTowers[selectedTowerIndex].maxCount) return;

        GameObject prefabToUse = availableTowers[selectedTowerIndex].towerPrefab;
        previewTower = Instantiate(prefabToUse);
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