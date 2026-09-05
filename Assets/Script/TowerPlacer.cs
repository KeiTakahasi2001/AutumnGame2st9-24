using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [System.Serializable]
    public struct TowerData
    {
        public string towerName;         // タワーの名前（例: 「ケーキスタンド」「チョコ床」）
        public GameObject towerPrefab;   // 配置するタワーのプレハブ
        public int towerCost;            // 置くのに必要な金平糖の数
        public int maxCount;             // このタワーを置ける最大個数
        public LayerMask placementLayer; // 【新機能！】このタワーが「置ける」レイヤーを指定！
    }

    [SerializeField] private TowerData[] availableTowers; // 配置できるタワーたちのリスト

    private int selectedTowerIndex = 0;                  // いま何番目のタワーを選んでいるか
    private GameObject previewTower;                     // マウスについてくるプレビュー用タワー

    private int[] placedCounts;                          // 各タワーの配置数カウント

    void Start()
    {
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

        // マウスのワールド座標を取得
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        // 【新機能！】現在選んでいるタワーの「指定レイヤー」の上にマウスがあるかチェック！
        LayerMask targetLayer = availableTowers[selectedTowerIndex].placementLayer;
        bool isValidPlacement = Physics2D.OverlapCircle(worldPos, 0.2f, targetLayer);

        // 指定されたレイヤーの上でない（＝置けない場所）ときはプレビューを消す
        if (!isValidPlacement)
        {
            if (previewTower != null && previewTower.activeSelf)
            {
                previewTower.SetActive(false);
            }
            return; // 置けない場所なら配置処理を行わない！
        }

        // 置ける場所ならプレビューを表示
        if (previewTower != null && !previewTower.activeSelf)
        {
            previewTower.SetActive(true);
        }

        if (previewTower == null)
        {
            CreateNewPreview();
        }

        if (previewTower != null)
        {
            previewTower.transform.position = worldPos;
        }

        // 左クリックを離した瞬間に配置する（正しいレイヤーの上だけのとき）
        if (Input.GetMouseButtonUp(0))
        {
            if (isLimitReached) return;

            int currentCost = availableTowers[selectedTowerIndex].towerCost;

            // お金を払えるかチェック
            if (SugarManager.Instance != null && SugarManager.Instance.TryConsumeSugar(currentCost))
            {
                previewTower.tag = "Tower";
                SetTowerAlpha(previewTower, 1.0f);

                Collider2D[] colliders = previewTower.GetComponentsInChildren<Collider2D>();
                foreach (var col in colliders)
                {
                    col.enabled = true;
                }

                placedCounts[selectedTowerIndex]++;
                Debug.Log(availableTowers[selectedTowerIndex].towerName + "を置いたよ！ 現在の数: " + placedCounts[selectedTowerIndex] + " / " + availableTowers[selectedTowerIndex].maxCount);

                previewTower = null;

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