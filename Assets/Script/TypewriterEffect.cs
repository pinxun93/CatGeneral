using UnityEngine;
using TMPro; // 記得引入 TextMeshPro 命名空間
using System.Collections; // 協程需要這個命名空間

public class TypewriterEffect : MonoBehaviour
{
    [Header("文本元件")]
    [Tooltip("要顯示對話的 TextMeshPro 元件")]
    public TextMeshProUGUI dialogueText;

    [Header("打字速度設定")]
    [Tooltip("每顯示一個字元間隔的秒數")]
    public float typeSpeed = 0.05f;

    // 儲存目前正在顯示的完整對話
    private string fullText;

    // 用來確保協程在打字過程中不會重複啟動
    private Coroutine typeCoroutine;

    /// <summary>
    /// 開始顯示指定的對話內容
    /// </summary>
    /// <param name="textToDisplay">要顯示的完整字符串</param>
    public void StartDisplay(string textToDisplay)
    {
        // 如果目前有協程正在運行，則停止它
        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }

        // 重置文本並儲存完整的文字
        dialogueText.text = "";
        fullText = textToDisplay;

        // 啟動新的打字協程
        typeCoroutine = StartCoroutine(DisplayingText());
    }

    /// <summary>
    /// 協程：控制文本的逐字顯示
    /// </summary>
    private IEnumerator DisplayingText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            // 將文本一個字元一個字元地添加到顯示元件中
            dialogueText.text += fullText[i];

            // 等待 typeSpeed 設定的時間後再進入下一次循環
            yield return new WaitForSeconds(typeSpeed);
        }

        // 文本完全顯示完成後，可以進行額外的處理，例如：顯示一個「請點擊」的圖示
        // Debug.Log("對話顯示完畢！");
        typeCoroutine = null;
    }

    /// <summary>
    /// 立即顯示完整的文本（用於玩家跳過打字過程）
    /// </summary>
    public void SkipTyping()
    {
        // 確保有協程在運行，並且文本還沒有完全顯示
        if (typeCoroutine != null)
        {
            // 停止打字協程
            StopCoroutine(typeCoroutine);
            typeCoroutine = null;

            // 直接設置為完整的文本
            dialogueText.text = fullText;
        }
    }

    /// <summary>
    /// 檢查文字是否已經顯示完畢
    /// </summary>
    public bool IsTyping()
    {
        return typeCoroutine != null;
    }
}
