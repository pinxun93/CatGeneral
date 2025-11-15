using UnityEngine;
using UnityEngine.EventSystems; // 為了滑鼠事件

// 繼承 MonoBehaviour 並實現滑鼠懸停的介面
public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("互動 UI 連結")]
    [Tooltip("點擊後彈出的 UI 介面 GameObject (例如：刻痕特寫)")]
    public GameObject interactivePanel;

    [Header("視覺效果")]
    [Tooltip("用來表現發光效果的 SpriteRenderer 或 Image 元件")]
    public SpriteRenderer highlightRenderer; // 假設你用 SpriteRenderer

    [Tooltip("懸停時的高亮顏色")]
    public Color highlightColor = Color.yellow;

    private Color originalColor;
    private bool isPanelActive = false;

    void Start()
    {
        // 初始化：預設隱藏彈出介面
        if (interactivePanel != null)
        {
            interactivePanel.SetActive(false);
        }

        // 儲存原始顏色 (如果沒有連結，這裡會是 NullReferenceException，請確保連結)
        if (highlightRenderer != null)
        {
            originalColor = highlightRenderer.color;
        }
    }

    // --- 1. 滑鼠懸停/離開邏輯 ---

    // 當滑鼠進入物件範圍時呼叫
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightRenderer != null && !isPanelActive)
        {
            // 變更顏色以實現「發光」效果
            highlightRenderer.color = highlightColor;
        }
    }

    // 當滑鼠離開物件範圍時呼叫
    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightRenderer != null)
        {
            // 恢復原始顏色
            highlightRenderer.color = originalColor;
        }
    }

    // --- 2. 點擊查閱/彈出介面邏輯 ---

    // 當玩家點擊物件時呼叫 (如果物件是 2D Collider)
    void OnMouseDown()
    {
        // 只有在彈出介面沒有激活時才允許再次點擊
        if (!isPanelActive && interactivePanel != null)
        {
            ShowInteractivePanel();
        }
    }

    /// <summary>
    /// 顯示互動介面
    /// </summary>
    public void ShowInteractivePanel()
    {
        interactivePanel.SetActive(true);
        isPanelActive = true;
        // 確保高亮效果在介面彈出時被關閉
        OnPointerExit(null);
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
