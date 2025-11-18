// UIManager.cs
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI 介面連結")]
    public GameObject safeKeypadUI; // 這裡必須有正確連結

    public void OpenKeypad()
    {
        // 檢查目標物件是否為 null
        if (safeKeypadUI == null)
        {
            Debug.LogError("UIManager ERROR: safeKeypadUI 欄位未在 Inspector 中連結！無法開啟介面。");
            return;
        }

        safeKeypadUI.SetActive(true);
        Debug.Log("UIManager: 成功激活 SafeKeypadUI Panel。"); // 添加成功 Log
    }

    public void CloseKeypad()
    {
        if (safeKeypadUI != null) safeKeypadUI.SetActive(false);
    }
}