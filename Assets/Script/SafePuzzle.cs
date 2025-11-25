using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class SafePuzzle : MonoBehaviour
{
    [Header("設定")]
    private const string CORRECT_CODE = "32";
    private const string ITEM_TO_GIVE = "破舊貓布偶";

    [Header("元件連結")]
    public InventoryManager inventoryManager;
    public UIManager uiManager;
    public TextMeshProUGUI inputField;
    public GameObject safeDoorObject;

    [Header("UI 反饋")]
    [Tooltip("密碼正確時顯示的 UI 畫面 (請在 Editor 中連結)")]
    public GameObject correctPanel;

    [Tooltip("密碼錯誤時顯示的 UI 畫面 (請在 Editor 中連結)")]
    public GameObject incorrectPanel;

    [Tooltip("密碼錯誤訊息顯示的時間 (秒)")]
    public float incorrectDisplayDuration = 1.5f;

    [Header("狀態與流程控制")]
    [Tooltip("追蹤保險櫃是否已解鎖")]
    public bool isUnlocked = false;

    [Tooltip("連結到保險櫃上的 SafeTrigger 腳本，用於更新點擊狀態")]
    public SafeTrigger safeTriggerToUpdate;

    [Tooltip("解謎成功後要載入的下一個場景名稱")]
    public string nextSceneName = "Room2";

    void Start()
    {
        if (inputField != null) inputField.text = "";

        // 確保所有反饋 UI 都是隱藏的
        if (correctPanel != null) correctPanel.SetActive(false);
        if (incorrectPanel != null) incorrectPanel.SetActive(false);
    }

    // 由 UI 按鈕呼叫：添加數字
    public void AppendNumber(string number)
    {
        if (!isUnlocked && inputField != null && inputField.text.Length < CORRECT_CODE.Length)
        {
            inputField.text += number;
            // 【移除】: 每輸入一個數字就關閉錯誤提示，避免錯誤提示停留太久
            // if (incorrectPanel != null) incorrectPanel.SetActive(false); 
        }
    }

    // 由 UI 按鈕呼叫：清空輸入
    public void ClearInput()
    {
        if (!isUnlocked && inputField != null) inputField.text = "";
        // 【移除】: 清除輸入時，同時關閉錯誤提示
        // if (incorrectPanel != null) incorrectPanel.SetActive(false);
    }

    // 由 Enter 按鈕呼叫：檢查密碼
    public void CheckCode()
    {
        if (isUnlocked) return;

        if (inputField == null) return;

        string currentInput = inputField.text.ToString().Trim();

        if (currentInput == CORRECT_CODE)
        {
            // 密碼正確
            UnlockSafe();
            ShowCorrectFeedback();
        }
        else
        {
            // 密碼錯誤
            // 1. 顯示錯誤 UI (啟用)
            ShowIncorrectFeedback();
            // 2. 清空輸入 (現在 ClearInput 不會立即關閉 UI)
            ClearInput();
        }
    }

    /// <summary>
    /// 顯示密碼正確的 UI 
    /// </summary>
    private void ShowCorrectFeedback()
    {
        if (correctPanel != null)
        {
            // 確保關閉錯誤面板
            if (incorrectPanel != null) incorrectPanel.SetActive(false);
            correctPanel.SetActive(true);
        }
    }

    /// <summary>
    /// 顯示密碼錯誤的 UI 並計時隱藏
    /// </summary>
    private void ShowIncorrectFeedback()
    {
        if (incorrectPanel != null)
        {
            if (correctPanel != null) correctPanel.SetActive(false);

            incorrectPanel.SetActive(true);

            StartCoroutine(HideIncorrectPanelAfterDelay(incorrectDisplayDuration));
        }
    }

    /// <summary>
    /// 延遲一段時間後隱藏密碼錯誤訊息
    /// </summary>
    private IEnumerator HideIncorrectPanelAfterDelay(float delay)
    {
        if (incorrectPanel != null && incorrectPanel.activeSelf)
        {
            yield return new WaitForSeconds(delay);

            if (incorrectPanel != null && incorrectPanel.activeSelf)
            {
                incorrectPanel.SetActive(false);
            }
        }
        else
        {
            yield break;
        }
    }

    private void UnlockSafe()
    {
        isUnlocked = true;

        if (inventoryManager != null) inventoryManager.AddItem(ITEM_TO_GIVE);
        if (safeDoorObject != null) safeDoorObject.SetActive(false);
        if (safeTriggerToUpdate != null)
        {
            safeTriggerToUpdate.isUnlocked = true;
        }

        StartCoroutine(LoadNextSceneAfterDelay(1.5f));
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (uiManager != null) uiManager.CloseKeypad();

            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}