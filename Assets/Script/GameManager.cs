using UnityEngine;
using UnityEngine.SceneManagement;
// 確保您有 using UnityEngine.SceneManagement; 才能使用 SceneManager

public class GameManager : MonoBehaviour
{
    // 【錯誤修正點】：必須在這裡宣告 gameOverPanel 變數
    [Header("UI 連結")]
    [Tooltip("死亡時彈出的整個 Panel (包含文字和按鈕)")]
    public GameObject gameOverPanel;

    [Header("場景管理")]
    [Tooltip("放棄挑戰時要跳回的上一個場景名稱 (例如: Room1)")]
    public string previousSceneName = "Room1";

    private string currentSceneName;

    void Start()
    {
        // 這裡可以使用 gameOverPanel，因為它已在類別頂部宣告
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        currentSceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 觸發遊戲失敗狀態
    /// </summary>
    public void GameOver()
    {
        // 這裡可以使用 gameOverPanel
        if (gameOverPanel == null || gameOverPanel.activeSelf) return;

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        Debug.Log("Game Over! 顯示死亡 UI。");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f; // <--- 必須是第一行，解除凍結！
                             // 之後的載入場景才不會卡住
        SceneManager.LoadScene(currentSceneName);
    }

    public void AbandonLevel()
    {
        Time.timeScale = 1f; // <--- 必須是第一行，解除凍結！
                             // 之後的載入場景才不會卡住
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
    }
}