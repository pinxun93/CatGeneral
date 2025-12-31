using System;
using UnityEngine;

/// <summary>
/// 擴展版 UIManager：負責控制 UI 開啟與遊戲互動狀態。
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI 介面連結")]
    public GameObject safeKeypadUI;

    // 靜態屬性，讓場景中其他的腳本可以輕易檢查目前的狀態
    public static bool IsUIPanelOpen { get; private set; } = false;

    /// <summary>
    /// 開啟保險箱密碼介面，並鎖定場景互動
    /// </summary>
    public void OpenKeypad()
    {
        if (safeKeypadUI == null)
        {
            Debug.LogError("UIManager ERROR: safeKeypadUI 欄位未連結！");
            return;
        }

        safeKeypadUI.SetActive(true);
        IsUIPanelOpen = true; // 設置鎖定狀態

        // 暫停所有 Interactable.cs 腳本的功能
        ToggleAllInteractables(false);

        // 解鎖滑鼠游標，讓玩家可以點擊 UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("UIManager: 介面已開啟，場景中所有 Interactable 腳本已禁用。");
    }

    /// <summary>
    /// 關閉保險箱密碼介面，並恢復場景互動
    /// </summary>
    public void CloseKeypad()
    {
        if (safeKeypadUI != null)
        {
            safeKeypadUI.SetActive(false);
            IsUIPanelOpen = false; // 解除鎖定狀態

            // 恢復所有 Interactable.cs 腳本的功能
            ToggleAllInteractables(true);

            Debug.Log("UIManager: 介面已關閉，場景中所有 Interactable 腳本已恢復。");
        }
    }

    /// <summary>
    /// 統一啟用或禁用場景中的 Interactable 腳本組件
    /// </summary>
    /// <param name="enabledState">true 為啟用，false 為禁用</param>
    private void ToggleAllInteractables(bool enabledState)
    {
        // 修正：移除可能導致錯誤的 FindObjectsInactive 參數
        // 使用更具相容性的語法，只傳遞排序模式
        Interactable[] interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);

        if (interactables == null || interactables.Length == 0)
        {
            Debug.Log("UIManager: 場景中目前沒有找到任何 Interactable 物件。");
            return;
        }

        foreach (var script in interactables)
        {
            // 直接控制腳本組件的啟用狀態
            if (script != null)
            {
                script.enabled = enabledState;
            }
        }
    }

    /// <summary>
    /// 顯示訊息的功能
    /// </summary>
    public void ShowMessage(string message)
    {
        Debug.Log("遊戲訊息: " + message);
    }
}