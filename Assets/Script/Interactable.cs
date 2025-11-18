using UnityEngine;
using UnityEngine.EventSystems; // 處理 UI 事件阻擋

// 腳本名稱保持為 Interactable (與你的檔案名一致)
public class Interactable : MonoBehaviour
{
    [Header("UI & 功能連結")]
    [Tooltip("點擊後彈出的 UI 介面 GameObject (例如：WallCarvingPanel)")]
    public GameObject interactivePanel;

    [Header("視覺元件")]
    [Tooltip("用於控制圖像和顏色的 SpriteRenderer")]
    public SpriteRenderer visualRenderer;

    [Header("視覺效果設定")]
    public Color highlightColor = Color.yellow;
    private Color originalColor;

    private bool isPanelActive = false; // 追蹤面板是否已彈出

    void Start()
    {
        // 確保初始狀態是隱藏的
        if (interactivePanel != null)
        {
            interactivePanel.SetActive(false);
        }

        // 嘗試自動獲取 SpriteRenderer
        if (visualRenderer == null)
        {
            visualRenderer = GetComponent<SpriteRenderer>();
        }

        if (visualRenderer != null)
        {
            originalColor = visualRenderer.color;
        }
        else
        {
            Debug.LogError($"[Interactable] ERROR: {gameObject.name} 找不到 SpriteRenderer 元件！發光功能將失敗！");
        }
    }

    // --- 1. 滑鼠懸停/離開邏輯 (使用 OnMouse 事件) ---

    // 當滑鼠進入 Collider 範圍時觸發 (發亮)
    void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter 觸發！嘗試改變顏色..."); // <<< 新增這個 Log
        // 只有在面板沒有激活時才發光，且確保滑鼠沒有在 UI 上
        if (visualRenderer != null && !isPanelActive && !EventSystem.current.IsPointerOverGameObject())
        {
            visualRenderer.color = highlightColor;
        }
    }

    // 當滑鼠離開 Collider 範圍時觸發 (恢復)
    void OnMouseExit()
    {
        if (visualRenderer != null)
        {
            visualRenderer.color = originalColor;
        }
    }

    // --- 2. 點擊彈出介面邏輯 (Raycast 偵測，解決 OnMouseDown 被 UI 阻擋的問題) ---

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPanelActive)
        {
            // 進行 Raycast 偵測
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // 確認擊中目標是當前這個 GameObject
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ShowInteractivePanel();
                OnMouseExit();
            }
        }
    }

    /// <summary>
    /// 顯示互動介面
    /// </summary>
    public void ShowInteractivePanel()
    {
        if (interactivePanel != null)
        {
            interactivePanel.SetActive(true);
            isPanelActive = true;
        }
    }

    /// <summary>
    /// 外部呼叫：退出查閱介面
    /// </summary>
    public void CloseInteractivePanel()
    {
        if (interactivePanel != null)
        {
            interactivePanel.SetActive(false);
            isPanelActive = false;
        }
    }
}