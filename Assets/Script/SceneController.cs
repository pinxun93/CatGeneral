using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    // 如果不需要在 SceneController 內手動控制 DialogueManager，這個連結可以移除
    // public DialogueManager dialogueManager; 

    [Header("場景切換設定")]
    [Tooltip("對話結束後要載入的下一個 Unity 場景名稱")]
    public string nextSceneName = "Chapter1-1-2";

    // 你可以保留 Start() 函式，但保持空白
    void Start()
    {
        // 這裡不執行任何場景切換邏輯
    }

    /// <summary>
    /// 觸發載入下一個遊戲場景
    /// </summary>
    public void LoadNextGameScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("無法切換場景：Next Scene Name 欄位為空！請檢查 Inspector 設定。");
            return;
        }

        // **注意：** 如果這裡成功執行並切換了場景，你之前看到成功的切換就是從這裡來的。
        SceneManager.LoadScene(nextSceneName);

        Debug.Log($"成功載入場景: {nextSceneName}");
    }

    internal void LoadSceneByName(string nextSceneName)
    {
        throw new NotImplementedException();
    }
}