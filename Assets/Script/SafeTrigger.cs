using UnityEngine;
using UnityEngine.EventSystems; // 處理 UI 阻擋

public class SafeTrigger : MonoBehaviour
{
    public UIManager uiManager;
    public bool isUnlocked = false;


    // 添加 Start 函式來測試初始化 Log
    void Start()
    {
        Debug.Log("SafeTrigger: Start 函式已執行，腳本運行中。"); // 這個 Log 應該在遊戲開始時出現一次
    }
    // 使用 Raycast 偵測，以規避 OnMouseDown 被 UI 阻擋的問題
    void Update()
    {
        // 1. 檢查滑鼠左鍵是否按下，且保險櫃尚未解鎖
        if (Input.GetMouseButtonDown(0) && !isUnlocked)
        {
            // ... (UI 阻擋檢查)

            // 進行 Raycast 偵測
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // 2. 診斷 Raycast 結果
            if (hit.collider == null)
            {
                Debug.LogWarning("SafeTrigger: Raycast 失敗，未擊中任何 Collider。請檢查保險箱是否有 Collider 2D。");
                return;
            }

            // 3. 確認擊中目標是當前這個保險櫃物件
            if (hit.collider.gameObject == gameObject)
            {
                // === 這是開啟密碼介面的關鍵邏輯！請確保它沒有被註釋！ ===
                Debug.Log("SafeTrigger: 點擊成功！開啟密碼介面。");
                if (uiManager != null)
                {
                    uiManager.OpenKeypad(); // 呼叫 UIManager 開啟 Panel
                }
                else
                {
                    Debug.LogError("SafeTrigger Error: UIManager 未連結！請在 Inspector 中連結 GameController。");
                }
            }
            else
            {
                // 擊中了其他 Collider！
                Debug.LogWarning($"SafeTrigger: 點擊失敗！擊中了 {hit.collider.gameObject.name}，而不是保險箱。");
            }
        }
    }
}