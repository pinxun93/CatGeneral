using UnityEngine;
using TMPro;

public class SafePuzzle : MonoBehaviour
{
    [Header("設定")]
    private const string CORRECT_CODE = "32";
    private const string ITEM_TO_GIVE = "破舊貓布偶";

    [Header("元件連結")]
    public InventoryManager inventoryManager;
    public UIManager uiManager;
    public TextMeshProUGUI inputField;
    public GameObject safeDoorObject; // 用於隱藏或動畫的門

    [Header("狀態")]
    public bool isUnlocked = false; // 追蹤是否已解鎖

    [Header("解鎖後狀態更新")]
    [Tooltip("連結到你保險櫃上的 SafeTrigger 腳本")]
    public SafeTrigger safeTriggerToUpdate; // <<<<< 新增這個連結
    void Start()
    {
        // 確保密碼框是空的
        if (inputField != null) inputField.text = "";
    }

    // 由 UI 按鈕呼叫
    public void AppendNumber(string number)
    {
        if (!isUnlocked && inputField.text.Length < CORRECT_CODE.Length)
        {
            inputField.text += number;
        }
    }

    // 由 UI 按鈕呼叫
    public void ClearInput()
    {
        if (!isUnlocked) inputField.text = "";
    }

    // 由 Enter 按鈕呼叫
    public void CheckCode()
    {
        if (isUnlocked) return;

        if (inputField.text == CORRECT_CODE)
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
        isUnlocked = true;
        Debug.Log("保險櫃已解鎖！");

        // 1. 給予物品
        if (inventoryManager != null) inventoryManager.AddItem(ITEM_TO_GIVE);

        // 2. 視覺變化 (門打開)
        if (safeDoorObject != null) safeDoorObject.SetActive(false);

        // 3. 關閉密碼介面
        if (uiManager != null) uiManager.CloseKeypad();

        // 4. (重要) 通過 Inspector 連結更新狀態，安全且高效
        if (safeTriggerToUpdate != null)
        {
            safeTriggerToUpdate.isUnlocked = true;
        }
    }
}