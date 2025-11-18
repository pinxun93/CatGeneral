using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("狀態與流程控制")]
    [Tooltip("追蹤保險櫃是否已解鎖")]
    public bool isUnlocked = false;

    [Tooltip("連結到保險櫃上的 SafeTrigger 腳本，用於更新點擊狀態")]
    public SafeTrigger safeTriggerToUpdate;

    [Tooltip("解謎成功後要載入的下一個場景名稱")]
    public string nextSceneName = "Room2";

    void Start()
    {
        // 確保密碼框是空的
        if (inputField != null) inputField.text = "";

        // 注意：這裡已經移除了所有關於 sceneController 的尋找邏輯，
        // 因為我們不再需要它。
    }

    // 由 UI 按鈕呼叫：添加數字
    public void AppendNumber(string number)
    {
        if (!isUnlocked && inputField != null && inputField.text.Length < CORRECT_CODE.Length)
        {
            inputField.text += number;
        }
    }

    // 由 UI 按鈕呼叫：清空輸入
    public void ClearInput()
    {
        if (!isUnlocked && inputField != null) inputField.text = "";
    }

    // 由 Enter 按鈕呼叫：檢查密碼
    public void CheckCode()
    {
        if (isUnlocked) return;

        if (inputField != null && inputField.text == CORRECT_CODE)
        {
            UnlockSafe();
        }
        else
        {
            Debug.LogWarning($"密碼錯誤！請重試。");
            ClearInput();
        }
    }

    private void UnlockSafe()
    {
        // 鎖定狀態，防止重複解鎖
        isUnlocked = true;
        Debug.Log("保險櫃已解鎖！");

        // 1. 給予物品
        if (inventoryManager != null) inventoryManager.AddItem(ITEM_TO_GIVE);

        // 2. 視覺變化
        if (safeDoorObject != null) safeDoorObject.SetActive(false);

        // 3. 關閉密碼介面
        if (uiManager != null) uiManager.CloseKeypad();

        // 4. 更新 SafeTrigger 狀態
        if (safeTriggerToUpdate != null)
        {
            safeTriggerToUpdate.isUnlocked = true;
        }

        // 5. 【最終場景轉換】：直接使用 Unity 核心 SceneManager 載入場景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // 使用 Application.CanStreamedLevelBeLoaded 來檢查場景是否存在 (更安全)
            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                Debug.Log($"SafePuzzle: 成功載入場景 -> {nextSceneName}");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                // 如果場景名稱不正確或不在 Build Settings，這裡會報錯
                Debug.LogError($"SafePuzzle Error: 場景 '{nextSceneName}' 不存在或未添加到 Build Settings 中！");
            }
        }
        else
        {
            Debug.LogError("SafePuzzle Error: nextSceneName 未設置！無法轉換場景。");
        }
    }
}