using UnityEngine;
using System.Collections; // 必須要引用才能使用協程 (IEnumerator)
using UnityEngine.SceneManagement; // 必須要引用才能切換場景

public class GoalZone : MonoBehaviour
{
    [Header("場景切換設定")]
    [Tooltip("勝利後要切換到的下一個場景名稱")]
    public string nextSceneName = "NextGameScene";

    [Tooltip("觸發勝利後，等待幾秒才切換場景")]
    public float delayBeforeSceneLoad = 1.0f;

    // 內部旗標，防止玩家重複觸發
    private bool levelCompleted = false;

    /// <summary>
    /// 偵測到碰撞體進入時呼叫
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 檢查是否是玩家，並且尚未完成關卡
        if (other.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true; // 鎖定勝利狀態

            Debug.Log("玩家到達出口！等待 1 秒後切換場景...");

            // 1. 立即停止玩家移動 (防止玩家在等待期間跑掉)
            DisablePlayerMovement(other.gameObject);

            // 2. 啟動協程，開始倒數計時
            StartCoroutine(CompleteLevelAfterDelay());
        }
    }

    /// <summary>
    /// 停止玩家的移動腳本和速度
    /// </summary>
    private void DisablePlayerMovement(GameObject player)
    {
        // 禁用玩家移動腳本
        if (player.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            // 如果您在 PlayerMovement.cs 中定義了 DisableMovement 函式
            // playerMovement.DisableMovement(); 

            // 否則直接禁用腳本：
            playerMovement.enabled = false;
        }

        // 清除 Rigidbody 的速度
        if (player.TryGetComponent<Rigidbody2D>(out Rigidbody2D playerRb))
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// 協程：等待指定時間後載入場景
    /// </summary>
    IEnumerator CompleteLevelAfterDelay()
    {
        // 等待指定時間（例如 1.0 秒）
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // 檢查場景名稱是否有效
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("GoalZone Error: nextSceneName 欄位為空，無法切換場景！");
            yield break; // 停止協程
        }

        // 載入下一個場景
        SceneManager.LoadScene(nextSceneName);
        Debug.Log($"場景切換至：{nextSceneName}");
    }
}