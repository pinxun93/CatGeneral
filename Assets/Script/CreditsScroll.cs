using UnityEngine;
using TMPro; // 必須使用 TextMeshPro
using UnityEngine.SceneManagement; // 用於切換回主菜單等

/// <summary>
/// 控制電影風格的製作人員名單，使其從底部緩慢滾動到頂部。
/// </summary>
public class CreditsScroll : MonoBehaviour
{
    [Header("滾動設定")]
    [Tooltip("名單向上滾動的速度 (像素/秒)")]
    public float scrollSpeed = 50f;

    [Tooltip("滾動結束後，延遲多久自動切換場景或執行其他動作")]
    public float endDelay = 5f;

    [Header("結束動作")]
    [Tooltip("滾動結束後要載入的場景名稱 (例如：MainMenu)")]
    public string nextSceneName = "MainMenu";

    private RectTransform creditsRectTransform;
    private float totalHeight;
    private bool isScrolling = true;
    private float startTime;

    void Start()
    {
        // 1. 獲取 RectTransform，這是控制 UI 位置的關鍵
        creditsRectTransform = GetComponent<RectTransform>();

        if (creditsRectTransform == null)
        {
            Debug.LogError("CreditsScroll 錯誤: 找不到 RectTransform 元件！請將此腳本掛載到 TextMeshProUGUI 物件上。");
            return;
        }

        // 2. 獲取內容的高度
        // 為了確保內容完全滾過螢幕，我們需要知道內容的實際高度。
        // 這要求 TextMeshProUGUI 上的 RectTransform 必須能包住所有文本。
        // 我們使用 preferredHeight 作為估計，但通常手動設置 RectTransform 的 height 屬性更可靠。
        // 為了通用性，我們使用當前 RectTransform 的 height 作為總高度。
        totalHeight = creditsRectTransform.rect.height;

        // 3. 初始位置設置 (從 Canvas 底部開始)
        // 將名單的 Y 座標設置到 Canvas 底部下方 (即 -高度的一半，確保整個文本內容位於視窗外)
        // 假設 TextMeshPro 的 Pivot Y = 0.5 (居中)
        // 我們將其錨點(Anchor)設置在底部中心 (0.5, 0)，並將 Y 位置調整到 Canvas 底部
        creditsRectTransform.anchoredPosition = new Vector2(0, -Screen.height / 2f);

        startTime = Time.time;
    }

    void Update()
    {
        if (isScrolling)
        {
            // 向上移動 RectTransform
            // deltaY = 速度 * 時間間隔
            float deltaY = scrollSpeed * Time.deltaTime;

            // 創建新的位置
            Vector2 newPosition = creditsRectTransform.anchoredPosition;
            newPosition.y += deltaY;

            creditsRectTransform.anchoredPosition = newPosition;

            // 檢查是否滾動完成
            // 如果當前位置 (newPosition.y) 超過了 Canvas 高度 + 內容高度，則停止。
            // 判斷條件：當名單的底部 (即 Y=0 的位置) 滾動到螢幕頂部時停止。
            // 這裡假設名單物件的錨點在底部中央 (0.5, 0)。
            float screenHeight = Screen.height;
            if (newPosition.y >= screenHeight + totalHeight)
            {
                isScrolling = false;
                Debug.Log("名單滾動結束。");
                Invoke("TriggerEndAction", endDelay); // 延遲後執行結束動作
            }
        }
    }

    /// <summary>
    /// 滾動結束後執行的動作
    /// </summary>
    private void TriggerEndAction()
    {
        if (!string.IsNullOrEmpty(nextSceneName) && Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("名單結束動作: 未設定下一個場景，或場景名稱錯誤，將停留原地。");
        }
    }
}