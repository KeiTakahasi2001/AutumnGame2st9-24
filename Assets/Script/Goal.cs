using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshProを使うおまじない！

public class Goal : MonoBehaviour
{
    [SerializeField] private int maxLife = 5;
    private int currentLife;

    [SerializeField] private TextMeshProUGUI lifeText; // ⬅️ 追加！画面のライフ表示用

    void Start()
    {
        currentLife = maxLife;
        UpdateLifeUI(); // ⬅️ 最初にも表示を更新！
        Debug.Log("防衛開始！ 残りライフ: " + currentLife);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);

            currentLife--;
            UpdateLifeUI(); // ⬅️ ライフが減ったら文字を更新！
            Debug.Log("大変！お菓子が奪われた……！ 残りライフ: " + currentLife);

            if (currentLife <= 0)
            {
                GameOver();
            }
        }
    }

    // ⬅️ 新しく追加！ライフの文字を書き換える仕組み
    private void UpdateLifeUI()
    {
        if (lifeText != null)
        {
            lifeText.text = "Life : " + currentLife;
        }
    }

    private void GameOver()
    {
        Debug.Log("【GAME OVER】お菓子が全部なくなっちゃった……！");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}