using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;// 配置するタワーのプレハブを指定する変数

    void Update()
    {
        if (Input.GetMouseButtonDown(0))// マウスの左クリックが押された瞬間
        {
            // 画面上のクリックした位置（マウス座標）を、ゲーム内のワールド座標に変換する
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z; // カメラからの距離を調整
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);//モニターの座標をゲームのワールド座標へ変換

            // Z座標を0に固定する（2Dゲーム用）
            worldPos.z = 0f;

            // 指定した位置にケーキスタンドを生成する
            Instantiate(towerPrefab, worldPos, Quaternion.identity);
            Debug.Log("ケーキスタンドを置いたよ！ 位置: " + worldPos);
        }
    }
}
